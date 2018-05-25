using System.Collections.Generic;
using System.Reflection;
using System;
using SimpleJSON;
using UnityEngine;
using HoloToolkit.Unity;
using ROSBridgeLib;

#if UNITY_EDITOR
using System.Threading;
using WebSocketSharp;
#endif

#if !UNITY_EDITOR //need to run hololens
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.Networking;
using Windows.Foundation;
using Windows.UI.Core;
#endif


/**
 * This class handles the connection with the external ROS world, deserializing
 * json messages into appropriate instances of packets and messages.
 * 
 * This class also provides a mechanism for having the callback's exectued on the rendering thread.
 * (Remember, Unity has a single rendering thread, so we want to do all of the communications stuff away
 * from that. 
 * 
 * The one other clever thing that is done here is that we only keep 1 (the most recent!) copy of each message type
 * that comes along.
 * 
 * Version History
 * Modified from Version 3.1 by Cole Shing, 2017
 * Modified to get it to work in both unity or Hololens
 * @author Michael Jenkin, Robert Codd-Downey and Andrew Speers
 * @version 3.1
 * 
 * Modified by Cole Shing to work in both Unity and Hololens(or any windows device)
 * 2017
 */

namespace ROSBridgeLib
{
    public class ROSBridgeWebSocketConnection : Singleton<ROSBridgeWebSocketConnection>
    {
        private class RenderTask {
            private Type _subscriber;
            private string _topic;
            private ROSBridgeMsg _msg;

            public RenderTask(Type subscriber, string topic, ROSBridgeMsg msg) {
                _subscriber = subscriber;
                _topic = topic;
                _msg = msg;
            }

            public Type getSubscriber() {
                return _subscriber;
            }

            public ROSBridgeMsg getMsg() {
                return _msg;
            }

            public string getTopic() {
                return _topic;
            }
        }; //end of private rendertask
        private string _host;
        private int _port;

#if UNITY_EDITOR
        private WebSocket _ws;
        private System.Threading.Thread _myThread;
#endif

        //WebSocket client from Windows.Networking.Sockets
#if !UNITY_EDITOR
        private MessageWebSocket messageWebSocket; //same as _ws
        Uri server;
        private DataWriter dataWriter; //to write the data to the socket
#endif

        private List<Type> _subscribers; // our subscribers
        private List<Type> _publishers; //our publishers
        private Type _serviceResponse; // to deal with service responses
        private string _serviceName = null; 
        private string _serviceValues = null; 
        private List<RenderTask> _taskQ = new List<RenderTask>(); //list to render tasks

        private object _queueLock = new object(); //na

        private static string GetMessageType(Type t) {
            return (string)t.GetMethod("GetMessageType", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).Invoke(null, null);
        }

        private static string GetMessageTopic(Type t) {
            return (string)t.GetMethod("GetMessageTopic", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).Invoke(null, null);
        }

        private static string GetQueueLength(Type t)
        {
            return (string)t.GetMethod("GetQueueLength", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).Invoke(null, null);
        }

        private static ROSBridgeMsg ParseMessage(Type t, JSONNode node) {
            return (ROSBridgeMsg)t.GetMethod("ParseMessage", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).Invoke(null, new object[] { node });
        }

        private static void Update(Type t, ROSBridgeMsg msg) {
            t.GetMethod("CallBack", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).Invoke(null, new object[] { msg });
        }

        private static void ServiceResponse(Type t, string service, string yaml) {
            t.GetMethod("ServiceCallBack", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).Invoke(null, new object[] { service, yaml });
        }

        private static void IsValidServiceResponse(Type t) {
            if (t.GetMethod("ServiceCallBack", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy) == null)
                throw new Exception("invalid service response handler");
        }

        private static void IsValidSubscriber(Type t) {
            if (t.GetMethod("CallBack", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy) == null)
                throw new Exception("missing Callback method");
            if (t.GetMethod("GetMessageType", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy) == null)
                throw new Exception("missing GetMessageType method");
            if (t.GetMethod("GetMessageTopic", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy) == null)
                throw new Exception("missing GetMessageTopic method");
            if (t.GetMethod("ParseMessage", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy) == null)
                throw new Exception("missing ParseMessage method");
        }
        
        private static void IsValidPublisher(Type t) {
            if (t.GetMethod("GetMessageType", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy) == null)
                throw new Exception("missing GetMessageType method");
            if (t.GetMethod("GetMessageTopic", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy) == null)
                throw new Exception("missing GetMessageTopic method");
        }

        /**
		 * Make a connection to a host/port. 
		 * This does not actually start the connection, use Connect to do that.
		 */
        public ROSBridgeWebSocketConnection(string host, int port) {
            _host = host;
            _port = port;

#if UNITY_EDITOR
            _myThread = null;
#endif
            _subscribers = new List<Type>();
            _publishers = new List<Type>();
        }

        /**
		 * Add a service response callback to this connection.
		 */
        public void AddServiceResponse(Type serviceResponse) {
            IsValidServiceResponse(serviceResponse);
            _serviceResponse = serviceResponse;
        }

        /**
		 * Add a subscriber callback to this connection. There can be many subscribers.
		 */
        public void AddSubscriber(Type subscriber) {
            IsValidSubscriber(subscriber);
            _subscribers.Add(subscriber);
        }

        /**
		 * Add a publisher to this connection. There can be many publishers.
		 */
        public void AddPublisher(Type publisher) {
            IsValidPublisher(publisher);
            _publishers.Add(publisher);
        }

        /**
         * Remove a subscriber callback to this connection.
         */
        public void RemoveSubscriber(Type subscriber)
        {
            IsValidSubscriber(subscriber);
            _subscribers.Remove(subscriber);
#if UNITY_EDITOR
            _ws.Send(ROSBridgeMsg.UnSubscribe(GetMessageTopic(subscriber)));
            Debug.Log("Sending " + ROSBridgeMsg.UnSubscribe(GetMessageTopic(subscriber)));
#endif

#if !UNITY_EDITOR
            string s = ROSBridgeMsg.UnSubscribe(GetMessageTopic(subscriber));
            dataWriter.WriteString(s);
            dataWriter.StoreAsync();
#endif
        }

        /**
         * Remove a publisher to this connection.
         */
        public void RemovePublisher(Type publisher)
        {
            IsValidPublisher(publisher);
            _publishers.Remove(publisher);
#if UNITY_EDITOR
            _ws.Send(ROSBridgeMsg.UnAdvertise(GetMessageTopic(publisher)));
            Debug.Log("Sending " + ROSBridgeMsg.UnAdvertise(GetMessageTopic(publisher)));
#endif

#if !UNITY_EDITOR
            string s = ROSBridgeMsg.UnAdvertise(GetMessageTopic(publisher));
            dataWriter.WriteString(s);
            dataWriter.StoreAsync();
#endif
        }

        /**
		 * Connect to the remote ros environment.
		 */
#if UNITY_EDITOR
        public void Connect()
        {
            _myThread = new System.Threading.Thread(Run);
            _myThread.Start();
        }
#endif

#if !UNITY_EDITOR
        public async void Connect() {
            MessageWebSocket webSocket = messageWebSocket;
            if (webSocket == null)
            {
                webSocket = new MessageWebSocket();
                server = new Uri(_host + ":" + _port.ToString()); //create the socket server

                webSocket.MessageReceived += MessageReceived; //callback

                await webSocket.ConnectAsync(server);
                messageWebSocket = webSocket;
                dataWriter = new DataWriter(webSocket.OutputStream);
            }
            foreach (Type p in _subscribers) {
                string s = ROSBridgeMsg.Subscribe (GetMessageTopic(p), GetMessageType (p)); 
                dataWriter.WriteString(s);
                await dataWriter.StoreAsync();
            }
            foreach (Type p in _publishers) {
                string s = ROSBridgeMsg.Advertise(GetMessageTopic(p), GetMessageType(p));
                dataWriter.WriteString(s);
                await dataWriter.StoreAsync();
            }
        }
#endif

        /**
		 * Disconnect from the remote ros environment.
         * need to add unsubscribe using hololens as well
		 */
        public void Disconnect() {
#if UNITY_EDITOR
            _myThread.Abort ();
		 	foreach(Type p in _subscribers) {
		 		_ws.Send(ROSBridgeMsg.UnSubscribe(GetMessageTopic(p)));
		 		Debug.Log ("Sending " + ROSBridgeMsg.UnSubscribe(GetMessageTopic(p)));
		 	}
		 	foreach(Type p in _publishers) {
		 		_ws.Send(ROSBridgeMsg.UnAdvertise (GetMessageTopic(p)));
		 		Debug.Log ("Sending " + ROSBridgeMsg.UnAdvertise(GetMessageTopic(p)));
		 	}
		 	_ws.Close ();
#endif

#if !UNITY_EDITOR
            foreach(Type p in _subscribers) {
                string s = ROSBridgeMsg.UnSubscribe(GetMessageTopic(p));
                dataWriter.WriteString(s);
                dataWriter.StoreAsync();
		 	}
		 	foreach(Type p in _publishers) {
                string s = ROSBridgeMsg.UnAdvertise(GetMessageTopic(p));
                dataWriter.WriteString(s);
                dataWriter.StoreAsync();
		 	}
            messageWebSocket.Dispose();
            messageWebSocket = null;
#endif
        }

        /*starting connection betwen unity and ROS
         *only ran in unity editor
         */
        private void Run() {
#if UNITY_EDITOR
            _ws = new WebSocket(_host + ":" + _port);
		 	_ws.OnMessage += (sender, e) => this.OnMessage(e.Data);
		 	_ws.Connect();

		 	foreach(Type p in _subscribers) {

                //if (GetMessageTopic(p) == "/image_converter/output_video")
                //{
                //    _ws.Send(ROSBridgeMsg.Subscribe(GetMessageTopic(p), GetMessageType(p), GetQueueLength(p)));
                //}
                //else
                //{
                    _ws.Send(ROSBridgeMsg.Subscribe(GetMessageTopic(p), GetMessageType(p)));
                    Debug.Log("Sending " + ROSBridgeMsg.Subscribe(GetMessageTopic(p), GetMessageType(p)));
                //}
		 	}
		 	foreach(Type p in _publishers) {
		 		_ws.Send(ROSBridgeMsg.Advertise (GetMessageTopic(p), GetMessageType(p)));
		 		Debug.Log ("Sending " + ROSBridgeMsg.Advertise (GetMessageTopic(p), GetMessageType(p)));
		 	}
		 	while(true) {
		 		Thread.Sleep (1000);
		 	}
#endif
        }

#if UNITY_EDITOR
        //the callback from the websocket, adds task to which calls the render task
        private void OnMessage(string s) {
		 	//Debug.Log ("Got a message " + s);
            if ((s!= null) && !s.Equals ("")) {
		 		JSONNode node = JSONNode.Parse(s);
                //Debug.Log ("Parsed it");
                string op = node["op"];
                //Debug.Log ("Operation is " + op);
                if ("publish".Equals (op)) {
		 			string topic = node["topic"];
                    //Debug.Log ("Got a message on " + topic);
		 			foreach(Type p in _subscribers) {
		 				if(topic.Equals (GetMessageTopic (p))) {
                            //Debug.Log ("And will parse it " + GetMessageTopic (p));
                            ROSBridgeMsg msg = ParseMessage(p, node["msg"]);
		 					RenderTask newTask = new RenderTask(p, topic, msg);
		 					lock(_queueLock) {
		 						bool found = false;
		 						for(int i=0;i<_taskQ.Count;i++) {
		 							if(_taskQ[i].getTopic().Equals (topic)) {
		 								_taskQ.RemoveAt (i);
		 								_taskQ.Insert (i, newTask);
		 								found = true;
		 								break;
		 							}
		 						}
		 						if(!found)
		 						_taskQ.Add (newTask);
		 					}

		 				}
		 			}
		 	    }
                else if("service_response".Equals (op)) {
		 				Debug.Log ("Got service response " + node.ToString ());
		 				_serviceName = node["service"];
                    _serviceValues = (node["values"] == null) ? "" : node["values"].ToString ();   
                }
                else
		 		    Debug.Log ("Must write code here for other messages");
		    }
            else
		 	    Debug.Log ("Got an empty message from the web socket");
		}
#endif

#if !UNITY_EDITOR
        /* Callback message for Hololens */
        private void MessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {            
            using (DataReader reader = args.GetDataReader())
                {
                    reader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                    string read = reader.ReadString(reader.UnconsumedBufferLength);

                    if ((read!= null) && !read.Equals ("")) {
		 		        JSONNode node = JSONNode.Parse(read);
                        string op = node["op"];
                        if ("publish".Equals (op)) {
		 			        string topic = node["topic"];
		 			        foreach(Type p in _subscribers) {
		 				        if(topic.Equals (GetMessageTopic (p))) {
                                    ROSBridgeMsg msg = ParseMessage(p, node["msg"]);
		 					        RenderTask newTask = new RenderTask(p, topic, msg);
		 					        lock(_queueLock) {
		 						        bool found = false;
		 						        for(int i=0;i<_taskQ.Count;i++) {
		 							        if(_taskQ[i].getTopic().Equals (topic)) {
		 								        _taskQ.RemoveAt (i);
		 								        _taskQ.Insert (i, newTask);
		 								        found = true;
		 								        break;
		 							        }
		 						        }
		 						        if(!found)
		 						        _taskQ.Add (newTask);
		 					        }
            	 				}
		 			        }
		 	            }
                        else if("service_response".Equals (op)) {
		 				    _serviceName = node["service"];
		 				    _serviceValues = (node["values"] == null) ? "" : node["values"].ToString ();
		 		        }
		            }
                }
        }
#endif

        //runs the callback to the appropiate message
        public void Render() {
		    RenderTask newTask = null;
		 	lock (_queueLock) {
		 	    if(_taskQ.Count > 0) {
		 		    newTask = _taskQ[0];
		 			_taskQ.RemoveAt (0);
		 		}
			}
			if(newTask != null)
 				Update(newTask.getSubscriber (), newTask.getMsg ());

			if (_serviceName != null) {
				ServiceResponse (_serviceResponse, _serviceName, _serviceValues);
				_serviceName = null;
			}
        }

        /* sends the publish command to actually publish the ros message*/
#if UNITY_EDITOR
        public void Publish(String topic, ROSBridgeMsg msg)
        {
            if (_ws != null)
            {
                string s = ROSBridgeMsg.Publish(topic, msg.ToYAMLString());
                //Debug.Log ("Sending " + s);
                _ws.Send(s);
            }
        }
#endif

#if !UNITY_EDITOR
        public async void Publish(String topic, ROSBridgeMsg msg)
        {
            if (messageWebSocket != null) {
                string s = ROSBridgeMsg.Publish(topic, msg.ToYAMLString ());
                dataWriter.WriteString(s);
                await dataWriter.StoreAsync();
            }
        }
#endif

#if UNITY_EDITOR
        public void CallService(string service, string args)
        {
            if (_ws != null)
            {
                string s = ROSBridgeMsg.CallService(service, args);
                Debug.Log("Sending " + s);
                _ws.Send(s);
            }
        }
#endif

#if !UNITY_EDITOR
        public async void CallService(string service, string args) {
            if (messageWebSocket != null) {
                string s = ROSBridgeMsg.CallService(service, args);
                dataWriter.WriteString(s);
                await dataWriter.StoreAsync();
            }
        }
#endif
    } //end of class definition
} //end of namespace
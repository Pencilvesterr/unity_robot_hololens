using UnityEngine;
using ROSBridgeLib;
using System; //for the boolean

//simple test viewer as another example to run services

public class TestViewer : MonoBehaviour
{
    public Boolean sendserv;
    long[] num = { 2, 2 };
    private ROSBridgeWebSocketConnection ros = null; //defined in ROSBridgeWebSocketConnection

    // Define our subscribers, publishers and service response handlers
    private TestServices services = new TestServices();

    void Start()
    {
        //creates the connection to the bridge
        //ros = new ROSBridgeWebSocketConnection("ws://137.82.173.74", 9090); //change to IP of ROS machine        
        ros = new ROSBridgeWebSocketConnection("ws://137.82.173.72", 9090); //change to IP of ROS machine       
        //add subscribers and publishers
        ros.AddServiceResponse(typeof(TestServiceResponse));
        ros.Connect(); //actually connects to the ros bridge
        services.ServInit(ros);
    }

    // When application close, disconnect to ROS bridge
    void OnApplicationQuit()
    {
        if (ros != null)
            ros.Disconnect(); //extremely important to disconnect from ROS.OTherwise packets continue to flow
    }

    // Update is called once per frame in Unity. The Unity camera follows the robot (which is driven by
    // the ROS environment. 
    void Update()
    {
        if (sendserv)
        {
            //Point32Msg point = new Point32Msg(2f, 3f, 4f);
            //List<Point32Msg> pointarray = new List<Point32Msg>();
            //pointarray.Add(point);
            //PolygonMsg test = new PolygonMsg(pointarray);
           
            //string jointmsg = "{\"cartesian_path\" : " + test.ToYAMLString() + "}";
            int[] num = { 2, 2 };
            services.AddtwoInt(num);
            //ros.CallService("/request_joint_path", jointmsg);
            //Debug.Log(jointmsg);
            sendserv = false;
        }
        ros.Render(); //pretty much same as ros.spin()
    }
}
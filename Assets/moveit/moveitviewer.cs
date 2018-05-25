using UnityEngine;
using System.Collections.Generic;
using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;
using System; //for the boolean

/*
 * Written by Cole Shing ,2017 , modified to try to work with moveit
 * Simple viewer for talking to WAM. Currently for testing
 */

public class moveitviewer : MonoBehaviour
{
    public Boolean Send_request; //enable sending joint request

    private ROSBridgeWebSocketConnection ros = null; //defined in ROSBridgeWebSocketConnection
    // Define our subscribers, publishers and service response handlers
    RequestJointTrajSrv Jointsrv = new RequestJointTrajSrv(); //define the request joint service class

    void Start()
    {
        //creates the connection to the bridge
        //ros = new ROSBridgeWebSocketConnection("ws://192.168.0.102", 9090); //change to IP of ROS machine        
        ros = new ROSBridgeWebSocketConnection("ws://137.82.173.72", 9090); //change to IP of ROS machine       
        //add subscribers and publishers and services
        Jointsrv.ServInit(ros); //initialize the joint service
        ros.AddServiceResponse(typeof(RequestJointTrajResponose)); //add the response callback for services
        ros.Connect(); //actually connects to the ros bridge
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

#if UNITY_EDITOR
        if (Send_request)
        {
            List<Point32Msg> pointarray = new List<Point32Msg>();
            Point32Msg point1 = new Point32Msg(0.4f, -0.3f, 0.4f);
            pointarray.Add(point1);
            Point32Msg point2 = new Point32Msg(0.4f, 0.0f, 0.4f);
            pointarray.Add(point2);
            Point32Msg point3 = new Point32Msg(0.4f, +0.3f, 0.4f);
            pointarray.Add(point3);

            Jointsrv.RequestJointTraj(pointarray);
            Send_request = false;

        }
#endif
        ros.Render(); //pretty much same as ros.spin()

    }
}


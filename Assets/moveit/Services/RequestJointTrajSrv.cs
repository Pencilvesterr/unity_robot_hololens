using System.Collections.Generic;
using UnityEngine;
using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;
using ROSBridgeLib.trajectory_msgs;

/*
 * Custom service class, written by Cole Shing, 2017
 */

public class RequestJointTrajSrv : MonoBehaviour
{
    private ROSBridgeWebSocketConnection rosbridge = null; //local copy of the rosbridge
    private string service,args;

    public void ServInit(ROSBridgeWebSocketConnection ros) //initialization of the class
    {
        rosbridge = ros;
    }

    public void RequestJointTraj(List<Point32Msg> pointarray) //sending a list of points and returns robot trajectory
    {
        PolygonMsg jointrajmsg = new PolygonMsg(pointarray);
        args = "{\"cartesian_path\" : " + jointrajmsg.ToYAMLString() + "}";
        service = "/request_joint_path";
        rosbridge.CallService(service, args);
    }

    public void SendJointTraj(JointTrajectoryMsg jointtraj)
    {
        service = "/bhand/joint_trajectory";
        rosbridge.CallService(service, jointtraj.ToYAMLString());
    }
}
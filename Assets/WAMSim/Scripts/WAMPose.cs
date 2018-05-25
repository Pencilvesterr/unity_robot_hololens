using ROSBridgeLib;
using SimpleJSON;
using UnityEngine;

/* WAM pose subscriber /wam/pose position is in meter units, x positive 
 * is point away from the cable side, y is perpendicular on a flat plane
 * z positive is the vertical height of the end effector.
 * 
    geometry_msgs/PoseStamped
    std_msgs/Header header
        Unit32 seq
        Time stamp
        String frame_id
    Geometry_msgs/Pose pose
        geometry_msgs/Point position
            Float64 x
            Float64 y
            Float64 z
        geometry_msgs/Quaternion orientation
            Float64 x
            Float64 y      
            Float64 z
            Float64 w 

* Written by Cole Shing, 2017
*/

public class WAMPose : ROSBridgeSubscriber
{
    static double x, y, z;

    public new static string GetMessageTopic()
    {
        return "/wam/pose";
    }

    public new static string GetMessageType()
    {
        return "geometry_msgs/PoseStamped";
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new ROSBridgeLib.geometry_msgs.PoseStampedMsg(msg);
    }

    public new static void CallBack(ROSBridgeMsg msg)
    {
        ROSBridgeLib.geometry_msgs.PoseStampedMsg wampose = (ROSBridgeLib.geometry_msgs.PoseStampedMsg)msg;
        x = wampose.GetPostion().GetX();
        y= wampose.GetPostion().GetY();
        z = wampose.GetPostion().GetZ();
#if UNITY_EDITOR
        Debug.Log("pose position is : " + x + " " + y + " " + z);
#endif
    }
}  
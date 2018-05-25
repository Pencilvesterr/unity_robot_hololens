using ROSBridgeLib;
using SimpleJSON;
using UnityEngine;

/* WAM force torque sensor subscriber /wam/FT_sensor, forces is in N, torques in Nm 
 * 
    geometry_msgs/WrenchStamped
    std_msgs/Header header
        Unit32 seq
        Time stamp
        String frame_id
    Geometry_msgs/Wrench wrench
        geometry_msgs/Vector3 force
            Float64 x
            Float64 y
            Float64 z
        geometry_msgs/Vector torque
            Float64 x
            Float64 y      
            Float64 z

* Written by Cole Shing, 2018
*/

public class WAMFTSensor : ROSBridgeSubscriber
{
    public static double[] forces = new double[3];
    public static double[] torques = new double[3];

    public new static string GetMessageTopic()
    {
        return "/wam/FT_sensor";
    }

    public new static string GetMessageType()
    {
        return "geometry_msgs/WrenchStamped";
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new ROSBridgeLib.geometry_msgs.WrenchStampedMsg(msg);
    }

    public new static void CallBack(ROSBridgeMsg msg)
    {
        ROSBridgeLib.geometry_msgs.WrenchStampedMsg wrench_msg = (ROSBridgeLib.geometry_msgs.WrenchStampedMsg)msg;
        forces[0] = wrench_msg.GetForces().GetX();
        forces[1] = wrench_msg.GetForces().GetY();
        forces[2] = wrench_msg.GetForces().GetZ();
        torques[0] = wrench_msg.GetTorques().GetX();
        torques[1] = wrench_msg.GetTorques().GetX();
        torques[2] = wrench_msg.GetTorques().GetX();
#if UNITY_EDITOR
       // Debug.Log("forces are : " + fx + " " + fy + " " + fz + " and torques are: " + tx + " " + ty + " " + tz);
#endif
    }
}
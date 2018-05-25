using ROSBridgeLib;
using SimpleJSON;
using UnityEngine;

/* WAM force torque acceleartion subscriber /wam/FT_accel, acceleration is in m/s^2
 * 
    geometry_msgs/AccelStamped
    std_msgs/Header header
        Unit32 seq
        Time stamp
        String frame_id
    Geometry_msgs/Accel accel
        geometry_msgs/Vector3 linear
            Float64 x
            Float64 y
            Float64 z
        geometry_msgs/Vector angular
            Float64 x
            Float64 y      
            Float64 z

* Written by Cole Shing, 2018
*/

public class WAMFTAccel : ROSBridgeSubscriber
{
    public static double[] accel = new double[3];

    public new static string GetMessageTopic()
    {
        return "/wam/FT_accel";
    }

    public new static string GetMessageType()
    {
        return "geometry_msgs/AccelStamped";
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new ROSBridgeLib.geometry_msgs.AccelStampedMsg(msg);
    }

    public new static void CallBack(ROSBridgeMsg msg)
    {
        ROSBridgeLib.geometry_msgs.AccelStampedMsg accel_msg = (ROSBridgeLib.geometry_msgs.AccelStampedMsg)msg;
        accel[0] = accel_msg.GetLinearAccel().GetX();
        accel[1] = accel_msg.GetLinearAccel().GetY();
        accel[2] = accel_msg.GetLinearAccel().GetZ();
#if UNITY_EDITOR
      //  Debug.Log("acceleration is : " + x + " " + y + " " + z);
#endif
    }
}
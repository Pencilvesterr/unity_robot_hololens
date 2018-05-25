using ROSBridgeLib;
using ROSBridgeLib.wam_common;

/* WAM Orthogonal Velocity publisher using RTOrtnVelMsg to publish to 
 * /wam/ortn_vel_cmd
 * written by Cole Shing, 2017
 */
public class WAMRTOrtnVel : ROSBridgePublisher
{

    public new static string GetMessageTopic()
    {
        return "/wam/ortn_vel_cmd";
    }

    public new static string GetMessageType()
    {
        return "wam_common/RTOrtnVel";
    }

    public static string ToYAMLString(RTOrtnVelMsg msg)
    {
        return msg.ToYAMLString();
    }
}

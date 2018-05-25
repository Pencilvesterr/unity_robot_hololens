using ROSBridgeLib;
using ROSBridgeLib.wam_common;

/* WAM cartesian velocity publisher using RTCartVelMsg to publish to 
 * /wam/cart_vel_cmd
 * written by Cole Shing, 2017
 */
public class WAMRTCartVel : ROSBridgePublisher
{

    public new static string GetMessageTopic()
    {
        return "/wam/cart_vel_cmd";
    }

    public new static string GetMessageType()
    {
        return "wam_common/RTCartVel";
    }

    public static string ToYAMLString(RTCartVelMsg msg)
    {
        return msg.ToYAMLString();
    }
}

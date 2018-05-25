using ROSBridgeLib;
using ROSBridgeLib.wam_common;

/* WAM Joint velocity publisher using RTJointVelMsg to publish to 
 * /wam/jnt_vel_cmd
 * written by Cole Shing, 2017
 */
public class WAMRTJointVel : ROSBridgePublisher
{

    public new static string GetMessageTopic()
    {
        return "/wam/jnt_vel_cmd";
    }

    public new static string GetMessageType()
    {
        return "wam_common/RTJointVel";
    }

    public static string ToYAMLString(RTJointVelMsg msg)
    {
        return msg.ToYAMLString();
    }
}

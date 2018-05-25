using ROSBridgeLib;
using ROSBridgeLib.wam_common;

/* WAM joint position publisher using RTJointPosMsg to publish
 * to /wam/jnt_pos_cmd
 * written by Cole Shing, 2017
 */
public class WAMRTJointPos : ROSBridgePublisher
{

    public new static string GetMessageTopic()
    {
        return "/wam/jnt_pos_cmd";
    }

    public new static  string GetMessageType()
    {
        return "wam_common/RTJointPos";
    }

    public static string ToYAMLString(RTJointPosMsg msg)
    {
        return msg.ToYAMLString();
    }
}

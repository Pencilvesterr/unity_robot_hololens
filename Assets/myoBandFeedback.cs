using ROSBridgeLib;
using ROSBridgeLib.wam_common;
using ROSBridgeLib.geometry_msgs;

/* WAM joint position publisher using RTJointPosMsg to publish
 * to /wam/jnt_pos_cmd
 * written by Cole Shing, 2017
 */

public class myoBandFeedback : ROSBridgePublisher {

    public new static string GetMessageTopic()
    {
        return "/myo_raw/vibrate";
    }

    public new static string GetMessageType()
    {
        return "std_msgs/UInt8";
    }

    public static string ToYAMLString(PoseMsg msg)
    {
        return msg.ToYAMLString();
    }
}

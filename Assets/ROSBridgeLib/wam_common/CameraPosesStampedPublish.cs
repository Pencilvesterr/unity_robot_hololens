using ROSBridgeLib;
using ROSBridgeLib.wam_common;
using ROSBridgeLib.geometry_msgs;

/* WAM joint position publisher using RTJointPosMsg to publish
 * to /wam/jnt_pos_cmd
 * written by Cole Shing, 2017
 */
public class CameraPoseStampedPublish : ROSBridgePublisher
{

    public new static string GetMessageTopic()
    {
        return "/hololens/poseStamped";
    }

    public new static string GetMessageType()
    {
        return "geometry_msgs/PoseStamped";
    }

    public static string ToYAMLString(PoseStampedMsg msg)
    {
        return msg.ToYAMLString();
    }
}

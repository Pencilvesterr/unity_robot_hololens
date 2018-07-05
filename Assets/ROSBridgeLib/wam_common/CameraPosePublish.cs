using ROSBridgeLib;
using ROSBridgeLib.wam_common;
using ROSBridgeLib.geometry_msgs;

/* WAM joint position publisher using RTJointPosMsg to publish
 * to /wam/jnt_pos_cmd
 * written by Cole Shing, 2017
 */
public class CameraPosePublish : ROSBridgePublisher
{

    public new static string GetMessageTopic()
    {
        return "/hololens/pose";
    }

    public new static string GetMessageType()
    {
        return "geometry_msgs/Pose";
    }

    public static string ToYAMLString(PoseMsg msg)
    {
        return msg.ToYAMLString();
    }
}

using ROSBridgeLib;
using ROSBridgeLib.trajectory_msgs;

/* Joint Trajectory publisher for trajectory msgs
 * written by Cole Shing, 2017
 */
public class JointTrajectoryPub : ROSBridgePublisher
{

    public new static string GetMessageTopic()
    {
        return "/joint_trajectory";
    }

    public new static string GetMessageType()
    {
        return "trajectory_msgs/JointTrajectory";
    }

    public static string ToYAMLString(JointTrajectoryMsg msg)
    {
        return msg.ToYAMLString();
    }
}

using ROSBridgeLib;
using ROSBridgeLib.trajectory_msgs;

/* Joint Trajectory publisher for trajectory_msgs joint trayjectory point
 * written by Cole Shing, 2017
 */
public class JointTrajectoryPointPub : ROSBridgePublisher
{

    public new static string GetMessageTopic()
    {
        return "/joint_trajectory_point";
    }

    public new static string GetMessageType()
    {
        return "trajectory_msgs/JointTrajectoryPoint";
    }

    public static string ToYAMLString(JointTrajectoryPointMsg msg)
    {
        return msg.ToYAMLString();
    }
}

using ROSBridgeLib;
using ROSBridgeLib.moveit_msgs;

/* Robot Trajectory publisher  for moveit_msgs
 * written by Cole Shing, 2017
 */
public class RobotTrajectoryPub : ROSBridgePublisher
{

    public new static string GetMessageTopic()
    {
        return "/cartesian_trajectory";
    }

    public new static string GetMessageType()
    {
        return "moveit_msgs/RobotTrajectory";
    }

    public static string ToYAMLString(RobotTrajectoryMsg msg)
    {
        return msg.ToYAMLString();
    }
}

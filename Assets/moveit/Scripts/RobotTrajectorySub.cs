using ROSBridgeLib;
using SimpleJSON;
using UnityEngine;

/* Robot Trajectory subscriber for moveit_msgs
 * add to callback for what you want to do once message is recieved
 * written by Cole Shing, 2017
 */
public class RobotTrajectorySub : ROSBridgeSubscriber
{

    public new static string GetMessageTopic()
    {
        return "/path_trajectory";
    }

    public new static string GetMessageType()
    {
        return "moveit_msgs/RobotTrajectory";
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new ROSBridgeLib.moveit_msgs.RobotTrajectoryMsg(msg);
    }

    public new static void CallBack(ROSBridgeMsg msg)
    {
        //do something once new topic arrives
        ROSBridgeLib.moveit_msgs.RobotTrajectoryMsg robottrajmsg = (ROSBridgeLib.moveit_msgs.RobotTrajectoryMsg)msg;
    }
}

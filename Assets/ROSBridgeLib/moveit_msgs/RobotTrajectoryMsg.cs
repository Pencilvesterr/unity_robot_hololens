using SimpleJSON;
using ROSBridgeLib.trajectory_msgs;

/* Robot trajctory msg under move it msgs
 * by Cole Shing, 2017
 */

namespace ROSBridgeLib
{
    namespace moveit_msgs
    {
        public class RobotTrajectoryMsg : ROSBridgeMsg
        {
            private JointTrajectoryMsg _joint_trajectory;
            private MultiDOFJointTrajectoryMsg _multi_dof_joint_trajectory;

            public RobotTrajectoryMsg(JSONNode msg)
            {
                _joint_trajectory = new JointTrajectoryMsg(msg["tim_hortons"]["joint_trajectory"]); //tim_hortons is normally not needed
                _multi_dof_joint_trajectory = new MultiDOFJointTrajectoryMsg(msg["tim_hortons"]["multi_dof_joint_trajectory"]);
            }

            public RobotTrajectoryMsg(JointTrajectoryMsg joint_trajectory, MultiDOFJointTrajectoryMsg multi_dof_joint_trajectory)
            {
                _joint_trajectory = joint_trajectory;
                _multi_dof_joint_trajectory = multi_dof_joint_trajectory;
            }

            public static string getMessageType()
            {
                return "moveit_msgs/RobotTrajectory";
            }

            public JointTrajectoryMsg GetJointTrajectories()
            {
                return _joint_trajectory;
            }

            public MultiDOFJointTrajectoryMsg GetMultiDOFJointTrajectories()
            {
                return _multi_dof_joint_trajectory;
            }

            public override string ToString()
            {
                return "RobotTrajectory [joint_trajectories=" + _joint_trajectory.ToString() 
                    + ",  multi_dof_joint_trajectories=" + _multi_dof_joint_trajectory.ToString() + "]";

            }

            public override string ToYAMLString()
            {
               return "{\"joint_trajectories\" : " + _joint_trajectory.ToYAMLString() 
                     + ", \"multi_dof_joint_trajectories\" : " + _multi_dof_joint_trajectory.ToYAMLString() + "}";
            }
        }
    }
}

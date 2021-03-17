/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

using Newtonsoft.Json;

namespace RosSharp.RosBridgeClient.MessageTypes.Moveit
{
    public class MotionPlanRequest : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "moveit_msgs/MotionPlanRequest";

        //  This service contains the definition for a request to the motion
        //  planner and the output it provides
        //  Parameters for the workspace that the planner should work inside
        public WorkspaceParameters workspace_parameters;
        //  Starting state updates. If certain joints should be considered
        //  at positions other than the current ones, these positions should
        //  be set here
        public RobotState start_state;
        //  The possible goal states for the model to plan for. Each element of
        //  the array defines a goal region. The goal is achieved
        //  if the constraints for a particular region are satisfied
        public Constraints[] goal_constraints;
        //  No state at any point along the path in the produced motion plan will violate these constraints (this applies to all points, not just waypoints)
        public Constraints path_constraints;
        //  The constraints the resulting trajectory must satisfy
        public TrajectoryConstraints trajectory_constraints;
        //  A set of trajectories that may be used as reference or initial trajectories for (typically optimization-based) planners
        //  These trajectories do not override start_state or goal_constraints
        public GenericTrajectory[] reference_trajectories;
        //  The name of the motion planner to use. If no name is specified,
        //  a default motion planner will be used
        public string planner_id;
        //  The name of the group of joints on which this planner is operating
        public string group_name;
        //  The number of times this plan is to be computed. Shortest solution
        //  will be reported.
        public int num_planning_attempts;
        //  The maximum amount of time the motion planner is allowed to plan for (in seconds)
        public double allowed_planning_time;
        //  Scaling factors for optionally reducing the maximum joint velocities and
        //  accelerations.  Allowed values are in (0,1].  The maximum joint velocity and
        //  acceleration specified in the robot model are multiplied by thier respective
        //  factors.  If either are outside their valid ranges (importantly, this
        //  includes being set to 0.0), the factor is set to the default value of 1.0
        //  internally (i.e., maximum joint velocity or maximum joint acceleration).
        public double max_velocity_scaling_factor;
        public double max_acceleration_scaling_factor;
        //  Maximum cartesian speed for the given end effector.
        //  If max_cartesian_speed <= 0 the trajectory is not modified.
        //  These fields require the following planning request adapter: default_planner_request_adapters/SetMaxCartesianEndEffectorSpeed
        public string cartesian_speed_end_effector_link;
        public double max_cartesian_speed;
        //  m/s

        public MotionPlanRequest()
        {
            this.workspace_parameters = new WorkspaceParameters();
            this.start_state = new RobotState();
            this.goal_constraints = new Constraints[0];
            this.path_constraints = new Constraints();
            this.trajectory_constraints = new TrajectoryConstraints();
            this.reference_trajectories = new GenericTrajectory[0];
            this.planner_id = "";
            this.group_name = "";
            this.num_planning_attempts = 0;
            this.allowed_planning_time = 0.0;
            this.max_velocity_scaling_factor = 0.0;
            this.max_acceleration_scaling_factor = 0.0;
            this.cartesian_speed_end_effector_link = "";
            this.max_cartesian_speed = 0.0;
        }

        public MotionPlanRequest(WorkspaceParameters workspace_parameters, RobotState start_state, Constraints[] goal_constraints, Constraints path_constraints, TrajectoryConstraints trajectory_constraints, GenericTrajectory[] reference_trajectories, string planner_id, string group_name, int num_planning_attempts, double allowed_planning_time, double max_velocity_scaling_factor, double max_acceleration_scaling_factor, string cartesian_speed_end_effector_link, double max_cartesian_speed)
        {
            this.workspace_parameters = workspace_parameters;
            this.start_state = start_state;
            this.goal_constraints = goal_constraints;
            this.path_constraints = path_constraints;
            this.trajectory_constraints = trajectory_constraints;
            this.reference_trajectories = reference_trajectories;
            this.planner_id = planner_id;
            this.group_name = group_name;
            this.num_planning_attempts = num_planning_attempts;
            this.allowed_planning_time = allowed_planning_time;
            this.max_velocity_scaling_factor = max_velocity_scaling_factor;
            this.max_acceleration_scaling_factor = max_acceleration_scaling_factor;
            this.cartesian_speed_end_effector_link = cartesian_speed_end_effector_link;
            this.max_cartesian_speed = max_cartesian_speed;
        }
    }
}

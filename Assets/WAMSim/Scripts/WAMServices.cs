using System.Collections.Generic;
using UnityEngine;
using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;

/// <summary>
// services class for the Barrett WAM, written by Cole Shing, 2017
/// </summary>

public class WAMServices : MonoBehaviour
{
    private ROSBridgeWebSocketConnection rosbridge = null; //local copy of the rosbridge
    private string service, args;

    public void ServInit(ROSBridgeWebSocketConnection ros) //initialization of the class
    {
        rosbridge = ros;
    }

    public void CartMove(float[] Position) //move the wam through cartesian [3]
    {
        args = "{\"position\" : [" + Position[0] + ", " + Position[1] +
            ", " + Position[2] + "] }";
        service = "/wam/cart_move";
        rosbridge.CallService(service, args);
    }

    public void CartVel(float[] v_direction, float v_magnitude, float kp, bool visual_system) //set velocity of the wam in cartesian
    {
        args = "{\"v_direction\" : [" + v_direction[0] + ", " + v_direction[1] +
            ", " + v_direction[2] + "], \"v_magnitude\" : " + v_magnitude + ", \"kp\" : "
            + kp + ", \"visual_system\" : " + visual_system + "}";
        service = "/bhand/finger_pos";
        rosbridge.CallService(service, args);
    }

    public void ForceTorqueBase(float[] force, float[] torque) //set forque and torque at the base
    {
        string forcearray = floattoarray(force);
        string torquearray = floattoarray(torque);

        args = "{\"force\" : " + forcearray + ", \"torque\" : " + torquearray + "}";
        service = "/wam/force_torque_base";
        rosbridge.CallService(service, args);
    }

    public void ForceTorqueTool(float[] force, float[] torque) //set forque and torque at the tool
    {
        string forcearray = floattoarray(force);
        string torquearray = floattoarray(torque);

        args = "{\"force\" : " + forcearray + ", \"torque\" : " + torquearray + "}";
        service = "/wam/force_torque_tool";
        rosbridge.CallService(service, args);
    }

    public void GoHome() //send the wam home
    {
        service = "/wam/go_home";
        args = "";
        rosbridge.CallService(service, args);
    }

    public void GravityComp(bool gravity) //set graviy comp on or off
    {
        args = "{\"gravity\" : " + gravity + "}";
        service = "/wam/gravity_comp";
        rosbridge.CallService(service, args);
    }

    public void HoldCartPos(bool hold) //set to hold cartesian position
    {
        args = "{\"hold\" : " + hold + "}";
        service = "/wam/hold_cart_pos";
        rosbridge.CallService(service, args);
    }

    public void HoldJointPos(bool hold) //set to hold joint position
    {
        args = "{\"hold\" : " + hold + "}";
        service = "/wam/hold_joint_pos";
        rosbridge.CallService(service, args);
    }

    public void HoldOrtn(bool hold) //set to hold Ortn position
    {
        args = "{\"hold\" : " + hold + "}";
        service = "/wam/hold_ortn";
        rosbridge.CallService(service, args);
    }

    public void HoldOrtn2(bool hold) //set to hold Ortn position 2 , 
    {
        args = "{\"hold\" : " + hold + "}";
        service = "/wam/hold_ortn2";
        rosbridge.CallService(service, args);
    }

    public void JointMove(float[] joints) //move the wam using joints
    {
        string jointarray = floattoarray(joints);
      
        args = "{\"joints\" : " + jointarray + "}";
        service = "/wam/joint_move";
        rosbridge.CallService(service, args);
    }

    public void OrtnMove(float[] orientation) //move the wam using ortn [4]
    {
        args = "{\"orientation\" : [" + orientation[0] + ", " + orientation[1] +
        ", " + orientation[2] + ", " + orientation[3] + "] }";
        service = "/wam/ortn_move";
        rosbridge.CallService(service, args);
    }

    public void OrtnSplitMove(float[] orientation, float[] kp_gain, float[] kd_gain) //move the wam using ortn with split [4], [3], [3]
    {
        args = "{\"orientation\" : [" + orientation[0] + ", " + orientation[1] +
        ", " + orientation[2] + ", " + orientation[3] + "], \"kp_gain\" : [" + kp_gain[0] + ", " + kp_gain[1] 
        + ", " +kp_gain[2] +"], \"kd_gain\" : [" + kd_gain[0] + ", " + kd_gain[1] + ", " + kd_gain[2] + "] }";
        service = "/wam/ortn_split_move";
        rosbridge.CallService(service, args);
    }

    public void PlayMotion(string path) //play the path from file location
    {
        args = "{\"path\" : " + path + "}";
        service = "/wam/play_motion";
        rosbridge.CallService(service, args);
    }

    public void PoseMove(Point32Msg position, QuaternionMsg orientation) //move to position and orientation
    {
        args = "{\"position\" : " + position.ToYAMLString() + ", \"orientation\" : " + orientation.ToYAMLString() + "}";
        service = "/wam/pose_move";
        rosbridge.CallService(service, args);
    }

    public void TeachMotion(string path) //teach and save the motion to specified path
    {
        args = "{\"path\" : " + path + "}";
        service = "/wam/teach_motion";
        rosbridge.CallService(service, args);
    }

    public void FollowPath(List<Point32Msg> position, List<Point32Msg> normal, int size) //set a follow path 
    {
        ConvertingArraytoString convert = new ConvertingArraytoString();
        //PolygonMsg posarray = new PolygonMsg(position);
        //PolygonMsg normalarray = new PolygonMsg(normal);


        //return "{\"points\" : " + pointarray + "}";

        args = "{\"position\" : " +convert.Listtoarray(position)+", \"normal\" : "+ convert.Listtoarray(normal)+", \"size\" : " + size + "}";
       // args = "{position: " + convert.Listtoarray(position) + ",normal: " + convert.Listtoarray(normal) + ",size: " + size + "}"; //debug mode

        service = "/bhand/follow_path"; //wam-->bhand TODO: correct namespace inside wam_node 
        rosbridge.CallService(service, args);
    }

    public void LinkArm(string remote_ip) //link the hand to the arm through ip
    {
        args = "{\"velocity\" : '" + remote_ip + "'}";
        service = "/wam/link_arm";
        rosbridge.CallService(service, args);
    }

    public void StartVisualFix() //start visual fix
    {
        service = "/wam/start_visual_fix";
        args = "";
        rosbridge.CallService(service, args);
    }

    public void StopVisualFix() //stop visual fix
    {
        service = "/bhand/stop_visual_fix";
        args = "";
        rosbridge.CallService(service, args);
    }

    public void UnlinkArm() //unlink the arm
    {
        service = "/wam/unlink_arm";
        args = "";
        rosbridge.CallService(service, args);
    }


    public void MoveToPose(PoseMsg pose)
    {
        args = "{\"pose\" : " + pose.ToYAMLString() + "}";
        service = "/move_to_pose";
        rosbridge.CallService(service, args);
    }




    private string floattoarray(float[] input) //create the array
    {
        string floatarray = "[";
        for (int i = 0; i < input.Length; i++)
        {
            floatarray = floatarray + input[i];
            if (input.Length - i >= 1 && i < input.Length - 1)
                floatarray += ",";
        }
        floatarray += "]";
        return floatarray;
    }

}

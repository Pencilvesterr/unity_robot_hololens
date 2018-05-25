using ROSBridgeLib;
using SimpleJSON;
using UnityEngine;
using System;

/* 
 * WAM Joint state subscriber, subscribes to wam/joint_states through the JointStateMsg
 * based on the standard subscriber
 * written by Cole Shing, 2017
 */
public class WAMJointState : ROSBridgeSubscriber {

    private static float[] position = new float[7];
    private static float[] angle = new float[7]; //the position in degrees of the WAM
    private static float[] home_angles = { 0.0F, -2.0F, 0.0F, 3.1F, 0.0F, 0F, 0.0F }; //the angles in radians
    private static double[] rotation = new double[7]; //the positions in rads of the WAM in double

    public new static string GetMessageTopic()
    {
        return "/wam/joint_states";
    }

    public new static string GetMessageType()
    {
        return "sensor_msgs/JointState";
    }

    public static bool isHome() //is true when the angles is within 1 degrees
    {                         
        bool home = false;
        for (int i = 0; i < 7; i++)
        { 
            if (!(Math.Abs(position[i] - home_angles[i]) < 0.05))
            {
                    home = false;
                    break;    
            }
            else
                home = true; 
        }
        return home;
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new ROSBridgeLib.sensor_msgs.JointStateMsg(msg);
    }

    public new static void CallBack(ROSBridgeMsg msg)
    {   
        GameObject robot = GameObject.Find("WAM"); //grab the WAM
        if (robot == null)
        {
#if UNITY_EDITOR
            Debug.Log("Can't find the robot???");
#endif
        }
        else
        {
            GameObject[] Rotations = new GameObject[7]; //finding all 7 rotations
            Rotations[0] = GameObject.Find("B2124"); //J1 horizontal base rotation
            Rotations[1] = GameObject.Find("B2125"); //J2 vertical base rotation
            Rotations[2] = GameObject.Find("B2126"); //J3 upper arm rotation around axis
            Rotations[3] = GameObject.Find("B2127"); //J4 eblow joint
            Rotations[4] = GameObject.Find("B3308"); //J5 rotation of the wrist
            Rotations[5] = GameObject.Find("B2573"); //J6 Flex of the wrist
            Rotations[6] = GameObject.Find("B2574"); //J7 rotation of the tool plate

            ROSBridgeLib.sensor_msgs.JointStateMsg jointstate = (ROSBridgeLib.sensor_msgs.JointStateMsg)msg;
            rotation = jointstate.GetPosition(); //get the positions from the message
            for (int i = 0; i < rotation.Length; i++)
            {
                position[i] = System.Convert.ToSingle(rotation[i]);
                angle[i] = Mathf.Rad2Deg * position[i]; //convert to degrees
            }
            //setting rotations to the correct angle
            Rotations[0].transform.localRotation = Quaternion.Euler(0, -angle[0] - 90, 0); //J1 rotates around the base, negative due to physical rotation
            Rotations[1].transform.localRotation = Quaternion.Euler(angle[1], 90, 0); //J2 rotates vertically
            Rotations[2].transform.localRotation = Quaternion.Euler(0, -angle[2] - 180, 0); //J3 rotates around the axis of the arm
            Rotations[3].transform.localRotation = Quaternion.Euler(angle[3] - 90, 180, 0); //J4 bends the elbow
            Rotations[4].transform.localRotation = Quaternion.Euler(0, -angle[4], 0); //J5 rotates around the wrist, negative due to physical rotation
            Rotations[5].transform.localRotation = Quaternion.Euler(angle[5], 0, 0); //J6 bends the wrist
            Rotations[6].transform.localRotation = Quaternion.Euler(0, angle[6], 0); //J6 rotates the wrist
        }
#if UNITY_EDITOR
    //    Debug.Log("Render callback in /wam/joint_states" + msg);
#endif
    }
}

using ROSBridgeLib;
using SimpleJSON;
using UnityEngine;

/* 
 * Barrett Hand Joint state subscriber, subscribes to bhand/joint_states through the JointStateMsg
 * based on the standard subscriber, will update the scene if there is the barrett hand to visualize current configuration of the physical hand
 * written by Cole Shing, 2017
 */
public class BHandJointState : ROSBridgeSubscriber
{
    private static float[] position = new float[7];
    private static float[] angle = new float[7]; //the position in degrees of the barrett hand
    private static float[] home_angles = { 1.888F, 1.773F, 1.79F, 0.005F, 1.30F, 1.42F, 1.397F, 0.005F };  //the home angles
    private static double[] rotation = new double[7]; //the positions in rads of the barrett hand

    public new static string GetMessageTopic()
    {
        return "/bhand/joint_states";
    }

    public new static string GetMessageType()
    {
        return "sensor_msgs/JointState";
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new ROSBridgeLib.sensor_msgs.JointStateMsg(msg);
    }

    public new static void CallBack(ROSBridgeMsg msg)
    {
        GameObject bhand = GameObject.Find("BHand"); //grab the Barrett Hand
        if (bhand == null)
        {
#if UNITY_EDITOR
            Debug.Log("Can't find the hand???");
#endif
        }
        else
        {
            GameObject[] Rotations = new GameObject[8]; //finding all 7 rotations
            Rotations[0] = GameObject.Find("B4159_2"); //inner_f1
            Rotations[1] = GameObject.Find("B4159_1"); //inner_f2
            Rotations[2] = GameObject.Find("B4159_3"); //inner_f3
            Rotations[3] = GameObject.Find("B4158_1"); //spread, used for one side
            Rotations[4] = GameObject.Find("B4160_3"); //outer_f1
            Rotations[5] = GameObject.Find("B4160_1"); //outer_f2
            Rotations[6] = GameObject.Find("B4160_2"); //outer_f3
            Rotations[7] = GameObject.Find("B4158_2"); //spread for the other side

            ROSBridgeLib.sensor_msgs.JointStateMsg jointstate = (ROSBridgeLib.sensor_msgs.JointStateMsg)msg;
            rotation = jointstate.GetPosition(); //get the positions from the message
            for (int i = 0; i < rotation.Length; i++)
            {
                position[i] = System.Convert.ToSingle(rotation[i]);
                angle[i] = Mathf.Rad2Deg * position[i]; //convert to degrees
            }
            //setting rotations to the correct angle, offset due to physical vs unity's origin
            Rotations[0].transform.localRotation = Quaternion.Euler(-angle[0] + 130, 0, 0); //inner_f1
            Rotations[1].transform.localRotation = Quaternion.Euler(-angle[1] + 130, 0, 0); //inner_f2
            Rotations[2].transform.localRotation = Quaternion.Euler(-angle[2] + 130, 0, 0); //inner_f3
            Rotations[3].transform.localRotation = Quaternion.Euler(0, -angle[3]+180, 0); //spread, used for one side
            Rotations[4].transform.localRotation = Quaternion.Euler(-angle[4] + 55, 0, 0); //outer_f1
            Rotations[5].transform.localRotation = Quaternion.Euler(-angle[5] + 55, 0, 0); //outer_f2
            Rotations[6].transform.localRotation = Quaternion.Euler(-angle[6] + 55, 0, 0); //outer_f3
            Rotations[7].transform.localRotation = Quaternion.Euler(0, angle[3]+180, 0); //spread for the other side
        }
#if UNITY_EDITOR
 //       Debug.Log("Render callback in /bhand/joint_states" + msg);
#endif
    }
}

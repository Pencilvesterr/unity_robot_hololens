using UnityEngine;
using System; //for the boolean

/*
 * Written by Cole Shing ,2017
 * -currently does not work, just left for archiving purposes - USE Wamviewer instead.
 * 
 * 
 * 
 * Simple viewer for talking to virtual Barrett Hand, if both hand and wam or using ros bridge,
 * use the vamviewer.cs instead of this. To get this working, attach it to any object in the scene and run
 */

public class BHandViewer : MonoBehaviour
{
#if UNITY_EDITOR
    float[] sliders = { -0.838F, -2.44F, -2.44F, -2.44F, -0.838F, -0.838F, 0.0F, 0.0F }; //starting slider positions
#endif
    public Boolean Use_Sliders; //enable to allow control of the WAM using the sliders
    public Boolean Close_hand; //starts the open grasp service
    public Boolean Open_hand; //starts the close grasp service
    Boolean Wamconnected; //boolean if the wam is connected

    GameObject handbase; //the transform for the wrist
    GameObject[] Rotations = new GameObject[8]; //finding all 8 rotations;

    private static float[] angle = new float[8]; //the angles in degrees for each joint

    // Define our subscribers, publishers and service response handlers
    void Start()
    {
#if !UNITY_EDITOR
            Use_Sliders = false; //do not use if not in unity
#endif
        if (GameObject.Find("WAM")) //see if WAM is in the scene
        {
            Wamconnected = true; //if true flag that it is connected
            handbase = GameObject.Find("B2574");
            this.gameObject.transform.SetParent(handbase.transform); //set the hand transform to the handbase
        }

        Rotations[0] = GameObject.Find("B4160_3"); //bh_j23_joint, left finger distal joint
        Rotations[1] = GameObject.Find("B4159_3"); //bh_j12_joint, right finger proximal joint
        Rotations[2] = GameObject.Find("B4159_2"); //bh_j22_joint, left finger proximal joint
        Rotations[3] = GameObject.Find("B4159_1"); //bh_j32_joint, middle finger proximal joint
        Rotations[4] = GameObject.Find("B4160_1"); //bh_j33_joint, middle finger distal joint
        Rotations[5] = GameObject.Find("B4160_2"); //bh_j13_joint, right finger distal joint
        Rotations[6] = GameObject.Find("B4158_1"); //bh_j11_joint, right finger spread
        Rotations[7] = GameObject.Find("B4158_2"); //bh_j21_joint, left finger spread
    }

    // When application close, disconnect to ROS bridge
    void OnApplicationQuit()
    {
    }

    // Update is called once per frame in Unity. The Unity camera follows the robot (which is driven by
    // the ROS environment. 
    void Update()
    {
#if UNITY_EDITOR
        if (Use_Sliders)
        {
            float[] joints = new float[8];
            float[] rate_limits = new float[8];

            for (int i = 0; i < 8; i++)
            {
                joints[i] = sliders[i]; //set joints to the sliders position
                rate_limits[i] = System.Convert.ToSingle(0.1); //set rate to be 0.1 always
                angle[i] = Mathf.Rad2Deg * joints[i];
            }

            Rotations[0].transform.localRotation = Quaternion.Euler(angle[0]+48, 0 , 0); //left finger distal joint
            Rotations[1].transform.localRotation = Quaternion.Euler(angle[1]+140, 0, 0); //right finger proximal joint
            Rotations[2].transform.localRotation = Quaternion.Euler(angle[2]+140, 0, 0); //left finger proximal joint
            Rotations[3].transform.localRotation = Quaternion.Euler(angle[3]+140, 0, 0); //middle finger proximal joint
            Rotations[4].transform.localRotation = Quaternion.Euler(angle[4]+48, 0, 0); //middle finger distal joint
            Rotations[5].transform.localRotation = Quaternion.Euler(angle[5]+48, 0, 0); //right finger distal joint
            Rotations[6].transform.localRotation = Quaternion.Euler(0, angle[6], 0); //right finger spread
            Rotations[7].transform.localRotation = Quaternion.Euler(0, angle[7], 0); //left finger spread
        }
#endif
    }

#if UNITY_EDITOR
    void OnGUI() //creates the sliders onto the gui in editor
    {
        sliders[0] = GUI.HorizontalSlider(new Rect(300, 25, 100, 30), sliders[0], -0.838F, 0.0F);
        sliders[1] = GUI.HorizontalSlider(new Rect(300, 55, 100, 30), sliders[1], -2.44F, 0F);
        sliders[2] = GUI.HorizontalSlider(new Rect(300, 85, 100, 30), sliders[2], -2.44F, 0F);
        sliders[3] = GUI.HorizontalSlider(new Rect(300, 115, 100, 30), sliders[3], -2.44F, 0F);
        sliders[4] = GUI.HorizontalSlider(new Rect(300, 145, 100, 30), sliders[4], -0.838F, 0F);
        sliders[5] = GUI.HorizontalSlider(new Rect(300, 175, 100, 30), sliders[5], -0.838F, 0F);
        sliders[6] = GUI.HorizontalSlider(new Rect(300, 205, 100, 30), sliders[6], 0F, -3.0F);
        sliders[7] = GUI.HorizontalSlider(new Rect(300, 235, 100, 30), sliders[7], 0F, 3.0F);
    }
#endif
}
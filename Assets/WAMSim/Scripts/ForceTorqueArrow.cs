using UnityEngine;
using System;
using UnityEngine.UI;
public class ForceTorqueArrow : MonoBehaviour
{
    public GameObject tip_sphere; // the tip_sphere prefab
    public GameObject arrow; //the arrow prefab
    public GameObject donut; //the torque prefab
    public Transform wrist; //the wam wrist
    public bool render_arrow=true;
    public Text torque_debug_text;
    GameObject[] forces = new GameObject[3]; //the 3 force arrows
    GameObject[] torques = new GameObject[3]; //the 3 torques
    GameObject[] accel = new GameObject[3]; //the 3 force arrows
    Color[] colours = { Color.red, Color.green, Color.blue }; //the 3 colors for forces and torques

    public bool tare = false; //tare the force/torque readings
    static double[] force_biased= new double[3];
    static double[] torque_biased = new double[3];
    public bool show_accel = true; //show the acceleration, currently not implemented
    
    static int vert = 64; //number of vertices for the uv mapping
    static Color[] temp_color = new Color[vert * vert];
    public static int adjustvalue = 1;//use to adjust the torque ratio, i.e. how much amount per 1% of torque as the ring goes from 0-100
    public Vector3 tip_position;
    void Start()
    {
        
        Vector3[] force_pos = new Vector3[3];
        Vector3[] torque_pos = new Vector3[3];
        Quaternion[] rotation = new Quaternion[3];
        tip_position = new Vector3(0.0f,0.0f,0.0f);
        Vector3 wrist_pos = wrist.position; //shortening variable
        Quaternion wrist_rot = wrist.rotation; //shortening variable

        
        /*
        force_pos[0] = new Vector3(wrist_pos.x, wrist_pos.y, wrist_pos.z); //X position
        force_pos[1] = new Vector3(wrist_pos.x, wrist_pos.y, wrist_pos.z); //Y position
        force_pos[2] = new Vector3(wrist_pos.x, wrist_pos.y, wrist_pos.z); //Z position
        rotation[0] = wrist_rot; 
        rotation[1] = Quaternion.Euler(180, 0, 0);
        rotation[2] = Quaternion.Euler(0, -90, 0);
        */
        /*        
        force_pos[0] = new Vector3(wrist_pos.x - 0.15F, wrist_pos.y, wrist_pos.z); //X position
        force_pos[1] = new Vector3(wrist_pos.x, wrist_pos.y, wrist_pos.z - 0.15F); //Y position
        force_pos[2] = new Vector3(wrist_pos.x, wrist_pos.y + 0.15F, wrist_pos.z); //Z position
        rotation[0] = Quaternion.Euler(0, -90, 0);
        rotation[1] = Quaternion.Euler(180, 0, 0);
        rotation[2] = wrist_rot;
        */


        /*
        torque_pos[0] = new Vector3(wrist_pos.x - 0.07F, wrist_pos.y, wrist_pos.z); //X position
        torque_pos[1] = new Vector3(wrist_pos.x, wrist_pos.y, wrist_pos.z - 0.07F); //Y position
        torque_pos[2] = new Vector3(wrist_pos.x, wrist_pos.y + 0.07F, wrist_pos.z); //Z position
        */


        //create each force arrow with their respective color
        for (var i = 0; i < 3; i++) {
            forces[i] = (GameObject)Instantiate(arrow, force_pos[i], rotation[i]);
            forces[i].transform.localScale = new Vector3(0.05F, 0.05F, 0.05F);
            Renderer[] arrow_colors = forces[i].GetComponentsInChildren<Renderer>();
            arrow_colors[1].material.color = colours[i];
            arrow_colors[2].material.color = colours[i];
            force_biased[0] = 0.0; //initialize to 0
            torque_biased[0] = 0.0; //initialize to 0
          //  torques[i] = (GameObject)Instantiate(donut, torque_pos[i], rotation[i]);
          //  torques[i].transform.localScale = new Vector3(0.05F, 0.05F, 0.05F);
        }
        tip_sphere = (GameObject)Instantiate(tip_sphere, force_pos[2], rotation[2]);


        /* if (show_accel) {//not implemented yet
             for (var i = 0; i < 3; i++) {
                 accel[i] = (GameObject)Instantiate(arrow, force_pos[i], rotation[i]);
                 accel[i].transform.localScale = new Vector3(0.05F, 0.05F, 0.05F);
                 Renderer[] arrow_colors = accel[i].GetComponentsInChildren<Renderer>();
                 arrow_colors[1].material.color = colours[i];
                 arrow_colors[2].material.color = colours[i];
             }
         } */
    }

    void Update()
    {
        if (render_arrow) //toggle to visualize arrows
        {

            //move and rotate to the appropiate place around the wrist for forces
            for (var i = 0; i < 3; i++)
            {
                //adjust the size base on the force reading
                if (Math.Abs(WAMFTSensor.forces[i] - force_biased[i]) > 0.01)
                {
                    forces[i].transform.localScale = new Vector3(0.05F, 0.05F, (float)(0.05 * (WAMFTSensor.forces[i] - force_biased[i]) / 10.0)); //divided by 10 due to scaling of unity
                    //torque_debug_text.text = WAMFTSensor.forces[2].ToString();
                }
                else
                    forces[i].transform.localScale = new Vector3(0F, 0F, 0F); //make it invisble by making it 0 size
                forces[i].transform.position = wrist.position;
                forces[i].transform.position += wrist.rotation * new Vector3(0, 0.06F, 0); //add the the space vector based on the rotation of the wrist
                                                                                           //            forces[i].transform.position += wrist.rotation * new Vector3(0, 0, 0.15F); //add the the space vector based on the rotation of the wrist


                forces[i].transform.rotation = wrist.rotation;
            }
            forces[1].transform.rotation *= Quaternion.Euler(0, -90, 0); //rotate by 90 on the Y axis to get it pointing to the X  (neg due to left hand notation)
                                                                         //forces[1].transform.rotation *= Quaternion.Euler(-90, 0, 0); //rotate by 90 on the X axis to get it pointing to the Y  (neg due to left hand notation)
            forces[2].transform.rotation *= Quaternion.Euler(-90, 0, 0); //rotate by 90 on the X axis to get it pointing to the Y  (neg due to left hand notation)



            //move and rotate to the appropiate place around the wrist for torques

            /*
            for (var i = 0; i < 3; i++) {

                int temp_value = Mathf.RoundToInt(Mathf.Abs((float)WAMFTSensor.torques[i]) *adjustvalue * vert *0.01F);
                if ((float)WAMFTSensor.torques[i] > 0) { //if the torque value is positive
                    for (var j = 0; j < vert * temp_value; j++)
                        temp_color[j] = colours[i];
                    for (var j = 0; j < vert; j++)
                        temp_color[j] = Color.black; //set to show where the start is
                    for (var j = 0; j < vert * (vert - temp_value); j++)
                        temp_color[j + temp_value * vert] = Color.clear; //set the rest of the circle to be transparent
                }
                else {
                    for (var j = 0; j < vert * temp_value; j++) //if torque is negative
                        temp_color[vert * vert - j-1] = colours[i];
                    for (var j = 0; j < vert; j++)
                        temp_color[vert * vert - j-1] = Color.black; //set to show where the start is
                    for (var j = 0; j < vert * (vert - temp_value); j++)
                        temp_color[j] = Color.clear;//set the rest of the circle to be transparent
                }

                Texture2D destTex = new Texture2D(vert, vert);
                destTex.SetPixels(temp_color);//update the color pixels of the torque
                destTex.Apply();
                torques[i].GetComponent<Renderer>().material.mainTexture = destTex; //update the textures

                torques[i].transform.position = wrist.position;
                torques[i].transform.position += wrist.rotation * new Vector3(0, 0, 0.15F); //add the the space vector based on the rotation of the wrist
                torques[i].transform.rotation = wrist.rotation;
            }
            torques[0].transform.position += new Vector3(-0.1F, 0, 0); //separate the torque rings
            torques[1].transform.position += new Vector3(0, 0, -0.1F);
            torques[2].transform.position += new Vector3(0, 0.1F,0);
            torques[0].transform.rotation *= Quaternion.Euler(0, -90, 0); //rotate by 90 on the Y axis to get it pointing to the X  (neg due to left hand notation)
            torques[1].transform.rotation *= Quaternion.Euler(-90, 0, 0); //rotate by 90 on the X axis to get it pointing to the Y  (neg due to left hand notation)
            */
            //for taring the force/torque values
            if (tare)
            {
                for (var i = 0; i < 3; i++)
                {
                    force_biased[i] = WAMFTSensor.forces[i];
                    //      torque_biased[i] = WAMFTSensor.torques[i];
                }
                tare = false;
            }


            torque_debug_text.text = "Force read from sensor:" + Environment.NewLine;
            torque_debug_text.text = WAMFTSensor.forces[2].ToString();
            tip_position[2] = (float)(WAMFTSensor.forces[2]/50.0);
            tip_sphere.transform.position = forces[2].transform.position + forces[2].transform.TransformDirection(tip_position);

        }
        else
        {
            arrow.SetActive(false);
        }
    }
}

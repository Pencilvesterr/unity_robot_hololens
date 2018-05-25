using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
using ROSBridgeLib.geometry_msgs;
using ROSBridgeLib;
using ROSBridgeLib.wam_common;

public class SpeechManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    public Transform path_sphere;
    public Transform normal_arrow;
    public Transform profile_sphere;
    private LineRenderer lineConnecting;
    public List<Vector3> path_points;
    public List<Vector3> path_points_global;
    public List<Vector3> path_normals;
    public Text set_point_list_text;
    public Transform WAM_t;//CP 
    public Vector3 point_wrt_WAM;
    private const float start_force =0.5f; //size in m for starting force--> force=15N -->0.3m////force 25N-->0.5m
    private const float end_force = 0.2f;   //size in m for end force --> force=5N -->0.1m
    private const float start_force_line = 0.3f;
    private const float start_force_sin = 0.3f; //force=15N-->0.3m, force=10-->0.2m
    private const float sin_amplitude = 0.2f;//force 10N-->0.2m

    // public GameObject path_clone;
    //public GameObject path_sphere3;
    //private Transform normal_clone;
    //public Rigidbody path_sphere2;
    private int path_point_counter;
    // Use this for initialization
    void Start()
    {
        path_points = new List<Vector3>();
        path_normals = new List<Vector3>();
        path_points_global = new List<Vector3>();
        path_point_counter = 0;
        lineConnecting = gameObject.AddComponent<LineRenderer>();
        lineConnecting.widthMultiplier = 0.01f;
        //lineConnecting.material=new Material(Shader.Find("Particle/Additive"));
        //lineConnecting.material.SetColor("test",new Color(1,0,0,0.5f));

        lineConnecting.enabled = false;

        //lineConnecting.SetPositions();


        keywords.Add("Go Home", () =>
        {
            float[] home = { 0.0F, 0.0F, 0.0F, 1.57F, 0.0F, 0F, 0.0F };

            GameObject.Find("WAM").GetComponent<WAMViewer>().go_home(home);
        });


        keywords.Add("Free Path", () =>
        {
            GameObject.Find("WAM").GetComponent<WAMViewer>().free_path();
        });

        keywords.Add("Free Robot", () =>
        {
            GameObject.Find("WAM").GetComponent<WAMViewer>().gravity();
        });

        keywords.Add("Display Menu", () =>
        {


            set_point_list_text.text = "Go Home" + System.Environment.NewLine;
            set_point_list_text.text += "Free Robot" + System.Environment.NewLine;
            set_point_list_text.text += "Set Point" + System.Environment.NewLine;
            set_point_list_text.text += "Lock Path" + System.Environment.NewLine;
            set_point_list_text.text += "Free Path" + System.Environment.NewLine;
            set_point_list_text.text += "Reset Path" + System.Environment.NewLine;
            set_point_list_text.text += "Open Hand" + System.Environment.NewLine;
            set_point_list_text.text += "Close Hand" + System.Environment.NewLine;            
            set_point_list_text.text += "Next Point" + System.Environment.NewLine;
            set_point_list_text.text += "Move Up" + System.Environment.NewLine;
            set_point_list_text.text += "Move Down" + System.Environment.NewLine;
            set_point_list_text.text += "Move Left" + System.Environment.NewLine;
            set_point_list_text.text += "Move Right" + System.Environment.NewLine;
            set_point_list_text.text += "Move Back" + System.Environment.NewLine;
            set_point_list_text.text += "Move Forward" + System.Environment.NewLine;
            /* for (int i = 0; i < keywords.Count; i++)
             {
                 set_point_list_text.text += keywords[i].ToString()  + System.Environment.NewLine;
             }
             */
        });

        keywords.Add("Close Menu", () =>
        {
            set_point_list_text.text = "";
        });



        keywords.Add("Reset Path", () =>
        {
            path_point_counter = 0;
            set_point_list_text.text = "";
            path_points.Clear();
            path_points_global.Clear();
            path_normals.Clear();
            var clones = GameObject.FindGameObjectsWithTag("clone");
            foreach (var clone in clones)
            { Destroy(clone);
                //set_point_list_text.text += "1, ";
            }
            var clones_profile = GameObject.FindGameObjectsWithTag("clone_profile");
            foreach (var clone_profile in clones_profile)
            {
                Destroy(clone_profile);
                //set_point_list_text.text += "1, ";
            }


            // Call the OnReset method on every descendant object.
            //this.BroadcastMessage("OnReset");


            Vector3[] temp_global_points = new Vector3[2];
            temp_global_points[0] = new Vector3(0.0f,0.0f,0.0f);
            temp_global_points[1] = new Vector3(0.0f, 0.0f, 0.0f);
            lineConnecting.SetPositions(temp_global_points);
            lineConnecting.enabled = false;



        });


        keywords.Add("Open Hand", () =>
        {
            //GameObject.Find("WAM").GetComponent<WAMViewer>().bhandserv.OpenGrasp();

            GameObject.Find("WAM").GetComponent<WAMViewer>().open_hand();
        });

        keywords.Add("Close Hand", () =>
        {
            GameObject.Find("WAM").GetComponent<WAMViewer>().close_hand();
            // GameObject.Find("WAM").GetComponent<WAMViewer>().bhandserv.CloseGrasp();
        });


       /* keywords.Add("Drop Sphere", () =>
        {
            var focusObject = GazeGestureManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Call the OnDrop method on just the focused object.
                focusObject.SendMessage("OnDrop", SendMessageOptions.DontRequireReceiver);
            }
        });
        */

        keywords.Add("Display Profile", () =>
        {
            //WAM_t = GameObject.Find("WAM").transform;//CP
            //path_clone=Instantiate(path_sphere, GazeGestureManager.Instance.hit_point, Quaternion.identity);
            
            Vector3 temp_location;

            Vector3[] temp_global_points=new Vector3[path_points_global.Count];
            lineConnecting.numPositions = path_points_global.Count;
            lineConnecting.enabled = true;
            for (int i = 0; i < path_points_global.Count; i++)
            {
                profile_sphere.tag = "clone_profile";

                temp_location = path_points_global[i];
                temp_location.y = temp_location.y + 0.3f;
                Instantiate(profile_sphere,temp_location , Quaternion.identity);
     
                temp_global_points[i] = temp_location;

               // lineConnecting.SetPosition(i,temp_location);
            }
            lineConnecting.SetPositions(temp_global_points);
            
        });


        keywords.Add("Display line", () =>
        {
            //WAM_t = GameObject.Find("WAM").transform;//CP
            //path_clone=Instantiate(path_sphere, GazeGestureManager.Instance.hit_point, Quaternion.identity);

            Vector3 temp_location;

            Vector3[] temp_global_points = new Vector3[path_points_global.Count];
            lineConnecting.numPositions = path_points_global.Count;
            lineConnecting.enabled = true;
            for (int i = 0; i < path_points_global.Count; i++)
            {
                profile_sphere.tag = "clone_profile";

                temp_location = path_points_global[i];
                temp_location.y = temp_location.y + start_force_line;
                Instantiate(profile_sphere, temp_location, Quaternion.identity);

                temp_global_points[i] = temp_location;

                // lineConnecting.SetPosition(i,temp_location);
            }
            lineConnecting.SetPositions(temp_global_points);

        });


        keywords.Add("Display ramp", () =>
        {
            //WAM_t = GameObject.Find("WAM").transform;//CP
            //path_clone=Instantiate(path_sphere, GazeGestureManager.Instance.hit_point, Quaternion.identity);
           
            Vector3 temp_location;
            float a = 0.0f;
            float b = 0.0f;
            float A = 0.0f;
            float B = 0.0f;
            Vector3[] temp_global_points = new Vector3[path_points_global.Count];
            lineConnecting.numPositions = path_points_global.Count;
            lineConnecting.enabled = true;

            B = path_points_global[path_points_global.Count - 1].x - path_points_global[0].x;
            A = start_force-end_force;
            
            
            //m = (end_force - start_force)/(path_points_global[path_points_global.Count-1].x- path_points_global[0].x);

            for (int i = 0; i < path_points_global.Count; i++)
            {
                profile_sphere.tag = "clone_profile";

                temp_location = path_points_global[i];

                b = path_points_global[path_points_global.Count - 1].x-temp_location.x;
                a = A * b / B;

                temp_location.y = temp_location.y + a +end_force;

                Instantiate(profile_sphere, temp_location, Quaternion.identity);


                temp_global_points[i] = temp_location;

                // lineConnecting.SetPosition(i,temp_location);
            }
            lineConnecting.SetPositions(temp_global_points);

        });





        keywords.Add("Display arc", () =>
        {
            //WAM_t = GameObject.Find("WAM").transform;//CP
            //path_clone=Instantiate(path_sphere, GazeGestureManager.Instance.hit_point, Quaternion.identity);

            Vector3 temp_location;
            float trajectory_lenght=0.0f;
            float current_x_radians = 0.0f;
            float current_x = 0.0f;
            float sin_height = 0.0f;
            float calculate_value = 0.0f;
            Vector3[] temp_global_points = new Vector3[path_points_global.Count];
            lineConnecting.numPositions = path_points_global.Count;
            lineConnecting.enabled = true;
            for (int i = 0; i < path_points_global.Count; i++)
            {
                profile_sphere.tag = "clone_profile";

                temp_location = path_points_global[i];

                trajectory_lenght = path_points_global[path_points_global.Count-1].x-path_points_global[0].x;
                current_x = path_points_global[i].x - path_points_global[0].x;
                current_x_radians = 2 * Mathf.PI * current_x / trajectory_lenght;
                sin_height = Mathf.Sin(current_x_radians);

                calculate_value = start_force_sin + sin_amplitude * sin_height;

                temp_location.y = temp_location.y + calculate_value;
                Instantiate(profile_sphere, temp_location, Quaternion.identity);

                temp_global_points[i] = temp_location;

                // lineConnecting.SetPosition(i,temp_location);
            }
            lineConnecting.SetPositions(temp_global_points);

        });



        keywords.Add("Next Point", () =>
        {

            if (path_points.Count > 0)
            {
                //convert unity frame of reference to WAM frame of reference
                float x = path_points[path_point_counter].z;
                float y = -path_points[path_point_counter].x;
                float z = path_points[path_point_counter].y;
                Quaternion q_unity =Quaternion.FromToRotation(Vector3.down, path_normals[path_point_counter]);
                Quaternion q;
                q.w = q_unity.w;
                q.x = -(q_unity.z);
                q.y = -(-q_unity.x);
                q.z = -(q_unity.y);

                
                set_point_list_text.text = "(qx,qy,qz,qw)="+"("+ q.x +","+q.y+","+q.z+","+q.w+")";
                path_point_counter++;
                Vector3 p;
                p.x = x;
                p.y = y;
                p.z = z+ 0.2f;
                GameObject.Find("WAM").GetComponent<WAMViewer>().move_to_pose(p,q);

            }

            if (path_point_counter == path_points.Count)
            {
                path_point_counter = 0;
            }


        });



        keywords.Add("Lock Path", () =>
        {



            GameObject.Find("WAM").GetComponent<WAMViewer>().lock_path(path_points,path_normals, path_points.Count);

        });


        keywords.Add("Move Up", () =>
        {
            GameObject.Find("WAM").transform.position+=Vector3.up*0.01F;
        });

        keywords.Add("Move Down", () =>
        {
            GameObject.Find("WAM").transform.position -= Vector3.up * 0.01F;
            });

        keywords.Add("Move Left", () =>
        {
            GameObject.Find("WAM").transform.position += Vector3.right * 0.01F;
            });

        keywords.Add("Move Right", () =>
        {
            GameObject.Find("WAM").transform.position -= Vector3.right * 0.01F;
        });


        keywords.Add("Move Back", () =>
        {
            GameObject.Find("WAM").transform.position -= Vector3.forward * 0.01F;
        });

        keywords.Add("Move Forward", () =>
        {
            GameObject.Find("WAM").transform.position += Vector3.forward * 0.01F;
        });

        keywords.Add("Move Side", () =>
        {
            GameObject.Find("WAM").transform.position -= Vector3.right * 1.5F;
        });

        keywords.Add("Move Center", () =>
        {
            GameObject.Find("WAM").transform.position += Vector3.right * 1.5F;
        });



        keywords.Add("Set Point", () =>
        {
            WAM_t = GameObject.Find("WAM").transform;//CP
            //path_clone=Instantiate(path_sphere, GazeGestureManager.Instance.hit_point, Quaternion.identity);
            path_sphere.tag = "clone";
            Instantiate(path_sphere, GazeGestureManager.Instance.hit_point, Quaternion.identity);
            normal_arrow.tag = "clone";
            Instantiate(normal_arrow, GazeGestureManager.Instance.hit_point, Quaternion.FromToRotation(Vector3.forward, GazeGestureManager.Instance.hit_normal));
            //Vector3 relative_point_local=transform.InverseTransformPoint(GazeGestureManager.Instance.hit_point);  //CP: This should put the point in the local frame
            //Vector3 relative_point_global = transform.TransformPoint(GazeGestureManager.Instance.hit_point);  //CP: This should put the point in the world frame which is align with the robot frame
            //Vector3 relative_normal_global= transform.T
            //Vector3 relative_point_WAM=GameObject.Find("WAM").transform.TransformPoint(GazeGestureManager.Instance.hit_point); //CP: transform point in hololens frame to point in WAM coordinates
            //Vector3 relative_normal_WAM = GameObject.Find("WAM").transform.TransformDirection(GazeGestureManager.Instance.hit_normal);//CP:transform normal in hololens to normal in WAM frame, not sure about scale test it can be Transform Vector instead

            point_wrt_WAM = WAM_t.InverseTransformPoint(GazeGestureManager.Instance.hit_point);
            point_wrt_WAM = point_wrt_WAM * 0.001F;
            point_wrt_WAM.y = point_wrt_WAM.y - 0.354F;

            path_points.Add(point_wrt_WAM);
            path_points_global.Add(GazeGestureManager.Instance.hit_point);
            //path_points.Add(GazeGestureManager.Instance.hit_point);
            path_normals.Add(GazeGestureManager.Instance.hit_normal);//TODO: if a not flat surface is tested, this may need to change also to corrdinates relative to the WAM.

            /* Comment for user trials
            set_point_list_text.text +=  GazeGestureManager.Instance.hit_point.ToString("R") + ", (-y,z,x),value wrt wam->"+ point_wrt_WAM.ToString("R");//CP

    */
        });



        keywords.Add("Enable Force", () =>
        {
            GameObject.Find("WAM").GetComponent<ForceTorqueArrow>().render_arrow = true;
        });

        keywords.Add("Disable Force", () =>
        {
            GameObject.Find("WAM").GetComponent<ForceTorqueArrow>().render_arrow = false;
        });


        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}
using UnityEngine;

using UnityEngine.UI;
public class GazeGestureManager : MonoBehaviour
{
    public static GazeGestureManager Instance { get; private set; }

    // Represents the hologram that is currently being gazed at.
    public GameObject FocusedObject { get; private set; }
    public Text coordinate_text;
    public Vector3 hit_normal;
    public Vector3 hit_point;
    UnityEngine.XR.WSA.Input.GestureRecognizer recognizer;

    // Use this for initialization
    void Start()
    {
        Instance = this;

        // Set up a GestureRecognizer to detect Select gestures.
        recognizer = new UnityEngine.XR.WSA.Input.GestureRecognizer();
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            // Send an OnSelect message to the focused object and its ancestors.
            if (FocusedObject != null)
            {
                FocusedObject.SendMessageUpwards("OnSelect");
            }
        };
        recognizer.StartCapturingGestures();
    }

    // Update is called once per frame
    void Update()
    {
        float distance_x;
        float distance_y;
        float distance_z;
        // Figure out which hologram is focused this frame.
        GameObject oldFocusObject = FocusedObject;

        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {

            // If the raycast hit a hologram, use that as the focused object.
            FocusedObject = hitInfo.collider.gameObject;
            distance_x = hitInfo.point.x;
            distance_y = hitInfo.point.y;
            distance_z = hitInfo.point.z;
            hit_normal = hitInfo.normal;
            hit_point = hitInfo.point;
            //hit_normal = transform.TransformDirection(Vector3.forward) * 10;
            //Debug.DrawRay(transform.position, hit_normal, Color.blue);
            //Gizmos.color = Color.red;
            //Gizmos.DrawRay(hitInfo.point, hitInfo.normal);
            //Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.blue, 0, false);
            //print("The hit point is ("+distance_x+", " + distance_y + ", " + distance_z + ")");
            // string text_temp = "The hit point is (" + distance_x.ToString() + ", " + distance_y.ToString() + ", " + distance_z.ToString() + ")";
            //coordinate_text.text = "The hit point is (" + distance_x.ToString() + ", " + distance_y.ToString() + ", " + distance_z.ToString() + ")"; //uncomment to know hit point by head cursos
        }
        else
        {
            // If the raycast did not hit a hologram, clear the focused object.
            FocusedObject = null;
        }

        // If the focused object changed this frame,
        // start detecting fresh gestures again.
        if (FocusedObject != oldFocusObject)
        {
            recognizer.CancelGestures();
            recognizer.StartCapturingGestures();
        }
    }
}
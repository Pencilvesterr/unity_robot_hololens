using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RobotController : MonoBehaviour
{
    public float speed;
    public Text warningMessage;
    private Rigidbody rb;
    private bool isHit = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("barrier"))
        {
            GameObject.Find("WAM").GetComponent<WAMViewer>().free_path();
            //SceneManager.LoadScene("ROSscene");
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerCWS : MonoBehaviour {

    GameObject[] selectionRings;
    GameObject[] CWSSpaces;
    public int SelectedZoneHuman { get; set; } = -1;
    public int SelectedZoneRobot { get; set; } = -1;

    public Material DefaultMaterial;
    public Material HumanSelectedMaterial;
    public Material RobotSelectedMaterial;
    Dictionary<int, GameObject> Zones;

    void Start()
    {
        // Fine all CWS Zones based on their name
        Zones = new Dictionary<int, GameObject> { 
            {1, GameObject.Find("Zone Space 1")},
            {2, GameObject.Find("Zone Space 2")},
            {3, GameObject.Find("Zone Space 3")}
        };

        // Start all selection rings as invisible
        selectionRings = GameObject.FindGameObjectsWithTag("ZoneSelectionRing");
        foreach (GameObject selectingRing in selectionRings)
        {
            selectingRing.GetComponent<Renderer>().enabled = false;
        }
    }


    void Update()
    {

    }

    public void SetSelectedZoneHuman(int i)
    {
        if (Zones.ContainsKey(i))
        { 
        
            if (SelectedZoneHuman != -1)
            {
                Zones[SelectedZoneHuman].GetComponent<MeshRenderer>().material = DefaultMaterial;
            }

            SelectedZoneHuman = i;
            Zones[i].GetComponent<MeshRenderer>().material = HumanSelectedMaterial;
        }  
    }

    public void ClearAllZones()
    {
        Dictionary<int, GameObject>.KeyCollection keys = Zones.Keys;
        foreach (int key in keys)
        {
            Zones[key].GetComponent<MeshRenderer>().material = DefaultMaterial;
        }
    }

}

 


   


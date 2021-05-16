using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class ManagerBlocks : MonoBehaviour {
    // Tracks which block has been selected by the user and gives option to show rings or not

    public GameObject RingVisibilityToggle;
    private Interactable ToggleStatus;

    private Dictionary<int, GameObject> BlockRings;

    public int SelectedBlock { get; set; } = -1;

    void Start()
    {
        ToggleStatus = RingVisibilityToggle.GetComponent<Interactable>();
        BlockRings = new Dictionary<int, GameObject> { 
            {11, GameObject.Find("Ring 11")},
            {12, GameObject.Find("Ring 12")},
            {13, GameObject.Find("Ring 13")},
            {14, GameObject.Find("Ring 14")},
            {15, GameObject.Find("Ring 15")},
            {21, GameObject.Find("Ring 21")},
            {22, GameObject.Find("Ring 22")},
            {23, GameObject.Find("Ring 23")},
            {24, GameObject.Find("Ring 24")},
            {25, GameObject.Find("Ring 25")},
            {31, GameObject.Find("Ring 31")},
            {32, GameObject.Find("Ring 32")},
            {33, GameObject.Find("Ring 33")},
            {34, GameObject.Find("Ring 34")},
            {35, GameObject.Find("Ring 35")}
        };

        ToggleStatus.IsToggled = false;

        // Start all selection rings as invisible
        AllRingsInvisible();
    }

    public void SetGazeSelection(int block)
    {
        if (BlockRings.ContainsKey(block))
        { 
            // One has been selected before, so hide that ring
            if (SelectedBlock != -1)
            {
                BlockRings[SelectedBlock].SetActive(false);
            }
            SelectedBlock = block;

            if (ToggleStatus.IsToggled)
            {
                BlockRings[block].SetActive(true);
            }
        }  
    }

    private void SelectionVisible()
    {
        if (SelectedBlock != -1)
        {
            BlockRings[SelectedBlock].SetActive(true);
        }
    }

    public void AllRingsInvisible()
    {
        foreach (KeyValuePair<int, GameObject> entry in BlockRings)
        {
            entry.Value.SetActive(false);
        }
    }

    public void ToggleRingVisibility()
    {
        if (ToggleStatus.IsToggled)
        {
            SelectionVisible();
        }
        else
        {
            AllRingsInvisible();
        }
    }
}

 
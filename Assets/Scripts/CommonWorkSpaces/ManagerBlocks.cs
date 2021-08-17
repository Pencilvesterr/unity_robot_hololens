using System;
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

        // Find all ring objects and create a dict for each with key being ring number
        BlockRings = new Dictionary<int, GameObject>();
        GameObject[] BlockRingsList = GameObject.FindGameObjectsWithTag("ZoneSelectionRing");
        foreach (GameObject ring in BlockRingsList)
        {
            string[] split_name = ring.name.Split(' ');
            BlockRings.Add(Int32.Parse(split_name[1]), ring);
        }

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

 
using UnityEngine;
using System.Collections.Generic;
using ROSBridgeLib.geometry_msgs;

public class ConvertingArraytoString : MonoBehaviour {

    public string Listtoarray(List<Point32Msg> input) //list of point32msg to array, need to adapt to accepts all list
    {
        string Listarray = "[";
        for (int i = 0; i < input.Count; i++)
        {
            Listarray = Listarray + input[i].ToYAMLString();
            if (input.Count - i >= 1 && i < input.Count - 1)
                Listarray += ",";
        }
        Listarray += "]";
        return Listarray;
    }


    public string inttoarray(int[] input) //int to array
    {
        string intarray = "[";
        for (int i = 0; i < input.Length; i++)
        {
            intarray = intarray + input[i];
            if (input.Length - i >= 1 && i < input.Length - 1)
                intarray += ",";
        }
        intarray += "]";
        return intarray;
    }

    public string floattoarray(float[] input) //floats to array
    {
        string floatarray = "[";
        for (int i = 0; i < input.Length; i++)
        {
            floatarray = floatarray + input[i];
            if (input.Length - i >= 1 && i < input.Length - 1)
                floatarray += ",";
        }
        floatarray += "]";
        return floatarray;
    }

    public string doubletoarray(double[] input) //double to array
    {
        string doublearray = "[";
        for (int i = 0; i < input.Length; i++)
        {
            doublearray = doublearray + input[i];
            if (input.Length - i >= 1 && i < input.Length - 1)
                doublearray += ",";
        }
        doublearray += "]";
        return doublearray;
    }

    public string stringtoarray(string[] input) //string to array
    {
        string stringarray = "[";
        for (int i = 0; i < input.Length; i++)
        {
            stringarray = stringarray + input[i];
            if (input.Length - i >= 1 && i < input.Length - 1)
                stringarray += ",";
        }
        stringarray += "]";
        return stringarray;
    }
}

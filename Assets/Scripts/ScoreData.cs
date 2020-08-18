using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData : IComparable<ScoreData>
{
    public float score;
    public string playerName;

    public int CompareTo (ScoreData other) 
    {
        if (other == null) 
        {
            return 1;
        }

        if (this.score > other.score) 
        {
            return 1;
        }

        if (this.score < other.score)
        {
            return -1;
        }

        return 0;
    }

    //TODO: Figure out how to use this cause idk
}

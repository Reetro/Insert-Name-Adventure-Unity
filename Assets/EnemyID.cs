﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyID : MonoBehaviour
{
    int ID = 0;

    void Start()
    {
        ID = GeneralFunctions.GenID();
    }
    public int GetID()
    {
        return ID;
    }
}

using System;
using UnityEngine;

public class FirstSP : MonoBehaviour
{
    public static FirstSP Instance;

    private void Awake()
    {
        Instance = this;
    }
}

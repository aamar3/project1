using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Name : MonoBehaviour
{
   [SerializeField] private string instanceName;

    public string GetName() { return instanceName; }
}

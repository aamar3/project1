using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;

    public Transform GetShootPoint()
    {
        return shootPoint;
    }
}

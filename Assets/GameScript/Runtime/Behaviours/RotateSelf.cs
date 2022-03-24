using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSelf : MonoBehaviour
{
    private float _speed = 30f;

    void Update()
    {
        this.transform.Rotate(Vector3.up, Time.deltaTime * _speed);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRotate : MonoBehaviour
{
    public float RotationSpeed;
    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.up*RotationSpeed*Time.deltaTime);
    }
}

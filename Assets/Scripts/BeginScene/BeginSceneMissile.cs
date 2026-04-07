using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginSceneMissile : MonoBehaviour
{
    void Update()
    {
        this.transform.Rotate(Vector3.up, 20 * Time.deltaTime);
    }
}

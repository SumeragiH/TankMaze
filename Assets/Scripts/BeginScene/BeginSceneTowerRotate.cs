using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginSceneTowerRotate : MonoBehaviour
{
    void Update()
    {
        this.transform.Rotate(Vector3.down, 40 * Time.deltaTime);
    }
}

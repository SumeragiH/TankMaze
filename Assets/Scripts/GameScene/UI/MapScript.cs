using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    public GameObject PlayerTank;
    private Vector3 map;
    //摄像机位置移动，写在LateUpdate中
    private void LateUpdate()
    {
        if (PlayerTank == null)
        {
            return;
        }
        map=this.transform.position;
        map.x = PlayerTank.transform.position.x;
        map.z = PlayerTank.transform.position.z;
        this.transform.position = map;
    }
}

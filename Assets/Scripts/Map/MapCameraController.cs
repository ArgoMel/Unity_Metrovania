using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    private Vector3 offset;

    private void Start()
    {
        offset.z = transform.position.z;
    }
    private void Update()
    {
        transform.position = PlayerHealthController.instance.transform.position+offset;
    }
}

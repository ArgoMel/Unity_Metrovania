using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapActivator : MonoBehaviour
{
    private void Start()
    {
        MapController.instance.ActivateMap(SceneManager.GetActiveScene().name);  
    }
}

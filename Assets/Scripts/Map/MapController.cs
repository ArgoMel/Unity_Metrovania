using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public static MapController instance;

    public GameObject fullMapCam;
    private Dictionary<string, GameObject> maps = new();

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (var child in GetComponentsInChildren<Grid>(true)) 
            {
                maps.Add(child.gameObject.name,child.gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (var item in maps)
        {
            item.Value.SetActive(PlayerPrefs.GetInt("Map_" + item.Key) == 1);
        }
    }

    public void ActivateMap(string mapToActivate) 
    {
        if (maps.ContainsKey(mapToActivate))
        {
            maps[mapToActivate].SetActive(true);
            PlayerPrefs.SetInt("Map_"+ mapToActivate,1);
        }
    }
}

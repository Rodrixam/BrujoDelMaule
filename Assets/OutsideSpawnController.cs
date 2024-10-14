using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideSpawnController : MonoBehaviour
{
    [SerializeField]
    float timeToTry = 10f;

    [SerializeField][Range(0, 1)]
    float spawnChance = 0.5f;

    [SerializeField]
    GameObject brujo;

    public bool summoned = false;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("TryToSpawn", timeToTry);
    }

    bool TryToSpawn()
    {
        if(!summoned && Random.value <= spawnChance)
        {
            summoned = true;
            brujo.SetActive(true);
        }

        Invoke("TryToSpawn", timeToTry);
        return false;
    }
}

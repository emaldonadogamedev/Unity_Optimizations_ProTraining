using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnCooldown;
    public float objectLifetime;

    bool spawning;

    void Update()
    {
        if(!spawning){
            StartCoroutine(WaitAndSpawn(spawnCooldown));
        }
    }

    IEnumerator WaitAndSpawn(float secondsToWaitFor) {
        spawning = true;
        yield return new WaitForSeconds(secondsToWaitFor);
        GameObject spawned = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        Destroy(spawned, objectLifetime);
        spawning = false;
    }

}

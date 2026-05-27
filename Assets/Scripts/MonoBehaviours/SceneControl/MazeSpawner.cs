using System.Collections.Generic;
using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    public List<GameObject> Modules = new();

    protected List<GameObject> SpawnPoints = new();

	// Use this for initialization
	protected virtual void Start ()
	{
        SpawnPoints.AddRange(GameObject.FindGameObjectsWithTag("ModuleLoc"));

        foreach (var SpawnPoint in SpawnPoints)
        {
            Instantiate(
				Modules[Random.Range(0, Modules.Count)],
				SpawnPoint.transform.position,
				Quaternion.identity,
				this.transform);
        }
	}
	
	// Update is called once per frame
	// void Update ()
	// {
	// 	
	// }
}

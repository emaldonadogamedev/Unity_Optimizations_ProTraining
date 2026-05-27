using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSpawnerIndustry : MazeSpawner
{
   public GameObject playerPrefab;
   public GameObject playerStartLoc;

   protected override void Start()
   {
      base.Start();

      StaticBatchingUtility.Combine(this.gameObject);

      // Spawning the roomba controlled by the player
      Instantiate(playerPrefab, playerStartLoc.transform.position, Quaternion.identity);
   }
}
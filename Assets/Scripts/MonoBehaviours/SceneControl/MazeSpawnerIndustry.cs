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
      GameObject.Instantiate(playerPrefab, playerStartLoc.transform.position, Quaternion.identity);
   }
}
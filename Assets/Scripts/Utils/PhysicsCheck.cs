using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      Debug.Log($"OnTriggerEnter {gameObject.name}<->{other.gameObject.name}, {Time.frameCount}");
   }

   private void OnTriggerStay(Collider other)
   {
      Debug.Log($"OnTriggerStay {gameObject.name}<->{other.gameObject.name}, {Time.frameCount}");
   }

   private void OnTriggerExit(Collider other)
   {
      Debug.Log($"OnTriggerExit {gameObject.name}<->{other.gameObject.name}, {Time.frameCount}");
   }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingDish : MonoBehaviour
{
   public Order OrderType;

   void Awake()
   {
      gameObject.GetComponent<SpriteRenderer>().sprite = OrderType.Sprite;
   }
}

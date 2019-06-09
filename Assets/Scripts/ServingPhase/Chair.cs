using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Chair : MonoBehaviour
{
    public bool isOccupied;
    private SpriteOutline spriteOutline;

    public SpriteOutline SpriteOutline { get => spriteOutline; set => spriteOutline = value; }

    public void Start()
    {
        spriteOutline = GetComponent<SpriteOutline>();
        spriteOutline.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Customer customer = collision.GetComponent<Customer>();
        if (customer && !isOccupied)
            SpriteOutline.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Customer customer = collision.GetComponent<Customer>();
        if (customer)
            SpriteOutline.enabled = false;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction : MonoBehaviour
{
    public bool FacingRight = true;
    public Vector3 scale;

    void Start()
    {
        scale = transform.localScale;
    }

    public void CheckDirection(float direction)
    {
        // Checks direction and determines if the sprite should flip or not
        if (direction >= transform.position.x && !FacingRight)
        {
            scale.x = -scale.x;
            FacingRight = true;
        }
        else if (direction <= transform.position.x && FacingRight)
        {
            scale.x = -scale.x;
            FacingRight = false;
        }
        transform.localScale = scale;
    }
}

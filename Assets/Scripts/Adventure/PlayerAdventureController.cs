using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAdventureController : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private LayerMask LayerMask;
    [SerializeField] float minX, maxX;
    [SerializeField] private Camera MainCamera;

    private Vector3 touchPos;
    private Vector3 directiona;
    private Vector3 targetPos;
    private Direction direction;
    private Rigidbody2D rb;
    private Animator animator;
    private bool canAct = true;

    // Start is called before the first frame update
    void Start()
    {
        direction = GetComponent<Direction>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAct)
        {
            CastRay();
        }
    }

    void FixedUpdate()
    {
        // Move towards target position
        if (transform.position.x != targetPos.x)
        {
            animator.SetBool("isMoving", true);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, transform.position.y, transform.position.z), Speed * Time.deltaTime);

            // Reached target destination
            if (transform.position.x == targetPos.x)
            {
               animator.SetBool("isMoving", false);
                //  Debug.Log("Reached target destination");
            }
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y, transform.position.z);
    }

    void CastRay()
    {
        Vector3 worldPoint = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        worldPoint.z = MainCamera.transform.position.z;
        Ray ray = new Ray(worldPoint, new Vector3(0, 0, 1));
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit)
        {
            targetPos = hit.point; // Set target pos
            direction.CheckDirection(targetPos.x); // Face target pos
            // Debug.Log("Target Hit:" + targetPos.x);
        }
    }

    public void EnableAction()
    {
        canAct = true;
    }

    public void DisableAction()
    {
        canAct = false;
    }
}
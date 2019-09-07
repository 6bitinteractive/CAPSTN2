using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldMapController : MonoBehaviour
{
    [SerializeField] private LayerMask LayerMask;

    public UnityEvent OnWaypointEnter = new UnityEvent();

    private bool canAct = true;
    private GameObject player;
    private PathManager pathManager;
    private Animator animator;
    private WaypointUI waypointUI;

    void Awake()
    {
        player = gameObject;
        pathManager = GetComponent<PathManager>();
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

    void CastRay()
    {
        // Create ray cast from mouse input
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector2 mouseDirection = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 20f, LayerMask);

        if (hit)
        {
            Debug.Log(hit.collider.gameObject.name);
            Waypoint waypoint = hit.collider.gameObject.GetComponent<Waypoint>();
   
            // If player target is waypoint
            if (waypoint)
            {
                StartCoroutine(MovingToWaypoint(waypoint));
            }         
        }

        IEnumerator MovingToWaypoint(Waypoint waypoint)
        {
            waypointUI = waypoint.GetComponent<WaypointUI>();
            pathManager.NavigateTo(waypoint.transform.position);
            bool isAtTargetPosition = false;
            DisableAction(); // Disable player action while moving
            //animator.SetBool("isMoving", true);

            while (!isAtTargetPosition)
            {
                if (gameObject.transform.position == waypoint.transform.position)
                {
                    // gameObject.transform.Translate(0, 0.5f, 0);
                    OnWaypointEnter.Invoke();
                    isAtTargetPosition = true;
                    pathManager.Stop();
                    waypointUI.InvokeOnWaypointArrival();
                    yield break;
                }
                yield return 0;
            }
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
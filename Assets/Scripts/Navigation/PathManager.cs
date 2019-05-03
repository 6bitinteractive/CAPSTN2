using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public float MoveSpeed = 5.0f;

    private Stack<Vector3> currentPath;
    private Vector3 currentWaypointPosition;
    private float moveTimeTotal;
    private float moveTimeCurrent;

    void Update()
    {
        if (currentPath != null && currentPath.Count > 0)
        {
            if (moveTimeCurrent < moveTimeTotal)
            {
                moveTimeCurrent += Time.deltaTime;
                if (moveTimeCurrent > moveTimeTotal)
                {
                    moveTimeCurrent = moveTimeTotal;
                }

                transform.position = Vector3.Lerp(currentWaypointPosition, currentPath.Peek(), moveTimeCurrent / moveTimeTotal);
            }
            else
            {
                currentWaypointPosition = currentPath.Pop();
                if (currentPath.Count == 0)
                {
                    Stop();
                }
                else
                {
                    moveTimeCurrent = 0;
                    moveTimeTotal = (currentWaypointPosition - currentPath.Peek()).magnitude / MoveSpeed;
                }
            }
        }
    }

    public void NavigateTo(Vector3 destination)
    {
        // Implement A*
        // 1 - Initialize currentPath;
        // find the closest waypoint to the current position and the closest one to the destination position
        currentPath = new Stack<Vector3>();
        var currentNode = FindClosestWaypoint(transform.position);
        var endNode = FindClosestWaypoint(destination);

        // 2 - Check to make sure that node for both end points can be found and that they're different
        if (currentNode == null || endNode == null || currentNode == endNode)
        {
            return;
        }

        // 3 - Initialize open and closed lists
        // In A*, the open list keeps track of nodes that you want to visit;
        // closed list keeps track of nodes that have already been visited preventing the algorithm
        // from going into an infinite loop amd re-trying paths it's already examined
        var openList = new SortedList<float, Waypoint>();
        var closedList = new List<Waypoint>();

        // 4 - Add the current or start node to the open list and reset its path properties
        openList.Add(0, currentNode);
        currentNode.Previous = null;
        currentNode.Distance = 0.0f;

        // 5
        // What happens in this loop is that it'll start by examining the start node.
        // It will add all its neighbors to the open list with their cost as the path distance from the start node to that node plus the
        // direct distance from that node to the destination node.
        // In the next loop, it will pick the node most likely to be on the right path to the target by being the shortest distance to go
        // which gets the closest to the destination.
        // It'll then exmaine all of that node's neighbors, pick the best node again, and so on until it reaches its destination.

        // TODO: Possible optimizations
        // Store the distances between neighboring nodes in the nodes themselves to avoid recalculating them all the time (unless nodes are dynamic).
        // If nodes are dynamic, there needs to be a system to detect changes and recalculate.

        // Loop until there are no more nodes to explore
        while (openList.Count > 0)
        {
            // Set the current node to the lowest cost node in the open list and then remove that node from the list
            currentNode = openList.Values[0];
            openList.RemoveAt(0);

            var dist = currentNode.Distance;

            // Add to closed list so it cannot be visited again
            closedList.Add(currentNode);

            // Check if destination has been reached; if true, we simply exit the loop
            if (currentNode == endNode)
            {
                break;
            }

            // If destination hasn't been reached yet, we loop through all the node's neighbors
            foreach (var neighbor in currentNode.Neighbors)
            {
                // Skip any that have already been visited or are already in the queue to be visited
                if (closedList.Contains(neighbor) || openList.ContainsValue(neighbor))
                {
                    continue;
                }

                // Else, we set the previous node which lets us retrieve the full path later
                neighbor.Previous = currentNode;

                // Set the total path distance to get to that node by adding the path distance to this current node plus
                // the distance between this current node and this new node
                neighbor.Distance = dist + (neighbor.transform.position - currentNode.transform.position).magnitude;
                var distanceToTarget = (neighbor.transform.position - endNode.transform.position).magnitude;

                // Add the neighbor node to the open list and assign the cost
                openList.Add(neighbor.Distance + distanceToTarget, neighbor);

                // Note: In A*, the cost is g(n) + h(n) where g(n) is the path cost(distance) to get to node n, and
                // h(n) is known as the heuristic which is an estimate of how close node n is to the goal.
                // Often you can just use the straight line distance from the node to the goal, which is what we do here.
            }
        }

        // 6 - Recreate the path
        // Since currentPath is a stack, we just start at currentNode, which is the end of the path, and follow the previous nodes,
        // pushing them onto the stack. When we’re done (i.e. the previous node is null), currentPath has all the nodes that
        // make up the shortest path starting from the start node to the end node.
        if (currentNode == endNode)
        {
            while (currentNode.Previous != null)
            {
                currentPath.Push(currentNode.transform.position);
                currentNode = currentNode.Previous;
            }

            currentPath.Push(transform.position);
        }
    }

    // Reset currentPath and moveTime
    public void Stop()
    {
        currentPath = null;
        moveTimeTotal = 0;
        moveTimeCurrent = 0;
    }

    // TODO: Possible optimizations
    // 1 - Cache waypoints or come up of a way to avoid searching through the whole list to find the closest one
    // 2 - Use sqrMagnitude instead of magnitude to avoid calculating the square root
    private Waypoint FindClosestWaypoint(Vector3 target)
    {
        GameObject closest = null;
        float closestDist = Mathf.Infinity;

        foreach (var waypoint in GameObject.FindGameObjectsWithTag("Waypoint"))
        {
            var dist = (waypoint.transform.position - target).magnitude;
            if (dist < closestDist)
            {
                closest = waypoint;
                closestDist = dist;
            }
        }

        if (closest != null)
        {
            return closest.GetComponent<Waypoint>();
        }

        Debug.Log("Closest waypoint is null.");
        return null;
    }
}

// Reference: Waypoint Pathing System
// https://www.trickyfast.com/2017/09/21/building-a-waypoint-pathing-system-in-unity/

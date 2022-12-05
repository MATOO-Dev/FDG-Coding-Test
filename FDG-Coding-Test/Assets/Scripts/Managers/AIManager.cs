using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    //this is a modified version of Selzier's answer from https://answers.unity.com/questions/475066/how-to-get-a-random-point-on-navmesh.html
    //this was then converted to a donut-like shape using a modified version of TerXIII's approach from https://forum.unity.com/threads/random-between-two-unit-spheres.64363/
    //this function will grab a random point on the nav mesh between the minimum and maximum distance away from the origin
    public Vector3 GetRandomPointOnNavMesh(Vector3 originPos, float minDistance, float maxDistance)
    {
        //create random point
        Vector3 randomPoint = (Random.insideUnitCircle * Random.Range(minDistance, maxDistance));
        //flip y and z coordinates (y set to zero since a plane is assumed)
        randomPoint.z = randomPoint.y;
        randomPoint.y = 0;
        //add the origin position to center on origin
        randomPoint += originPos;
        //check for collision with navmesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, maxDistance, 1))
            return hit.position;
        else
        {
            //if outside navmesh, run again recursively
            //performance wise, this was tested with 1 million iterations, with recursion called ~3k times, with a maximum depth of 1
            return GetRandomPointOnNavMesh(originPos, minDistance, maxDistance);
        }
    }
}

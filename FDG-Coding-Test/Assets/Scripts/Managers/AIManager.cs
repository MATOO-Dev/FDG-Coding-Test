using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    //debug
    public int depth;
    public int maxDepth;
    public int totalRecursionCount;

    //this is a modified version of Selzier's answer from https://answers.unity.com/questions/475066/how-to-get-a-random-point-on-navmesh.html
    //this was then converted to a torus-like shape using a modified version of TerXIII's approach from https://forum.unity.com/threads/random-between-two-unit-spheres.64363/

    public Vector3 GetRandomPointOnNavMesh(Vector3 originPos, float minDistance, float maxDistance)
    {
        //create random point on unit sphere, scale it by a random value, and add that onto the origin position
        Vector3 randomPoint = (Random.insideUnitSphere * Random.Range(minDistance, maxDistance)) + originPos;
        //create hit variables
        NavMeshHit hit;
        Vector3 finalPos = Vector3.zero;
        //check if position intersects with navmesh
        if (NavMesh.SamplePosition(randomPoint, out hit, maxDistance, 1))
            finalPos = hit.position;
        else
        {
            finalPos = GetRandomPointOnNavMesh(originPos, minDistance, maxDistance);
        }
        return finalPos;
    }
}

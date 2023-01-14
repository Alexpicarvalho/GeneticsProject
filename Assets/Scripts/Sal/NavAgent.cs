using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavAgent : MonoBehaviour
{
    public Transform target;
    [SerializeField]float speed = 1f;
    Vector3[] path;
    int targetIndex;
    Vector3 targetLastPos;

    private void Start()
    {
       
        PathRequestManager.RequestPath(transform.position,target.position,OnPathFound);


    }
    private void Update()
    {
       
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
       
    }
    public void OnPathFound(Vector3[] newPath,bool pathSucessful)
    {
        if (pathSucessful)
        {
            path= newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }
    IEnumerator FollowPath() 
    {
        if (path.Length > 0)
        {
            Vector3 currentWayPoint = path[0];
            while (true)
            {
                if (transform.position == currentWayPoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length) yield break;
                    currentWayPoint = path[targetIndex];
                }
                transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}  

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public bool walkable; // N�o � um obstaculo
    public Vector3 worldPosition;

    public Node(bool _walkable, Vector3 _worldPosition)
    {
        this.walkable = _walkable;
        this.worldPosition = _worldPosition;
    }
}

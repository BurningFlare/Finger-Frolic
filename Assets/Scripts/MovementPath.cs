using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using JetBrains.Annotations;
using Mono.Cecil;

public class MovementPath : MonoBehaviour
{
    [SerializeField] protected List<Transform> _pathNodes = new List<Transform>();
    [SerializeField] protected bool _loop;
    protected bool reversedDirection;
    private void Awake()
    {
        reversedDirection = false;
    }

    public virtual void GetMovePosition()
    {

    }

    protected virtual void OnDrawGizmos()
    {
        Color32 color = Util.GetColorFromHash(GetHashCode());
        Gizmos.color = color;
        Gizmos.DrawLineStrip(_pathNodes.Select(n => n.position).ToArray(), _loop);
    }

    protected virtual void OnValidate()
    {
        if (!_pathNodes.Contains(transform))
        {
            _pathNodes.Add(transform);
        }
        int hasEnemy = 0;
    }

    public void AddPathNode()
    {
        GameObject newNode = new GameObject("PathNode " + _pathNodes.Count);
        newNode.transform.position = transform.position;
        newNode.transform.parent = transform;
        _pathNodes.Add(newNode.transform);
    }

    protected virtual void ValidateCurrentPath()
    {
        for (int i = 0; i < _pathNodes.Count; ++i)
        {

        }
    }
}

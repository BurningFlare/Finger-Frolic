using System;
using UnityEngine;

public class CollideRejectMoveScript : MonoBehaviour
{
    EntityTilePositionTracker _boundEntityTracker;
    Func<Collision2D, bool> EntityCheck { get; set; }

    private void Awake()
    {
        _boundEntityTracker = GetComponent<EntityTilePositionTracker>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _boundEntityTracker.RejectMove(collision);
    }
}
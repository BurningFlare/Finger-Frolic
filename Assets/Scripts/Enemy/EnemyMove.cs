using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    const int DEPTH_LIMIT = 100;

    public virtual void GetMovePosition()
    {

    }

    // uses A* to calculate the best path through the tilemap
    public IList<Vector2> GetBestPathTo(Vector2 position)
    {
        Vector2 startPosition = transform.position;
        return new List<Vector2>();
    }
}

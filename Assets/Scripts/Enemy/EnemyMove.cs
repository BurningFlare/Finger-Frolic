using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public virtual void GetMovePosition()
    {

    }

    public IList<Vector2> GetBestPathTo(Vector2 position)
    {

        return new List<Vector2>();
    }
}

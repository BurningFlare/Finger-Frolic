using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementPath))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AnimateMoving))]
public class EnemyBase : MonoBehaviour
{
    protected MovementPath _enemyMoveComponent;
    int _stunTimer;

    protected virtual void Start()
    {
        _enemyMoveComponent = GetComponent<MovementPath>();
        EnemyManager.Instance.AddEnemy(this);
        _stunTimer = 0;
    }

    protected virtual void OnDestroy()
    {
        EnemyManager.Instance.RemoveEnemy(this);
    }

    public void Move()
    {
        _enemyMoveComponent.AdvancePosition();
    }

    public bool IsStunned()
    {
        return _stunTimer > 0;
    }

    public void Stun(int time)
    {
        _stunTimer = time;
    }

    public void InputReceived(bool pressedDown)
    {
        if (IsStunned())
        {
            _stunTimer--;
        }
        else
        {
            if (_enemyMoveComponent != null)
            {
                Move();
            }
        }
    }
}
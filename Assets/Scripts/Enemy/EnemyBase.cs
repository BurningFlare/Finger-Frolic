using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyBase : MonoBehaviour
{
    protected EnemyMove enemyMoveComponent;
    int _stunTimer;

    protected virtual void Awake()
    {
        EnemyManager.instance.AddEnemy(this);
        _stunTimer = 0;
    }

    protected virtual void OnDestroy()
    {
        EnemyManager.instance.RemoveEnemy(this);
    }

    public void Move()
    {
        // TODO implement moving
        enemyMoveComponent.GetMovePosition();
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
            Move();
        }
    }
}
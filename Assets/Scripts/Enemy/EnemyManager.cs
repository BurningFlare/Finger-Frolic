using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyManager : MonoBehaviour
{

    private IList<EnemyBase> _enemiesList;

    // singleton
    public static EnemyManager Instance { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        _enemiesList = new List<EnemyBase>();
    }

    public void InputReceived(InputAction.CallbackContext context, bool pressedDown)
    {
        foreach (EnemyBase enemy in _enemiesList)
        {
            enemy.InputReceived(pressedDown);
        }
    }

    public void AddEnemy(EnemyBase enemy)
    {
        _enemiesList.Add(enemy);
    }

    public void RemoveEnemy(EnemyBase enemy)
    {
        _enemiesList.Remove(enemy);
    }
}

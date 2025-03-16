using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyManager : MonoBehaviour
{

    public IList<EnemyBase> EnemiesList { get; private set; }

    // singleton
    public static EnemyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        EnemiesList = new List<EnemyBase>();
    }

    public void InputReceived(InputAction.CallbackContext context, bool pressedDown)
    {
        foreach (EnemyBase enemy in EnemiesList)
        {
            enemy.InputReceived(pressedDown);
        }
    }

    public void AddEnemy(EnemyBase enemy)
    {
        EnemiesList.Add(enemy);
    }

    public void RemoveEnemy(EnemyBase enemy)
    {
        EnemiesList.Remove(enemy);
    }
}

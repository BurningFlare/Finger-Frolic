using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    private IList<EnemyBase> _enemiesList;

    // singleton
    public static EnemyManager instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        _enemiesList = new List<EnemyBase>();
    }

    void InputReceived(bool pressedDown)
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

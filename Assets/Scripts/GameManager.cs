using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    // singleton
    static GameManager instance;

    [SerializeField] private Tilemap _groundTilemap;
    [SerializeField] private TilesAndGameObjectsBinder _groundTilesManager;

    [SerializeField] private GameObject _player;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        _groundTilesManager = GetComponent<TilesAndGameObjectsBinder>();
        _groundTilesManager.LoadTiles(_groundTilemap);
        _groundTilesManager.TrackGameObject(_player);
    }

    private void Start()
    {
        StartCoroutine(PostStartSetup());
    }

    IEnumerator PostStartSetup()
    {
        yield return null;
        _groundTilesManager.PopulateEnemies(EnemyManager.Instance.EnemiesList);
        _groundTilesManager.PrintDebug();
    }
}

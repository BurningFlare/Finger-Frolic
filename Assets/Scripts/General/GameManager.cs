using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    // singleton
    public static GameManager Instance { get; private set; }

    public TilesAndGameObjectsBinder GroundTilesManager
    {
        get
        {
            return _groundTilesManager;
        }
    }

    [SerializeField] private Tilemap _groundTilemap;
    [SerializeField] private TilesAndGameObjectsBinder _groundTilesManager;

    [SerializeField] private GameObject _player;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
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

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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

    private PhysicsScene2D _entitiesPhysicsScene;
    private Scene _entitiesScene;

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
        _entitiesScene = SceneManager.CreateScene("EntitiesScene", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        _entitiesPhysicsScene = _entitiesScene.GetPhysicsScene2D();
        StartCoroutine(PostStartSetup());
    }

    IEnumerator PostStartSetup()
    {
        yield return null;
        _groundTilesManager.PopulateEnemies(EnemyManager.Instance.EnemiesList);
    }

    public void InputReceived(InputAction.CallbackContext context, Vector2 direction, bool isPressed)
    {
        PlayerScript.Instance.PlayerMovement.MoveSomewhere(context, direction, isPressed);
        EnemyManager.Instance.InputReceived(context, isPressed);
    }

    private void SkipAnims()
    {

    }
}

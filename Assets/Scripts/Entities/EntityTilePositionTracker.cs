using UnityEngine;

// purpose of this class is to track tile positions for ease of communicating with the tiles manager and movement scripts
[RequireComponent(typeof(AnimateMovingSomehow))]
public class EntityTilePositionTracker : MonoBehaviour
{
    Rigidbody2D _rb;
    AnimateMovingSomehow _mover;
    TilesAndGameObjectsBinder _tilesManager;
    Vector3Int _position;

    public Vector3Int CurrentTilePosition
    {
        get
        {
            return _position;
        }
        set
        {
            Vector3Int direction = value - _position;
            _mover.Move(_rb, new Vector2(direction.x, direction.y));
            _position = value;
        }
    }

    private void Awake()
    {
        _mover = GetComponent<AnimateMovingSomehow>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _tilesManager = GameManager.Instance.GroundTilesManager;
        _position = _tilesManager.GetTileArrayPos(transform.position);
    }
}
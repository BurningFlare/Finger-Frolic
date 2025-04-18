using UnityEngine;

// purpose of this class is to track tile positions for ease of communicating with the tiles manager and movement scripts
[RequireComponent(typeof(AnimateMoving))]
public class EntityTilePositionTracker : MonoBehaviour
{
    Rigidbody2D _rb;
    AnimateMoving _mover;
    TilesAndGameObjectsBinder _tilesManager;
    Vector3Int _originalPosition;
    Vector3Int _position;
    public System.Func<Vector3Int, TilesAndGameObjectsBinder.TileInfo, bool> MovementChecks { get; set; }

    public Vector3Int CurrentTilePosition
    {
        get
        {
            return _position;
        }
        // Setting this will also tween the entity position, be careful!!!
        set
        {
            if (MovementChecks == null || MovementChecks(value, _tilesManager.GetTileInfoAt(value)))
            {
                _originalPosition = _position;
                Vector3Int direction = value - _position;
                _tilesManager.Erase(_position, gameObject);
                _tilesManager.Set(value, gameObject, false);
                _mover.Move(_rb, new Vector2(direction.x, direction.y));
                _position = value;
            }
        }
    }

    public virtual void RejectMove(Collision2D collision)
    {
        _mover.RejectMove(_rb);
        _position = _originalPosition;
    }

    private void Awake()
    {
        _mover = GetComponent<AnimateMoving>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _tilesManager = GameManager.Instance.GroundTilesManager;
        _position = _tilesManager.GetTileArrayPos(transform.position);
    }
}
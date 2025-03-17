using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(EntityTilePositionTracker))]
public class MovementPath : MonoBehaviour
{
    [SerializeField] protected List<Transform> _pathNodes = new();
    [SerializeField] protected bool _loop = false;
    [SerializeField] protected bool _reversedDirection = false;

    EntityTilePositionTracker _positionTracker;

    TilesAndGameObjectsBinder _groundTilesManager;

    private int _DONOTACCESScurrNodeIndex;
    private int _DONOTACCESnextNodeIndex;
    protected int CurrNodeIndex
    {
        get
        {
            return _DONOTACCESScurrNodeIndex;
        }
        set
        {
            _DONOTACCESScurrNodeIndex = value;
            _currNodePosition = _groundTilesManager.GetTileArrayPos(_pathNodes[_DONOTACCESScurrNodeIndex].transform.position);
        }
    }
    protected int NextNodeIndex
    {
        get
        {
            return _DONOTACCESnextNodeIndex;
        }
        set
        {
            _DONOTACCESnextNodeIndex = value;
            _nextNodePosition = _groundTilesManager.GetTileArrayPos(_pathNodes[_DONOTACCESnextNodeIndex].transform.position);
        }
    }

    private Vector3Int _currNodePosition;
    private Vector3Int _nextNodePosition;

    private void Awake()
    {
        _positionTracker = GetComponent<EntityTilePositionTracker>();
    }

    private void Start()
    {
        _groundTilesManager = GameManager.Instance.GroundTilesManager;
        ValidateCurrentPath();
        // replace the enemy (which is the start node) with another start node
        // we want to start at our start node
        int possibleFirstNode = _pathNodes.IndexOf(transform);
        if (possibleFirstNode == -1)
        {
            Debug.LogWarning($"Please make sure the entity {name} is in the list of path nodes!!! (I don't know how you managed to remove it...)");
            gameObject.SetActive(false);
        }
        else
        {
            // replace the enemy in the list with another game object so that the enemy moving doesn't cause the path to not work properly
            GameObject startNodeReplacement = new GameObject("StartNode");
            startNodeReplacement.transform.parent = transform.parent;
            startNodeReplacement.transform.position = transform.position;
            _pathNodes[possibleFirstNode] = startNodeReplacement.transform;
            CurrNodeIndex = possibleFirstNode;
            UpdateNextNode();
        }
    }

    // assumes you are currently on the path
    public virtual Vector3Int GetMoveDirection()
    {
        return Vector3Int.Max(Vector3Int.Min(_nextNodePosition - _currNodePosition, Vector3Int.one), -Vector3Int.one);
    }

    public void AdvancePosition()
    {
        Vector3Int direction = GetMoveDirection();
        _positionTracker.CurrentTilePosition += direction;
        if (_positionTracker.CurrentTilePosition == _nextNodePosition)
        {
            AdvanceNextNode();
        }
        _groundTilesManager.PrintDebug();
    }

    protected void AdvanceNextNode()
    {
        CurrNodeIndex = NextNodeIndex;
        UpdateNextNode();
    }

    protected void UpdateNextNode()
    {
        int candidateNodeIndex = -1;
        if (_pathNodes.Count <= 1)
        {
            Debug.LogWarning($"Not enough nodes in {name} for pathing!!!");
            // if there aren't enough nodes this method will cause infinite recursion!!!
            return;
        }
        else if (_reversedDirection)
        {
            candidateNodeIndex = CurrNodeIndex - 1;
        }
        else
        {
            candidateNodeIndex = CurrNodeIndex + 1;
        }

        // check if invalid
        if (_loop)
        {
            if (candidateNodeIndex < 0)
            {
                NextNodeIndex = _pathNodes.Count - 1;
            }
            else if (candidateNodeIndex >= _pathNodes.Count)
            {
                NextNodeIndex = 0;
            }
            else
            {
                NextNodeIndex = candidateNodeIndex;
            }
        }
        else if (candidateNodeIndex < 0 || candidateNodeIndex >= _pathNodes.Count)
        {

            _reversedDirection = !_reversedDirection;
            UpdateNextNode();
        }
        else
        {
            NextNodeIndex = candidateNodeIndex;
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Color32 color = Util.GetColorFromHash(GetHashCode());
        Gizmos.color = color;
        Gizmos.DrawLineStrip(_pathNodes.Select(n => n.position).ToArray(), _loop);
    }

    protected virtual void OnValidate()
    {
        if (!_pathNodes.Contains(transform))
        {
            _pathNodes.Add(transform);
        }
    }

    public void AddPathNode()
    {
        GameObject newNode = new GameObject("PathNode " + _pathNodes.Count);
        newNode.transform.position = transform.position;
        newNode.transform.parent = transform.parent;
        _pathNodes.Add(newNode.transform);
    }

    // validates that all path items are within an axis of each other
    protected virtual void ValidateCurrentPath()
    {
        string pathInvalidString = "The path between {0} and {1} is not aligned on an axis and is invalid";
        TilesAndGameObjectsBinder groundTilesManager = GameManager.Instance.GroundTilesManager;
        Vector3Int prevNodePos = groundTilesManager.GetTileArrayPos(_pathNodes[0].position);
        if (_loop)
        {
            Vector3Int lastNodePos = groundTilesManager.GetTileArrayPos(_pathNodes[_pathNodes.Count - 1].position);
            if (!ValidatePathSegment(lastNodePos, prevNodePos))
            {
                Debug.LogWarning(string.Format(pathInvalidString, _pathNodes[_pathNodes.Count - 1].name, _pathNodes[0].name));
            }
        }

        for (int i = 1; i < _pathNodes.Count; ++i)
        {
            Vector3Int currNodePos = groundTilesManager.GetTileArrayPos(_pathNodes[i].position);
            if (!ValidatePathSegment(prevNodePos, currNodePos))
            {
                Debug.LogWarning(string.Format(pathInvalidString, _pathNodes[i - 1].name, _pathNodes[i].name));
            }
            prevNodePos = currNodePos;
        }
    }

    protected virtual bool ValidatePathSegment(Vector3Int first, Vector3Int second)
    {
        return first.x == second.x || first.y == second.y;
    }

    protected void PrintPositions()
    {
        foreach (Transform node in _pathNodes)
        {
            Debug.Log(_groundTilesManager.GetTileArrayPos(node.position));
        }
    }
}
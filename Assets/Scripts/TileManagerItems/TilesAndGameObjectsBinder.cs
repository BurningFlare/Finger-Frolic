using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TilesAndGameObjectsBinder : MonoBehaviour
{
    // not sure if this is needed but can't hurt to be safe
    static readonly TileInfo OUT_OF_BOUNDS = new TileInfo();
    [Tooltip("To define how far outside of the outer-most tiles enemies can move")]
    [SerializeField] private int margin = 1;

    public class TileInfo
    {
        public bool _hasTile;
        public GameObject _tileItem;

        public TileInfo(bool hasTile = false, GameObject tileItem = null)
        {
            _hasTile = hasTile;
            _tileItem = tileItem;
        }
    }

    Tilemap _tilemap;
    BoundsInt _bounds;

    IList<IList<TileInfo>> _tilesList;

    public void LoadTiles(Tilemap tiles)
    {
        tiles.CompressBounds();
        _bounds = tiles.cellBounds;
        tiles.size = _bounds.size + new Vector3Int(2 * margin, 2 * margin, 0);
        tiles.origin = tiles.origin - new Vector3Int(margin, margin, 0);
        tiles.ResizeBounds();
        _bounds = tiles.cellBounds;

        _tilemap = tiles;
        _tilesList = new List<IList<TileInfo>>(_bounds.size.x);
        for (int i = 0; i < _bounds.size.x; ++i)
        {
            _tilesList.Add(new List<TileInfo>(_bounds.size.y));
        }

        foreach (var point in _bounds.allPositionsWithin)
        {
            TileInfo currTileInfo = new TileInfo(tiles.HasTile(point));
            _tilesList[point.x - _bounds.xMin].Add(currTileInfo);
        }
    }

    public void PopulateEnemies(IList<EnemyBase> enemies)
    {
        foreach (EnemyBase enemy in enemies)
        {
            if (!TrackGameObject(enemy.gameObject))
            {
                Debug.LogWarning("Enemy was not added!");
            }
        }
    }

    public bool TrackGameObject(GameObject gameObjectToTrack)
    {
        Vector3Int position = GetTileArrayPos(gameObjectToTrack.transform.position);
        if (_tilesList[position.x][position.y]._tileItem != null)
        {
            Debug.LogWarning($"Something is overlapping with object on the tilemap at {position.x}, {position.y}");
            return false;
        }
        else
        {
            _tilesList[position.x][position.y]._tileItem = gameObjectToTrack;
            return true;
        }
    }

    public void Set(Vector3Int tilePos, GameObject gameObj, bool checkEmpty = true)
    {
        if (!InBounds(tilePos))
        {
            Debug.LogWarning($"Tried to set position {tilePos} to {gameObj.name} but was out of bounds");
            return;
        }

        if (checkEmpty && _tilesList[tilePos.x][tilePos.y]._tileItem != null)
        {
            Debug.LogWarning($"{_tilesList[tilePos.x][tilePos.y]._tileItem.name} was already in {tilePos.x}, {tilePos.y}!");
        }
        _tilesList[tilePos.x][tilePos.y]._tileItem = gameObj;
    }

    public bool Erase(GameObject objToErase, bool checkErase = true)
    {
        if (GetGameObjectAt(objToErase.transform.position) != objToErase)
        {
            Debug.LogWarning($"The object you tried to erase is not at the same spot anymore, please track the object properly!");
            return false;
        }
        return Erase(objToErase.transform.position, checkErase);
    }

    public bool Erase(Vector2 pos, bool checkErase = true)
    {
        Vector3Int position = GetTileArrayPos(pos);
        if (_tilesList[position.x][position.y]._tileItem != null)
        {
            _tilesList[position.x][position.y]._tileItem = null;
            return true;
        }
        if (checkErase)
        {
            Debug.LogWarning($"There was nothing to erase at {position.x}, {position.y}");
        }
        return false;
    }

    public bool Erase(Vector3Int tilePos, bool checkErase = true)
    {
        if (_tilesList[tilePos.x][tilePos.y]._tileItem != null)
        {
            _tilesList[tilePos.x][tilePos.y]._tileItem = null;
            return true;
        }
        if (checkErase)
        {
            Debug.LogWarning($"There was nothing to erase at {tilePos.x}, {tilePos.y}");
        }
        return false;
    }

    public GameObject GetGameObjectAt(Vector2 pos)
    {
        Vector3Int position = GetTileArrayPos(pos);
        return GetGameObjectAt(position);
    }

    public GameObject GetGameObjectAt(Vector3Int tilePos)
    {
        return GetTileInfoAt(tilePos)._tileItem;
    }

    public TileInfo GetTileInfoAt(Vector3Int tilePos)
    {
        if (InBounds(tilePos))
        {
            return _tilesList[tilePos.x][tilePos.y];
        }
        return OUT_OF_BOUNDS;
    }

    public bool InBounds(Vector3Int tilePos)
    {
        return tilePos.x >= 0 && tilePos.x < _bounds.size.x && tilePos.y >= 0 && tilePos.y < _bounds.size.y;
    }

    public bool IsOccupied(Vector2 pos)
    {
        return GetGameObjectAt(pos) != null;
    }

    public Vector3Int GetTileArrayPos(Vector2 worldPos)
    {
        return _tilemap.WorldToCell(worldPos) - _bounds.min;
    }

    public Vector2 GetWorldPos(Vector3Int tileArrayPos)
    {
        return _tilemap.CellToWorld(tileArrayPos + _bounds.min);
    }

    public void PrintDebug()
    {
        string currString = "";
        for (int y = _tilesList[0].Count - 1; y >= 0; --y)
        {
            for (int x = 0; x < _tilesList.Count; ++x)
            {
                if (_tilesList[x][y]._tileItem == null)
                {
                    currString += _tilesList[x][y]._hasTile ? "X" : "_";
                }
                else
                {
                    currString += _tilesList[x][y]._tileItem.GetComponent<EnemyBase>() != null ? "E" : "P";
                }
            }
            currString += "\n";
        }
        Debug.Log(currString);
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TilesAndGameObjectsBinder : MonoBehaviour
{
    class TileInfo
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
        _tilemap = tiles;
        _bounds = tiles.cellBounds;
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
                Debug.Log("Enemy was not added!");
            }
        }
    }

    public bool TrackGameObject(GameObject gameObjectToTrack)
    {


        Vector3Int position = GetTileArrayPos(gameObjectToTrack.transform.position);
        if (_tilesList[position.x][position.y]._tileItem != null)
        {
            Debug.Log($"Something is overlapping with object on the tilemap at {position.x}, {position.y}");
            return false;
        }
        else
        {
            _tilesList[position.x][position.y]._tileItem = gameObjectToTrack;
            return true;
        }
    }

    public bool Erase(GameObject objToErase, bool checkErase = true)
    {
        if (GetGameObjectAt(objToErase.transform.position) != objToErase)
        {
            Debug.Log($"The object you tried to erase is not at the same spot anymore, please track the object properly!");
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
            Debug.Log($"There was nothing to erase at {position.x}, {position.y}");
        }
        return false;
    }

    public GameObject GetGameObjectAt(Vector2 pos)
    {
        Vector3Int position = GetTileArrayPos(pos);
        return _tilesList[position.x][position.y]._tileItem;
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
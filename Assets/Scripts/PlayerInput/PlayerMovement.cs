using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public void MoveSomewhere(InputAction.CallbackContext context, Vector2 direction, bool isPressed)
    {
        PlayerScript.Instance.PlayerPositionTracker.CurrentTilePosition += Vector3Int.RoundToInt(direction);
    }

    public bool CheckPlayerMovement(Vector3Int newPosition, TilesAndGameObjectsBinder.TileInfo tileInfo)
    {
        return tileInfo._hasTile;
    }
}

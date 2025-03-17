using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private void Start()
    {
        PlayerScript.Instance.PlayerPositionTracker.MovementChecks = CheckPlayerMovement;
    }

    public void MoveSomewhere(InputAction.CallbackContext context, Vector2 direction, bool isPressed)
    {
        PlayerScript.Instance.PlayerPositionTracker.CurrentTilePosition += Vector3Int.RoundToInt(direction);
    }

    public bool CheckPlayerMovement(Vector3Int newPosition, TilesAndGameObjectsBinder.TileInfo tileInfo)
    {
        if (!tileInfo._hasTile)
        {
            Debug.Log("Fell off path");
        }
        return true;
    }
}

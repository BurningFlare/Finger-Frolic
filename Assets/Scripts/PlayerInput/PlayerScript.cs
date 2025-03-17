using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(EntityTilePositionTracker))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerScript : MonoBehaviour
{
    public static PlayerScript Instance { get; private set; }
    public IA_PlayerControls PlayerInputActions { get; private set; }
    public PlayerControls PlayerControls { get; private set; }
    public EntityTilePositionTracker PlayerPositionTracker { get; private set; }
    public PlayerMovement PlayerMovement { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerPositionTracker = GetComponent<EntityTilePositionTracker>();
        PlayerControls = GetComponent<PlayerControls>();
        PlayerInputActions = new IA_PlayerControls();
    }
    void Start()
    {
        PlayerControls.BindControls(PlayerInputActions);
        PlayerInputActions.Player.Enable();
    }
}

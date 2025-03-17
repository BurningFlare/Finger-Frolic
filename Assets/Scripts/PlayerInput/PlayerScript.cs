using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript Instance { get; private set; }
    private Rigidbody2D _rb;
    private IA_PlayerControls PlayerInputActions;
    private PlayerControls _playerControls;
    private AnimateMoving _movementController;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _movementController = GetComponent<AnimateMoving>();
        _playerControls = GetComponent<PlayerControls>();
        PlayerInputActions = new IA_PlayerControls();
        _playerControls.BindControls(PlayerInputActions);
        PlayerInputActions.Player.Enable();

    }

    public void moveSomewhere(InputAction.CallbackContext context, Vector2 direction, bool isPressed)
    {
        //Debug.Log($"{context.action}, {isPressed}");
        _movementController.Move(_rb, direction, Ease.OutQuad);
    }

}

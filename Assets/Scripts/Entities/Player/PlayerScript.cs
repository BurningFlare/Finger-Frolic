using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript Instance { get; private set; }
    private Rigidbody2D _rb;
    private ActionInputPlayerThing PlayerInputActions;
    private PlayerMovement _playerMovement;
    private AnimateMovingSomehow _movementController;

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
        _movementController = GetComponent<AnimateMovingSomehow>();
        _playerMovement = GetComponent<PlayerMovement>();
        PlayerInputActions = new ActionInputPlayerThing();
        _playerMovement.BindControls(PlayerInputActions);
        PlayerInputActions.Player.Enable();

    }

    public void moveSomewhere(InputAction.CallbackContext context, Vector2 direction, bool isPressed)
    {
        //Debug.Log($"{context.action}, {isPressed}");
        _movementController.Move(_rb, direction, Ease.OutQuad);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

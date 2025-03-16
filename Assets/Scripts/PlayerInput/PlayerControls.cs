using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerInput _playerInput;

    public static PlayerControls Instance { get; private set; }
    public ActionInputPlayerThing PlayerInputActions { get; private set; }
    private AnimateMovingSomehow _movementController;

    ControlToggler WPress;
    ControlToggler SPress;
    ControlToggler APress;
    ControlToggler DPress;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _movementController = GetComponent<AnimateMovingSomehow>();
        WPress = new ControlToggler(new Vector2(0, 1));
        SPress = new ControlToggler(new Vector2(0, -1));
        APress = new ControlToggler(new Vector2(-1, 0));
        DPress = new ControlToggler(new Vector2(1, 0));
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        PlayerInputActions = new ActionInputPlayerThing();
        BindControls();
        PlayerInputActions.Player.Enable();
    }

    public class ControlToggler
    {
        private readonly System.Action<InputAction.CallbackContext, bool> callback;
        private readonly System.Action<InputAction.CallbackContext, bool> enemyCallback;
        bool keyPressed = false;

        public ControlToggler(Vector2 direction)
        {
            callback = (context, isPressed) =>
            {
                PlayerControls.Instance.moveSomewhere(context, direction, isPressed);
            };
            if (EnemyManager.Instance == null)
            {
                Debug.LogWarning("EnemyManager was null... check if you have one in the scene");
            }
            enemyCallback = EnemyManager.Instance.InputReceived;
        }

        public void checkOnOrOff(bool p, InputAction.CallbackContext contextOnOrOff)
        {
            if (keyPressed != p)
            {
                keyPressed = p;
                callback(contextOnOrOff, keyPressed);
                enemyCallback(contextOnOrOff, keyPressed);
            }
        }

    }

    private void BindControls()
    {
        PlayerInputActions.Player.W.performed += context => WPress.checkOnOrOff(true, context);
        PlayerInputActions.Player.W.canceled += context => WPress.checkOnOrOff(false, context);
        PlayerInputActions.Player.S.performed += context => SPress.checkOnOrOff(true, context);
        PlayerInputActions.Player.S.canceled += context => SPress.checkOnOrOff(false, context);
        PlayerInputActions.Player.A.performed += context => APress.checkOnOrOff(true, context);
        PlayerInputActions.Player.A.canceled += context => APress.checkOnOrOff(false, context);
        PlayerInputActions.Player.D.performed += context => DPress.checkOnOrOff(true, context);
        PlayerInputActions.Player.D.canceled += context => DPress.checkOnOrOff(false, context);
    }

    private void moveSomewhere(InputAction.CallbackContext context, Vector2 direction, bool isPressed)
    {
        //Debug.Log($"{context.action}, {isPressed}");
        _movementController.Move(_rb, direction, Ease.OutQuad);
    }
}

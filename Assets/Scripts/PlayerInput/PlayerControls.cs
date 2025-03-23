using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public static PlayerControls Instance { get; private set; }
    
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
        WPress = new ControlToggler(new Vector2(0, 1));
        SPress = new ControlToggler(new Vector2(0, -1));
        APress = new ControlToggler(new Vector2(-1, 0));
        DPress = new ControlToggler(new Vector2(1, 0));
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
                PlayerScript.Instance.PlayerMovement.MoveSomewhere(context, direction, isPressed);
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

    public void BindControls(IA_PlayerControls PlayerInputActions)
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

    
}

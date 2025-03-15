using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviour : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerInput _playerInput;
    
  
    public ActionInputPlayerThing PlayerInputActions {get; private set;}
    

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        PlayerInputActions = new ActionInputPlayerThing();
        BindControls();
        PlayerInputActions.Player.Enable();
    }


    private void BindControls()
    {
        Debug.Log("bindcontrols");
        PlayerInputActions.Player.W.performed += context => moveSomewhere(context, new Vector2(0,1), true);
        PlayerInputActions.Player.W.canceled += context => moveSomewhere(context, new Vector2(0, 1), false);
        PlayerInputActions.Player.S.performed += context => moveSomewhere(context, new Vector2(0, -1), true);
        PlayerInputActions.Player.S.canceled += context => moveSomewhere(context, new Vector2(0, -1), false);
        PlayerInputActions.Player.A.performed += context => moveSomewhere(context, new Vector2(-1, 0), true);
        PlayerInputActions.Player.A.canceled += context => moveSomewhere(context, new Vector2(-1, 0), false);
        PlayerInputActions.Player.D.performed += context => moveSomewhere(context, new Vector2(1, 0), true);
        PlayerInputActions.Player.D.canceled += context => moveSomewhere(context, new Vector2(1, 0), false);
    }

    private void moveSomewhere(InputAction.CallbackContext context, Vector2 direction,bool isPressed)
    {
        if (isPressed)
        {
            Debug.Log("PRESSED KEY");
            _rb.MovePosition(_rb.position + direction);
        }
        else
        {
            Debug.Log("LET GO OF KEY");
            _rb.MovePosition(_rb.position + direction);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
public class PlayerActions : PlayerActionSet
{
    public PlayerAction Fire;
    public PlayerAction Jump;
    public PlayerAction Left;
    public PlayerAction Right;
    public PlayerAction Up;
    public PlayerAction Down;
    public PlayerTwoAxisAction Move;


    public PlayerActions()
    {
        Fire = CreatePlayerAction("Fire");
        Jump = CreatePlayerAction("Jump");
        Left = CreatePlayerAction("Move Left");
        Right = CreatePlayerAction("Move Right");
        Up = CreatePlayerAction("Move Up");
        Down = CreatePlayerAction("Move Down");
        Move = CreateTwoAxisPlayerAction(Left, Right, Down, Up);
    }
}

public class HeroController : MonoBehaviour
{
    PlayerActions playerInput;
    string saveData;


    void OnEnable()
    {
        playerInput = new PlayerActions();

        playerInput.Fire.AddDefaultBinding(Key.Shift, Key.A);
        playerInput.Fire.AddDefaultBinding(InputControlType.Action1);
        playerInput.Fire.AddDefaultBinding(Mouse.LeftButton);
        playerInput.Fire.AddDefaultBinding(Mouse.PositiveScrollWheel);

        playerInput.Jump.AddDefaultBinding(Key.Space);
        playerInput.Jump.AddDefaultBinding(InputControlType.Action3);
        playerInput.Jump.AddDefaultBinding(InputControlType.Back);
        playerInput.Jump.AddDefaultBinding(InputControlType.System);
        playerInput.Jump.AddDefaultBinding(Mouse.NegativeScrollWheel);

        playerInput.Up.AddDefaultBinding(Key.UpArrow);
        playerInput.Down.AddDefaultBinding(Key.DownArrow);
        playerInput.Left.AddDefaultBinding(Key.LeftArrow);
        playerInput.Right.AddDefaultBinding(Key.RightArrow);

        playerInput.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        playerInput.Right.AddDefaultBinding(InputControlType.LeftStickRight);
        playerInput.Up.AddDefaultBinding(InputControlType.LeftStickUp);
        playerInput.Down.AddDefaultBinding(InputControlType.LeftStickDown);

        playerInput.Up.AddDefaultBinding(Mouse.PositiveY);
        playerInput.Down.AddDefaultBinding(Mouse.NegativeY);
        playerInput.Left.AddDefaultBinding(Mouse.NegativeX);
        playerInput.Right.AddDefaultBinding(Mouse.PositiveX);

        playerInput.ListenOptions.MaxAllowedBindings = 3;

        playerInput.ListenOptions.OnBindingFound = (action, binding) =>
        {
            if (binding == new KeyBindingSource(Key.Escape))
            {
                action.StopListeningForBinding();
                return false;
            }
            return true;
        };

        playerInput.ListenOptions.OnBindingAdded += (action, binding) =>
        {
            Debug.Log("Added binding... " + binding.DeviceName + ": " + binding.Name);
        };
    }


    void Update()
    {
        //transform.Rotate(Vector3.down, 500.0f * Time.deltaTime * playerInput.Move.X, Space.World);
        //transform.Rotate(Vector3.right, 500.0f * Time.deltaTime * playerInput.Move.Y, Space.World);

        //var fireColor = playerInput.Fire.IsPressed ? Color.red : Color.white;
        //var jumpColor = playerInput.Jump.IsPressed ? Color.green : Color.white;

        //GetComponent<Renderer>().material.color = Color.Lerp(fireColor, jumpColor, 0.5f);

        if (playerInput.Fire.IsPressed)
        {
            Debug.LogError("----Fire--");
        }
        if (playerInput.Jump.IsPressed)
        {
            Debug.LogError("----Jump--");
        }

    }	
}

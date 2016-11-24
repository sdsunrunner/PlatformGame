/// <summary>
/// The 'Invoker' class that makes calls to execute the commands
/// </summary>

using UnityEngine;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour
{
    public float moveDistance = 10f;
    public GameObject objectToMove;

    private MoveCommandReceiver moveCommandReciever;
    private List<MoveCommand> commands = new List<MoveCommand>();
    private int currentCommandNum = 0;

    void Start()
    {
        moveCommandReciever = new MoveCommandReceiver();
        if (objectToMove == null)
        {
            Debug.LogError("objectToMove must be assigned via inspector");
            this.enabled = false;
        }
    }

    private void Move(MoveDirectionEnum direction)
    {
        MoveCommand moveCommand = new MoveCommand(moveCommandReciever, direction, moveDistance, objectToMove);
        //moveCommand.Execute();
        //commands.Add(moveCommand);
        //currentCommandNum++;
    }
   
    public void MoveUp()
    { 
        Move(MoveDirectionEnum.up); 
    }
    public void MoveDown()
    {
        Move(MoveDirectionEnum.down); 
    }
    public void MoveLeft() 
    { 
        Move(MoveDirectionEnum.left);
    }
    public void MoveRight()
    { 
        Move(MoveDirectionEnum.right);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDown();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
           
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            
        }
    }
}

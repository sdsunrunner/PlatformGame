/// <summary>
/// A simple example of a class inheriting from a command pattern
/// This handles execution of the command as well as unexecution of the command
/// </summary>

using UnityEngine;

public class MoveCommand:ICommand
{
    private MoveDirectionEnum _direction;
    private MoveCommandReceiver _receiver;
    private float _distance;
    private GameObject _gameObject;


    //Constructor
    public MoveCommand(MoveCommandReceiver reciever, MoveDirectionEnum direction, float distance, GameObject gameObjectToMove)
    {
        this._receiver = reciever;
        this._direction = direction;
        this._distance = distance;
        this._gameObject = gameObjectToMove;
    }

    //Execute new command
    public void excute(INotification note)
    {
        _receiver.MoveOperation(_gameObject, _direction, _distance);
    } 


    //invert the direction for undo
    private MoveDirectionEnum InverseDirection(MoveDirectionEnum direction)
    {
        switch (direction)
        {
            case MoveDirectionEnum.up:
                return MoveDirectionEnum.down;
            case MoveDirectionEnum.down:
                return MoveDirectionEnum.up;
            case MoveDirectionEnum.left:
                return MoveDirectionEnum.right;
            case MoveDirectionEnum.right:
                return MoveDirectionEnum.left;
            default:
                Debug.LogError("Unknown MoveDirection");
                return MoveDirectionEnum.up;
        }
    }


    //So we can show this command in debug output easily
    public override string ToString()
    {
        return _gameObject.name + " : " + MoveDirectionString(_direction) + " : " + _distance.ToString();
    }


    //Convert the MoveDirection enum to a string for debug
    public string MoveDirectionString(MoveDirectionEnum direction)
    {
        switch (direction)
        {
            case MoveDirectionEnum.up:
                return "up";
            case MoveDirectionEnum.down:
                return "down";
            case MoveDirectionEnum.left:
                return "left";
            case MoveDirectionEnum.right:
                return "right";
            default:
                return "unkown";
        }
    }
}

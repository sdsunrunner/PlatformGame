using UnityEngine;
public class MoveCommandReceiver
{
    public void MoveOperation(GameObject gameObjectToMove, MoveDirectionEnum direction, float distance)
    {
        switch (direction)
        {
            case MoveDirectionEnum.up:
                MoveY(gameObjectToMove, distance);
                break;
            case MoveDirectionEnum.down:
                MoveY(gameObjectToMove, -distance);
                break;
            case MoveDirectionEnum.left:
                MoveX(gameObjectToMove, -distance);
                break;
            case MoveDirectionEnum.right:
                MoveX(gameObjectToMove, distance);
                break;
        }
    }

    private void MoveY(GameObject gameObjectToMove, float distance)
    {
        Vector3 newPos = gameObjectToMove.transform.position;
        newPos.y += distance;
        gameObjectToMove.transform.position = newPos;
    }

    private void MoveX(GameObject gameObjectToMove, float distance)
    {
        Vector3 newPos = gameObjectToMove.transform.position;
        newPos.x += distance;
        gameObjectToMove.transform.position = newPos;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerChecker : MonoBehaviour
{
    bool[,] levelMap;

    Vector2Int path;

    // Start is called before the first frame update
    public void Configure(bool[,] levelMap)
    {
        this.levelMap = levelMap;
    }

    // Update is called once per frame
    public bool CheckAnswer(Vector2Int firstCard, Vector2Int secondCard)
    {
        // We try to go straight to our desired y pos
            // We try our current pos, and all the possible left and right positions.
        
        // If none of these work
            // We go down
            // Then check if we can go right or left to find a path straight to our card
            // If we can were done, if we can't there is no path to this card.


        // 1 for up in the array
        // 0 for same level
        // -1 for down in the array
        int up = secondCard.y > firstCard.y ? 1 : firstCard.y > secondCard.y ? -1 : 0;

        // 1 for left in the array
        // 0 for same level
        // -1 for right in the array
        int right = firstCard.x < secondCard.x ? 1 : firstCard.x > secondCard.x ? -1 : 0;

        // Current steps of the path;
        path = firstCard;

        int turnsTaken = 0;

        if (up != 0)
        {
            // Check all right or left paths straight upwards

            bool foundPath = false;
            // Check all right paths.
            for (int i = path.x; i < (levelMap.GetLength(0) - 1) - path.x; i++) 
            {
                if(CheckIfStraightPathToYPos(new Vector2Int(path.y, i), secondCard.y, up > 0, true)) 
                {
                    foundPath = true;
                    break;
                }
            }

            // Check left
            if (foundPath == false) 
            {
                for (int i = path.x; i >= 0; i--)
                {
                    if (CheckIfStraightPathToYPos(new Vector2Int(path.y, i), secondCard.y, up > 0, true))
                    {
                        foundPath = true;
                        break;
                    }
                }
            }

            if (foundPath) 
            {
                // If we found a path to it, but not at the same x, we took a turn.
                if (path.x != firstCard.x) turnsTaken++;

                if (path == secondCard) return true;

                // Now we check if we can go straight to it on the x-axis;
                if(CheckIfStraightPathToXPos(path, secondCard.x, path.x < secondCard.x, true)) 
                {
                    turnsTaken++;
                }

                if (path == secondCard) return true;

                if(turnsTaken < 2) 
                {
                    // We can try to go straight up to our target card
                    if(CheckIfStraightPathToYPos(path, secondCard.y, path.y < secondCard.y, true))
                    {
                        return true; // We reached it with less than 2 turns;
                    }
                }
            }

            // We couldnt reach it going right left or up
            // That means we need to go in the opposite y direction first.
            // We reset the position first
            path = firstCard;

            // Go one step in the opposite direction;
            path.y -= up;

            // We reuse the foundPath bool.
            foundPath = false;

            // We keep going down and see if theres a straight path to the x we need to get to.
            while(path.y - up >= 0 && path.y - up < levelMap.GetLength(0)) 
            {
                if(levelMap[path.y, path.x] == false) 
                {
                    // Check if there is a path to the right.
                    if (path.x + 1 < levelMap.GetLength(0))
                    {
                        if (CheckIfStraightPathToXPos(path, secondCard.x, true, true)) 
                        {
                            foundPath = true;
                            break;
                        }
                    }

                    // Check if there is a path to the left.
                    if (path.x - 1 >= 0)
                    {
                        if (CheckIfStraightPathToXPos(path, secondCard.x, false, true))
                        {
                            foundPath = true;
                            break;
                        }
                    }
                }
            }

            if (foundPath) 
            {
                // We check if we can go straight up to our target card, else we cant get to it.

                if(CheckIfStraightPathToYPos(path, secondCard.y, up > 0, true)) 
                {
                    if(path == secondCard) 
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool CheckIfStraightPathToYPos(Vector2Int fromPos, int desiredYPos, bool upWards, bool setPathY) 
    {
        int currentY = fromPos.y;
        while(currentY < levelMap.GetLength(0) && currentY >= 0) 
        {
            if (currentY == desiredYPos) 
            {
                if (setPathY)
                {
                    path.y = currentY;
                }
                return true;
            }

            if(levelMap[currentY + (upWards ? + 1 : -1), fromPos.x] == false) 
            {
                currentY += (upWards ? +1 : -1);
            }
        }

        return false;
    }

    public bool CheckIfStraightPathToXPos(Vector2Int fromPos, int desiredXPos, bool right, bool setPathX) 
    {
        int currentX = fromPos.x;
        while (currentX < levelMap.GetLength(1) && currentX >= 0)
        {
            if (currentX == desiredXPos)
            {
                if (setPathX)
                {
                    path.y = currentX;
                }
                return true;
            }

            if (levelMap[fromPos.x, currentX + (right ? +1 : -1)] == false)
            {
                currentX += (right ? +1 : -1);
            }
        }

        return false;
    }

}

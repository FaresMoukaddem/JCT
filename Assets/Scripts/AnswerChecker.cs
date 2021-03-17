using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerChecker : MonoBehaviour
{
    // A map defining where in the level cards currently exist.
    bool[,] levelMap;

    // The vector representing the path we are attempting at any moment in the algorithm.
    Vector2Int path;

    // Start is called before the first frame update
    public void Configure(bool[,] levelMap)
    {
        this.levelMap = levelMap;
    }

    public bool CheckAnswer(Vector2Int firstCard, Vector2Int secondCard)
    {
        if (firstCard == secondCard) return false;

        /*
         The algorithms steps (If any of these apply, then we can go to it with a maximum of 2 turns).

         * Check if we can go straight to it (either vertically or horizontally)
         * Check if we can go either left or right and then find a way straight to our targets y position (or staright to our target) then go staright to it horizontally.
         * Check if we can go either up or down and then find a way straight to our targets x position (or staright to our target) then go straight to it vertically.
         */

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

        Debug.Log(firstCard);
        Debug.Log(secondCard);
        
        // Check if we can go straight to it horizontally.
        if(path.y == secondCard.y) 
        {
            if(CheckIfStraightPathToXCard(path, secondCard, path.x < secondCard.x)) 
            {
                return true;
            }
        }

        // Reset path
        path = firstCard;

        // Check if we can go straight to it vertically.
        if(path.x == secondCard.x) 
        {
            if(CheckIfStraightPathToYCard(path, secondCard, path.y < secondCard.y))
            {
                return true;
            }
        }

        // Reset path.
        path = firstCard;

        // Now we check if we can go right or left, to see if we can go straight to the target, or its y position, then go straight to it horizontally.

        // A variable to keep track of steps moved (extra information).
        int stepsMoved = 1;

        Debug.Log("checking right paths");
        // Check all right paths.
        for (int i = path.x + 1; i < levelMap.GetLength(1); i++)
        {
            // If we have moved, we need to check if its an available path or not.
            if (i != path.x)
            {
                // If that path is not available, then stop moving that way.
                if (levelMap[path.y, i] == true) break;
            }

            if(i == secondCard.x) // If we are on the same x pos as our target, check if we can go straight to it on the y axis
            {
                if(CheckIfStraightPathToYCard(new Vector2Int(i, path.y),secondCard, up > 0)) 
                {
                    Debug.Log("straight from one of the right paths to our target" + " steps: " + stepsMoved);
                    return true;
                }
            }
            else if (CheckIfStraightPathToYPos(new Vector2Int(i, path.y), secondCard.y, up > 0, true)) // We check if we can go to our desired y pos.
            {
                Debug.Log("we found a path from the right to our desired y pos" + " steps: " + stepsMoved);
                Debug.Log("current path " + path);

                // Now we check if we can go straight to it.
                if (CheckIfStraightPathToXCard(path, secondCard, i < secondCard.x)) 
                {
                    Debug.Log("And were able to go directly to our goal from there!");
                    return true;
                }
                else 
                {
                    // We reset our y position.
                    path.x = i;
                    path.y = firstCard.y;
                }
            }
            stepsMoved++;
        }

        // Reset path
        path = firstCard;

        // Reset steps moved.
        stepsMoved = 1;

        Debug.Log("checking left paths");

        // Now we do the same but for the left paths.
        for (int i = path.x - 1; i >= 0; i--)
        {
            if (levelMap[path.y, i] == true) break;

            if (i == secondCard.x) // If we are on the same x pos as our target, check if we can go straight to it on the y axis
            {
                if (CheckIfStraightPathToYCard(new Vector2Int(i, path.y), secondCard, up > 0))
                {
                    Debug.Log("straight from one of the left paths to our target" + " steps: " + stepsMoved);
                    return true;
                }
            }
            else if (CheckIfStraightPathToYPos(new Vector2Int(i, path.y), secondCard.y, up > 0, true))
            {
                Debug.Log("we found a path from the left to our desired y pos" + " steps: " + stepsMoved);
                Debug.Log("current path " + path);
                if (CheckIfStraightPathToXCard(path, secondCard, i < secondCard.x))
                {
                    Debug.Log("And were able to go directly to our goal from there!");
                    return true;
                }
                else
                {
                    path.x = i;
                    path.y = firstCard.y;
                }
            }
            stepsMoved++;
        }

        //===============================================================================================================================
        //===============================================================================================================================

        // Now we check if we can move vertically, then either reach our target horizontally, or reach its x position, then reach it vertically.

        // Reset path
        path = firstCard;

        // First we start by going in the 'proper y direction' meaning towards out target.
        Debug.Log("Now we check all possibilites in the proper y direction");

        // Incase up is zero, we can set it to 1, just to have a starting point of which direction to go.
        // Not the best way to do it.
        if (up == 0) up = 1;

        // Now we keep going up from our initial position (as long as were in the array and we dont hit a card).
        while (path.y + up >= 0 && path.y + up < levelMap.GetLength(0) && levelMap[path.y + up, path.x] != true)
        {
            path.y += up;

            // If were on the same y pos, try to go straight to it horizontally.
            if (path.y == secondCard.y)
            {
                if (CheckIfStraightPathToXCard(path, secondCard, path.x < secondCard.x))
                {
                    Debug.Log("YESS NEW FEATURE WORKED!");
                    return true;
                }
            }
            else if (CheckIfStraightPathToXPos(path, secondCard.x, path.x < secondCard.x, true)) // We check if we can go to our desired x pos.
            {
                Debug.Log("Yes we went straight +up from out initial position, and then went straight to the proper x pos");

                // Now we check if we can go straight to it.
                if (CheckIfStraightPathToYCard(path, secondCard, path.y < secondCard.y))
                {
                    Debug.Log("And then we went vertically straight to the card!");
                    return true;
                }
            }
            path.x = firstCard.x;
        }

        // Reset path
        path = firstCard;

        Debug.Log("Now we check all possibilites in the opposite y direction");
        // Now we do the same as the previous loop, but in the opposite y direction.
        while (path.y - up >= 0 && path.y - up < levelMap.GetLength(0) && levelMap[path.y - up, path.x] != true)
        {
            path.y -= up;

            if (path.y == secondCard.y)
            {
                if (CheckIfStraightPathToXCard(path, secondCard, path.x < secondCard.x))
                {
                    Debug.Log("YESS NEW FEATURE WORKED!");
                    return true;
                }
            }
            else if (CheckIfStraightPathToXPos(path, secondCard.x, path.x < secondCard.x, true))
            {
                Debug.Log("Yes we went straight -up from out initial position, and then went straight to the proper x pos");
                if (CheckIfStraightPathToYCard(path, secondCard, path.y < secondCard.y))
                {
                    Debug.Log("And then we went vertically straight to the card!");
                    return true;
                }
            }
            path.x = firstCard.x;
        }

        // If none of those returned true, that means we can't go to it.
        return false;
    }

    // This function checks if we can go straight to our desired y position (if no card is there) from a starting point.
    // And sets our path to that point if we want to.
    public bool CheckIfStraightPathToYPos(Vector2Int fromPos, int desiredYPos, bool upWards, bool setPath) 
    {
        int currentY = fromPos.y;

        int step = upWards ? +1 : -1;

        while (currentY + step < levelMap.GetLength(0) && currentY + step >= 0)
        {
            if (levelMap[currentY + step, fromPos.x] == false)
            {
                currentY += step;
            }
            else
            {
                return false;
            }

            if(currentY == desiredYPos)
            {
                if (setPath)
                    path = new Vector2Int(fromPos.x, currentY);

                return true;
            }
        }

        return false;
    }

    public void UpdateLevelMap(Vector2Int firstCard, Vector2Int secondCard) 
    {
        levelMap[firstCard.y, firstCard.x] = false;
        levelMap[secondCard.y, secondCard.x] = false;
    }

    // This function prints the level map on the screen (used for debugging).
    public void PrintLevelMap()
    {
        for (int i = 0; i < levelMap.GetLength(0); i++)
        {
            string s = "";
            for (int j = 0; j < levelMap.GetLength(1); j++)
            {
                s += levelMap[i, j];
            }
            Debug.Log(s);
        }
    }

    // This function checks if we can go straight to our desired x position (if no card is there) from a starting point.
    // And sets our path to that point if we want to.
    public bool CheckIfStraightPathToXPos(Vector2Int fromPos, int desiredXPos, bool right, bool setPath) 
    {
        int currentX = fromPos.x;

        Debug.Log("Starting from " + fromPos + " right: " + right);

        int step = right ? +1 : -1;

        while (currentX + step < levelMap.GetLength(1) && currentX + step >= 0)
        {
            if (levelMap[fromPos.y, currentX + step] == false)
            {
                currentX += step;
            }
            else 
            {
                return false;
            }

            if(currentX == desiredXPos)
            {
                if (setPath)
                    path = new Vector2Int(currentX, fromPos.y);

                return true;
            }
        }

        return false;
    }

    // This function checks if we can go vertically straight to a card.
    public bool CheckIfStraightPathToYCard(Vector2Int fromPos, Vector2Int targetCardPos, bool upWards)
    {
        int currentY = fromPos.y;

        int step = upWards ? +1 : -1;

        while (currentY + step < levelMap.GetLength(0) && currentY + step >= 0)
        {
            if (levelMap[currentY + step, fromPos.x] == false)
            {
                currentY += step;
            }
            else
            {
                currentY += step;
                if (new Vector2Int(fromPos.x, currentY) == targetCardPos)
                {
                    Debug.Log("straight y " + (upWards ? " up" : " down"));
                    return true;
                }
                else
                    return false;
            }
        }

        return false;
    }

    // This function checks if we can go horizontally straight to a card.
    public bool CheckIfStraightPathToXCard(Vector2Int fromPos, Vector2Int targetCardPos, bool right)
    {
        int currentX = fromPos.x;

        int step = right ? +1 : -1;

        while (currentX + step < levelMap.GetLength(1) && currentX + step >= 0)
        {
            if (levelMap[fromPos.y, currentX + step] == false)
            {
                currentX += step;
            }
            else
            {
                currentX += step;
                if (new Vector2Int(currentX, fromPos.y) == targetCardPos)
                {
                    Debug.Log("straight x " + (right ? " right" : " left"));
                    return true;
                }
                else
                    return false;
            }
        }

        return false;
    }

}

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

        bool foundPath = false;

        Debug.Log(firstCard);
        Debug.Log(secondCard);
        
        if(path.y == secondCard.y) 
        {
            if(CheckIfStraightPathToXCard(path, secondCard, path.x < secondCard.x)) 
            {
                return true;
            }
        }

        path = firstCard;
        if(path.x == secondCard.x) 
        {
            if(CheckIfStraightPathToYCard(path, secondCard, path.y < secondCard.y))
            {
                return true;
            }
        }

        // Reset path.
        path = firstCard;

        // Check all paths from right or left straight upwards
        Debug.Log("up " + up);

        foundPath = false;

        int stepsMoved = 1;

        Debug.Log("checking right paths");
        // Check all right paths.
        for (int i = path.x + 1; i < levelMap.GetLength(1); i++)
        {
            // If we have moved, we need to check if its an available path or not.
            if (i != path.x)
            {
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
            else if (CheckIfStraightPathToYPos(new Vector2Int(i, path.y), secondCard.y, up > 0, true))
            {
                Debug.Log("we found a path from the right to our desired y pos" + " steps: " + stepsMoved);
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

        // Reset path
        path = firstCard;

        stepsMoved = 1;
        Debug.Log("checking left paths");
        // Check left paths.
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

        Debug.Log("Now we check all possibilites in the proper y direction");

        if (up == 0) up = 1;

        // Now how much up we can go from our initial position
        while (path.y + up >= 0 && path.y + up < levelMap.GetLength(0) && levelMap[path.y + up, path.x] != true)
        {
            path.y += up;

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
                Debug.Log("Yes we went straight up from out initial position, and then went straight to the proper x pos");
                if (CheckIfStraightPathToYCard(path, secondCard, path.y < secondCard.y))
                {
                    Debug.Log("And then we went vertically straight to the card!");
                    return true;
                }
            }
            path.x = firstCard.x;
        }

        path = firstCard;

        Debug.Log("Now we check all possibilites in the opposite y direction");
        // Now how much up we can go from our initial position
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
                Debug.Log("Yes we went straight up from out initial position, and then went straight to the proper x pos");
                if (CheckIfStraightPathToYCard(path, secondCard, path.y < secondCard.y))
                {
                    Debug.Log("And then we went vertically straight to the card!");
                    return true;
                }
            }
            path.x = firstCard.x;
        }

        return false;
    }

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
        Debug.Log("MAP BEFORE");
        for(int i = 0; i < levelMap.GetLength(0); i++) 
        {
            string s = "";
            for(int j = 0; j < levelMap.GetLength(1); j++)
            {
                s += levelMap[i, j];
            }
            Debug.Log(s);
        }

        levelMap[firstCard.y, firstCard.x] = false;
        levelMap[secondCard.y, secondCard.x] = false;

        Debug.Log("MAP AFTER");
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

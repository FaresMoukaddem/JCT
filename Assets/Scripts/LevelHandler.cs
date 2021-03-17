using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class LevelHandler : MonoBehaviour
{
    public TextAsset[] textAssets;

    public int numberOfLevels;

    public Transform buttonsParent;

    public RectTransform panel;

    public List<int> numbersList;

    public bool isDrawingLevel;

    public Sprite[] images;

    public List<CardHandler> activeCards;

    private int levelWidth, levelHeight, buttonsUsed, buttonCount, currentLevel;

    public string[] ReadFile(int index)
    {
        string[] lines = textAssets[index].text.Split('\n');
        
        for(int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].Trim();
        }

        return lines;
    }

    public void Awake()
    {
        numberOfLevels = textAssets.Length;
    }

    public int GetButtonCount(string[] lines) 
    {
        int buttonCount = 0;

        foreach(string s in lines) 
        {
            buttonCount += s.Count(x => x == 'X');
        }

        return buttonCount;
    }

    public int GetNumberOfDifferentCards(int buttonCount) 
    {
        for(int i = 20; i > 1; i--) 
        {
            if (i < buttonCount && buttonCount % i == 0 && (buttonCount/i) % 2 == 0)
            {
                Debug.Log("Number of different cards " + i + " and iterations are " + (buttonCount/i).ToString());
                return i;
            }
        }

        Debug.LogError("FIX LEVEL");

        return 0;
    }

    // Start is called before the first frame update
    public void DrawLevel(int index)
    {
        Debug.Log("Drawing Level " + index);

        // This variable is to not allow the level to be restarted while animating.
        isDrawingLevel = true;

        // Read the files and set the width and hieght.
        string[] lines = ReadFile(index);
        levelHeight = lines.Length;
        levelWidth = lines[0].Length;

        Debug.Log(levelWidth);
        Debug.Log(levelHeight);

        buttonsUsed = 0;
        buttonCount = GetButtonCount(lines);
        Debug.Log("button count " + buttonCount);

        bool[,] levelMap = new bool[levelHeight, levelWidth];

        // Generate the number list we will be using to choose the cards.
        //===============================================================
        numbersList = new List<int>();

        int numberOfDifferentCards = GetNumberOfDifferentCards(buttonCount);

        for(int i = 0; i < buttonCount/numberOfDifferentCards; i++) 
        {
            for(int j = 0; j < numberOfDifferentCards; j++) 
            {
                numbersList.Add(j);
                Debug.Log(j);
            }
        }
        //===============================================================

        // A list containing all of the active cards.
        activeCards = new List<CardHandler>();

        // To leave the first quarter of the screen open.
        int screenH = Screen.height - (Screen.height/4);

        // Temp variables to use in the loop.
        RectTransform t;
        CardHandler card;
        int number;

        // We loop through each card to activate and configure them.
        for (int i = 0; i < levelHeight; i++)
        {
            for(int j = 0; j < levelWidth; j++) 
            {
                if(lines[i][j] == 'X') 
                {
                    // Set the values in the level map (that will be used by the algorithm)
                    levelMap[i,j] = true;

                    // Get references for the card handler and the rect transform.
                    t = buttonsParent.GetChild(buttonsUsed).GetComponent<RectTransform>();
                    card = t.GetComponent<CardHandler>();

                    // Add the card to the active cards list.
                    activeCards.Add(card);

                    // Increment buttons used. 
                    buttonsUsed++;

                    // Set the carda as active.
                    t.gameObject.SetActive(true);

                    // Get a random number for this card from the number list.
                    number = GetRandomButtonNumber();

                    // Configure the card/
                    card.Configure(number, images[number] ,j, i, i * 0.1f);

                    // If this is the last card, start the coroutine for the is animating flag
                    // This is to stop the user for restarting the level as its animating in.
                    if(buttonsUsed == buttonCount) 
                    {
                        StartCoroutine(WaitAndTurnOffIsDrawing(2.0f));
                    }

                    // Place the card in the correct poisition.
                    t.position = new Vector2((Screen.width / 5 + 1) * 1, screenH * 0.5f);
                    t.position = new Vector2((Screen.width / (levelWidth + 1)) * (j+1), ((screenH / levelHeight) * (levelHeight - i)));
                }
                else
                {
                    // Set the values in the level map (that will be used by the algorithm)
                    levelMap[i, j] = false;
                }
            }
        }

        currentLevel = index;

        // Configure the round manager.
        RoundManager.instance.Configure(index, buttonCount, levelMap);

        Debug.Log("buttons used " + buttonsUsed);
    }

    public void RemoveCardsFromActiveList(CardHandler firstCard, CardHandler secondCard)
    {
        activeCards.Remove(firstCard);
        activeCards.Remove(secondCard);
    }

    public IEnumerator WaitAndTurnOffIsDrawing(float time) 
    {
        yield return new WaitForSeconds(time);
        isDrawingLevel = false;
    }

    public void RestartLevel() 
    {
        if (isDrawingLevel) 
        {
            Debug.Log("DRAWING LEVEL WAIT!");
            return;
        }

        Debug.Log("Restarting level " + currentLevel + " with bc " + buttonCount);
        
        foreach(CardHandler card in activeCards)
        {
            card.Reset();
        }

        RoundManager.instance.StartLevel(currentLevel);
    }

    public bool currentLevelIsValid(AnswerChecker answerChecker)
    {
        for(int i = 0; i < activeCards.Count; i++)
        {
            for(int j = 0; j < activeCards.Count; j++)
            {
                if (j == i) continue;

                if (activeCards[i].number == activeCards[j].number)
                {
                    if (answerChecker.CheckAnswer(activeCards[i].arrayPos, activeCards[j].arrayPos))
                    {
                        Debug.Log("<color=green>Current level is valid!</color>");
                        return true;
                    }
                }
            }
        }

        Debug.Log("<color=red>Current level is invalid!</color>");

        return false;
    }

    int GetRandomButtonNumber() 
    {
        int rnd = Random.Range(0, numbersList.Count);
        int number = numbersList[rnd];
        numbersList.RemoveAt(rnd);
        return number;
    }
}

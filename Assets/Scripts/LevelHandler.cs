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

    private int levelWidth, levelHeight, buttonsUsed, buttonCount, currentLevel;

    public Transform buttonsParent;

    public RectTransform panel;

    public List<int> numbersList;

    public bool isDrawingLevel;

    public AnswerChecker answerChecker;

    public Sprite[] images;

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

        isDrawingLevel = true;

        string[] lines = ReadFile(index);
        levelHeight = lines.Length;
        levelWidth = lines[0].Length;

        Debug.Log(levelWidth);
        Debug.Log(levelHeight);

        buttonsUsed = 0;
        buttonCount = GetButtonCount(lines);
        Debug.Log("button count " + buttonCount);

        bool[,] levelMap = new bool[levelHeight, levelWidth];

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

        int screenH = Screen.height - (Screen.height/4);

        RectTransform t;
        int number;
        for (int i = 0; i < levelHeight; i++)
        {
            for(int j = 0; j < levelWidth; j++) 
            {
                if(lines[i][j] == 'X') 
                {
                    levelMap[i,j] = true;
                    t = buttonsParent.GetChild(buttonsUsed).GetComponent<RectTransform>();
                    buttonsUsed++;
                    t.gameObject.SetActive(true);
                    number = GetRandomButtonNumber();
                    t.GetComponent<CardHandler>().Configure(number, images[number] ,j, i, i * 0.1f);

                    if(buttonsUsed == buttonCount) 
                    {
                        StartCoroutine(WaitAndTurnOffIsDrawing(2.0f));
                    }

                    t.position = new Vector2((Screen.width / 5 + 1) * 1, screenH * 0.5f);
                    t.position = new Vector2((Screen.width / (levelWidth + 1)) * (j+1), ((screenH / levelHeight) * (levelHeight - i)) );
                }
                else
                {
                    levelMap[i, j] = false;
                }
            }
        }

        currentLevel = index;

        RoundManager.instance.Configure(index, buttonCount);
        answerChecker.Configure(levelMap);

        Debug.Log("buttons used " + buttonsUsed);
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
        
        for(int i = 0; i < buttonCount; i++) 
        {
            buttonsParent.GetChild(i).GetComponent<CardHandler>().Reset();
        }

        RoundManager.instance.StartLevel(currentLevel);
    }

    int GetRandomButtonNumber() 
    {
        int rnd = Random.Range(0, numbersList.Count);
        int number = numbersList[rnd];
        numbersList.RemoveAt(rnd);
        return number;
    }
}

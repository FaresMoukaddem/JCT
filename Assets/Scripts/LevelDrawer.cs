using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class LevelDrawer : MonoBehaviour
{
    public TextAsset[] textAssets;

    private int levelWidth, levelHeight, buttonsUsed;

    public Transform buttonsParent;

    public RectTransform panel;

    private int buttonCount;

    public List<int> numbersList;

    public AnswerChecker answerChecker;

    public Sprite[] images;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ReadFile(0);
        }
    }

    public string[] ReadFile(int index)
    {
        string[] lines = textAssets[index].text.Split('\n');
        
        for(int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].Trim();
        }

        return lines;
    }

    public void Start()
    {
        DrawLevel(0);
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
        for(int i = 10; i > 1; i--) 
        {
            if (i < buttonCount && buttonCount % i == 0)
            {
                Debug.Log("Number of different cards " + i);
                return i;
            }
        }

        Debug.LogError("FIX LEVEL");

        return 0;
    }

    // Start is called before the first frame update
    void DrawLevel(int index)
    {
        string[] lines = ReadFile(index);
        levelHeight = lines.Length;
        levelWidth = lines[0].Length;

        Debug.Log(levelWidth);
        Debug.Log(levelHeight);

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
                    t.GetComponent<CardHandler>().Configure(number, images[number] ,j, i);
                    t.position = new Vector2((Screen.width / 5 + 1) * 1, screenH * 0.5f);
                    t.position = new Vector2((Screen.width / (levelWidth + 1)) * (j+1), ((screenH / levelHeight) * (levelHeight - i)) );
                }
                else
                {
                    levelMap[i, j] = false;
                }
            }
        }

        RoundManager.instance.Configure(index, buttonCount);
        answerChecker.Configure(levelMap);

        Debug.Log("buttons used " + buttonsUsed);
    }

    int GetRandomButtonNumber() 
    {
        int rnd = Random.Range(0, numbersList.Count);
        int number = numbersList[rnd];
        numbersList.RemoveAt(rnd);
        return number;
    }
}

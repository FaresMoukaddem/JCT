using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class LevelDrawer : MonoBehaviour
{
    public TextAsset[] textAssets;

    public Button testObject;

    private int levelWidth, levelHeight, buttonsUsed;

    public Transform buttonsParent;

    private int buttonCount;

    public List<int> numbersList;

    public AnswerChecker answerChecker;

    public string[] levels;

    public void Start()
    {
        levels = new string[2];
        levels[0] = "X0000000X00-0X000000X00-00X00000X00-000X0000X00-0000X000X00-00000X00X00-000000X0X00-0000000XXXX-XXXXXXXXXXX-0000000XXXX-0000000XXX0";
        levels[1] = "00000X00000-0000XXX0000-000XXXXX000-00XXXXXXX00-0XXXXXXXXX0-XXXXXXXXXXX-000XXXXX000-000XXXXX000-000XXXXX000-000XXXXX000-000XXXXX000-000XXXXX000";

        DrawLevel(0);
    }

    // Start is called before the first frame update
    void DrawLevel(int index)
    {
        string[] lines = levels[index].Split('-');
        levelHeight = lines.Length;
        levelWidth = lines[0].Length;

        Debug.Log(levelWidth);
        Debug.Log(levelHeight);

        buttonCount = levels[index].Count(x => x == 'X');
        Debug.Log("button count " + buttonCount);

        RectTransform t;

        bool[,] levelMap = new bool[levelHeight, levelWidth];

        //===============================================================
        numbersList = new List<int>();
        
        for(int i = 0; i < buttonCount/2; i++) 
        {
            numbersList.Add(i+1);
        }

        for (int i = buttonCount / 2; i < buttonCount; i++)
        {
            numbersList.Add(numbersList[i-(buttonCount / 2)]);
        }
        //===============================================================

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
                    t.GetComponent<CardHandler>().Configure(GetRandomButtonNumber(), j + 1, i + 1);
                    t.position = new Vector2((Screen.width / 5 + 1) * 1, Screen.height * 0.5f);
                    t.position = new Vector2((Screen.width / (levelWidth + 1)) * (j+1), ((Screen.height / levelHeight) * (levelHeight - i)) - 100);
                }
                else
                {
                    levelMap[i, j] = false;
                }
            }
        }

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

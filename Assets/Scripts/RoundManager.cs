using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    public CardHandler currentlySelectedCard;

    public AnswerChecker answerChecker;

    public int currentLevel, currentScore, numberOfCards;

    public RoundUIHandler roundUI;
    public LevelHandler levelHandler;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentlySelectedCard = null;
    }

    public void Configure(int currentLevel, int numberOfCards) 
    {
        this.currentLevel = currentLevel;
        this.numberOfCards = numberOfCards;
        currentScore = 0;
        roundUI.UpdateScore(currentScore);
        roundUI.UpdateLevel(currentLevel + 1);
    }

    public void StartLevel(int index)
    {
        levelHandler.DrawLevel(index);
        SoundHandler.instance.PlaySoundEffect(2);
    }

    // Update is called once per frame
    public void ButtonPressed(CardHandler selectedCard)
    {
        if (currentlySelectedCard == null)
        {
            Debug.Log("New card");
            currentlySelectedCard = selectedCard;
            currentlySelectedCard.ToggleHighlight(true);
            SoundHandler.instance.PlaySoundEffect(0);
        }
        else if (currentlySelectedCard == selectedCard)
        {
            Debug.Log("Chose same card");
            currentlySelectedCard.ToggleHighlight(false);

            SoundHandler.instance.PlaySoundEffect(0);

            currentlySelectedCard = null;
        }
        else
        {
            currentlySelectedCard.ToggleHighlight(false);

            //if (currentlySelectedCard.number == selectedCard.number)
            //{
                bool answer = answerChecker.CheckAnswer(currentlySelectedCard.arrayPos, selectedCard.arrayPos);

                Debug.Log(answer);

                if (answer)
                {
                    answerChecker.UpdateLevelMap(currentlySelectedCard.arrayPos, selectedCard.arrayPos);
                
                    Debug.Log("Cards left: " + numberOfCards);
                    currentlySelectedCard.Dissappear();
                    selectedCard.Dissappear();

                    currentScore += 15;
                    roundUI.UpdateScore(currentScore);

                    numberOfCards -= 2;
                    SoundHandler.instance.PlaySoundEffect(1);

                    if (numberOfCards == 0) 
                    {
                        SoundHandler.instance.PlaySoundEffect(3);

                        PlayerPrefs.SetInt("FinishedLevel" + currentLevel.ToString(), 1);

                        if (currentLevel + 1 > levelHandler.numberOfLevels - 1)
                        {
                            ShowNoMoreLevelsPopup();   
                        }
                        else
                        {
                            ShowNextLevelPopup();
                        }
                    }
                }
                else
                {
                    SoundHandler.instance.PlaySoundEffect(4);
                    currentScore -= 10;
                    if (currentScore < 0) currentScore = 0;
                    roundUI.UpdateScore(currentScore);
                }
            //}

            currentlySelectedCard = null;
        }
    }

    public void GoToMenu()
    {
        if (SoundHandler.instance)
        {
            SoundHandler.instance.StopMusic();
        }

        GameManager.instance.LoadMenuScene();
    }

    public void ShowNextLevelPopup() 
    {
        roundUI.ShowPopup("You beat the level with a score of " + currentScore + "!", "Next", "Menu", () => StartLevel(currentLevel + 1), () => GoToMenu());
    }

    public void ShowRestartLevelPopup()
    {
        roundUI.ShowPopup("You lost this round :(", "Restart", "Menu", () => StartLevel(currentLevel), () => GoToMenu());
    }

    public void ShowNoMoreLevelsPopup()
    {
        roundUI.ShowPopup("You finished the game!", "Restart", "Menu", () => StartLevel(0), () => GoToMenu());
    }
}

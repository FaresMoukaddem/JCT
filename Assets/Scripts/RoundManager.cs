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

    public void Configure(int currentLevel, int numberOfCards, bool[,] levelMap) 
    {
        this.currentLevel = currentLevel;
        this.numberOfCards = numberOfCards;
        currentScore = 0;
        roundUI.UpdateScore(currentScore);
        roundUI.UpdateLevel(currentLevel + 1);

        answerChecker.Configure(levelMap);
    }

    public void StartLevel(int index)
    {
        levelHandler.DrawLevel(index);

        if (SoundHandler.instance)
        {
            SoundHandler.instance.PlaySoundEffect(2);
        }
    }

    // This function will run when a card is pressed.
    public void ButtonPressed(CardHandler selectedCard)
    {
        // If we dont have a currently selected card, select this one.
        if (currentlySelectedCard == null)
        {
            Debug.Log("New card");
            currentlySelectedCard = selectedCard;
            currentlySelectedCard.ToggleHighlight(true);
            SoundHandler.instance.PlaySoundEffect(0);
        }
        else if (currentlySelectedCard == selectedCard) // If we selected the same card, deselect it.
        {
            Debug.Log("Chose same card");
            currentlySelectedCard.ToggleHighlight(false);

            SoundHandler.instance.PlaySoundEffect(0);

            currentlySelectedCard = null;
        }
        else // Check if these two cards are identical, if they try to see if they can reach each other.
        {

            // Turn off the currently selected card.
            currentlySelectedCard.ToggleHighlight(false);

            // If they are identical.
            if (true /*currentlySelectedCard.number == selectedCard.number*/)
            {
                // Check if they can reach each other (using the algorithm).
                bool answer = answerChecker.CheckAnswer(currentlySelectedCard.arrayPos, selectedCard.arrayPos);

                Debug.Log(answer);

                // If they can.
                if (answer)
                {
                    // Update the level map.
                    answerChecker.UpdateLevelMap(currentlySelectedCard.arrayPos, selectedCard.arrayPos);
                
                    Debug.Log("Cards left: " + numberOfCards);

                    // Update score.
                    currentScore += 15;
                    roundUI.UpdateScore(currentScore);

                    // Remove both cards from the game.
                    currentlySelectedCard.Dissappear();
                    selectedCard.Dissappear();
                    numberOfCards -= 2;
                    levelHandler.RemoveCardsFromActiveList(currentlySelectedCard, selectedCard);

                    SoundHandler.instance.PlaySoundEffect(1);

                    // If the game is done.
                    if (numberOfCards == 0) 
                    {
                        SoundHandler.instance.PlaySoundEffect(3);

                        // Save that we finished this level.
                        PlayerPrefs.SetInt("FinishedLevel" + currentLevel.ToString(), 1);

                        // Check if there are more levels.
                        if (currentLevel + 1 > levelHandler.numberOfLevels - 1)
                        {
                            ShowNoMoreLevelsPopup();   
                        }
                        else
                        {
                            ShowNextLevelPopup();
                        }
                    }
                    else if (levelHandler.currentLevelIsValid(answerChecker) == false) // Check if this level is still playable.
                    {
                        ShowRestartLevelPopup();
                    }
                }
                else // If these cards cant be reached.
                {
                    SoundHandler.instance.PlaySoundEffect(4);
                    currentScore -= 10;
                    if (currentScore < 0) currentScore = 0;
                    roundUI.UpdateScore(currentScore);
                }
            }
            else
            {
                SoundHandler.instance.PlaySoundEffect(4);
            }

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
        roundUI.ShowPopup("You lost this round :(", "Restart", "Menu", () => levelHandler.RestartLevel(), () => GoToMenu());
    }

    public void ShowNoMoreLevelsPopup()
    {
        roundUI.ShowPopup("You finished the game!", "Restart", "Menu", () => StartLevel(0), () => GoToMenu());
    }
}

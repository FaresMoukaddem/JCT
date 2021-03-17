using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundUIHandler : MonoBehaviour
{

    public Text scoreText, levelText;

    public PopupHandler popup;

    public void UpdateLevel(int newLevel)
    {
        levelText.text = "Level: " + newLevel.ToString();
    }

    public void UpdateScore(int newScore)
    {
        scoreText.text = "Score: " + newScore.ToString();
    }

    public void ShowPopup(string popupText, string firstButtonText, string secondButtonText, System.Action firstButtonAction, System.Action secondButtonAction) 
    {
        popup.PopupIn(popupText, firstButtonText, secondButtonText, firstButtonAction, secondButtonAction);
    }
}

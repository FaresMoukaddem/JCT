using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    public CardHandler currentlySelectedCard;

    public AnswerChecker answerChecker;

    public int currentLevel, numberOfCards;

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
    }

    // Update is called once per frame
    public void ButtonPressed(CardHandler selectedCard)
    {
        if(currentlySelectedCard == null)
        {
            Debug.Log("New card");
            currentlySelectedCard = selectedCard;
            currentlySelectedCard.ToggleHighlight(true);
        }
        else if(currentlySelectedCard == selectedCard)
        {
            Debug.Log("Chose same card");
            currentlySelectedCard.ToggleHighlight(false);
            

            Debug.Log(answerChecker.CheckIfStraightPathToXPos
                (currentlySelectedCard.arrayPos,
                5,
                false, false));

            currentlySelectedCard = null;
        }
        else
        {
            currentlySelectedCard.ToggleHighlight(false);

            //if (currentlySelectedCard.number == selectedCard.number)
            // {
            bool answer = answerChecker.CheckAnswer(currentlySelectedCard.arrayPos, selectedCard.arrayPos);

            Debug.Log(answer);
            
            if (answer)
            {
                answerChecker.UpdateLevelMap(currentlySelectedCard.arrayPos, selectedCard.arrayPos);
                numberOfCards -= 2;
                Debug.Log("Cards left: " + numberOfCards);
                currentlySelectedCard.Dissappear();
                selectedCard.Dissappear();
            }
            // }

            currentlySelectedCard = null;
        }
    }
}

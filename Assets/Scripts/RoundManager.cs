using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    public CardHandler currentlySelectedCard;

    public AnswerChecker answerChecker;

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
            currentlySelectedCard = null;
        }
        else
        {
            currentlySelectedCard.ToggleHighlight(false);

            if(currentlySelectedCard.number == selectedCard.number) 
            {
                Debug.Log("Checking answer");
               // answerChecker.CheckAnswer(new Vector2Int(currentlySelectedCard.x, currentlySelectedCard.y), new Vector2Int(selectedCard.x, selectedCard.y));
            }
            else
            {
                Debug.Log("Cards dont match");
            }

            currentlySelectedCard = null;
        }
    }
}

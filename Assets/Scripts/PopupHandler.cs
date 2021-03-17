using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupHandler : MonoBehaviour
{
    public Animator anim;

    public Text text;

    public Button buttonOne, buttonTwo;

    private Text buttonOneText, buttonTwoText;

    public void Awake()
    {
        buttonOneText = buttonOne.GetComponentInChildren<Text>();
        buttonTwoText = buttonTwo.GetComponentInChildren<Text>();
    }

    public void PopupIn(string popupText, string firstButtonText, string secondButtonText, System.Action firstButtonAction, System.Action secondButtonAction) 
    {
        buttonOne.onClick.RemoveAllListeners();
        buttonTwo.onClick.RemoveAllListeners();

        buttonOneText.text = firstButtonText;
        buttonTwoText.text = secondButtonText;

        text.text = popupText;

        buttonOne.onClick.AddListener(() =>
        {
            firstButtonAction();
            PopupOut();
            ToggleButtonsActive(false);
        });

        buttonTwo.onClick.AddListener(() =>
        {
            secondButtonAction();
            PopupOut();
            ToggleButtonsActive(false);
        });

        ToggleButtonsActive(true);

        anim.SetTrigger("In");
    }

    public void ToggleButtonsActive(bool on)
    {
        buttonOne.interactable = on;
        buttonTwo.interactable = on;
    }

    public void PopupOut()
    {
        anim.SetTrigger("Out");
    }
}

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
        // Remove all buttons listeners.
        buttonOne.onClick.RemoveAllListeners();
        buttonTwo.onClick.RemoveAllListeners();

        // Set button texts.
        buttonOneText.text = firstButtonText;
        buttonTwoText.text = secondButtonText;

        // Set popup text.
        text.text = popupText;

        // Add button one on click function.
        buttonOne.onClick.AddListener(() =>
        {
            // Invoke action.
            firstButtonAction();

            // Animate popup out.
            PopupOut();

            // We deactivate the buttons, so they don't get pressed twice.
            ToggleButtonsActive(false);
        });

        // Add button one on click function.
        buttonTwo.onClick.AddListener(() =>
        {
            secondButtonAction();
            PopupOut();
            ToggleButtonsActive(false);
        });

        // Set the buttons as active.
        ToggleButtonsActive(true);

        // Animate the popup in.
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

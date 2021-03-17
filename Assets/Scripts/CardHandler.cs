using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    public Text text;
    public Button button;
    public int number;
    public Image buttonImage, image;

    public Vector2Int arrayPos;

    public bool active;

    public bool isAnimating, isDisappearing;
    public float targetScale, currentScale;
    private RectTransform buttonRectTransform;

    public void Awake()
    {
        buttonRectTransform = button.GetComponent<RectTransform>();
        currentScale = buttonRectTransform.localScale.magnitude;

        button.onClick.AddListener(ButtonPressed);
    }

    // We animate the card using its update function.
    public void Update()
    {
        if (isAnimating) 
        {
            // If its growing, increase its size and clamp its max value.
            if (currentScale < targetScale)
            {
                currentScale += 0.1f;

                currentScale = Mathf.Clamp(currentScale, 0, targetScale);
            }
            else if(currentScale > targetScale) // If its shrinking, decrease its size and clamp its min value.
            {
                currentScale = Mathf.Clamp(currentScale, targetScale, 1.5f);
                currentScale -= 0.1f;
            }
            else // This means we have stopped animating.
            {
                // If our animation was to disappear, then reset the card.
                if (isDisappearing) 
                {
                    isDisappearing = false;
                    Reset();
                }

                isAnimating = false;
            }

            // Set the scale of the card.
            buttonRectTransform.localScale = new Vector3(currentScale, currentScale, currentScale);
        }
    }

    // Card on click function.
    private void ButtonPressed()
    {
        RoundManager.instance.ButtonPressed(this);
    }

    // Configure the card at the start of each round (if its going to be used).
    public void Configure(int number, Sprite sprite, int x, int y, float timeToAnimate = 0)
    {
        image.sprite = sprite;
        this.number = number;
        arrayPos.x = x;
        arrayPos.y = y;

        active = true;
        
        // Start animating in after a certain time.
        // To get the effect of the cards animating after each other.
        StartCoroutine(AnimateAfter(timeToAnimate));
    }

    // Reset the card to its initial state.
    public void Reset()
    {
        buttonRectTransform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void PlayInitialAnimation(float timeToAnimate = 0)
    {
        buttonRectTransform.localScale = Vector3.zero;
        currentScale = 0;
        targetScale = 1;
        isAnimating = true;
    }

    public IEnumerator AnimateAfter(float time)
    {
        yield return new WaitForSeconds(time);
        PlayInitialAnimation();
    }

    public void ToggleGrowAnimation(bool grow) 
    {
        buttonRectTransform.localScale = grow ? Vector3.one : new Vector3(1.5f, 1.5f, 1.5f);
        currentScale = grow ? 1.0f : 1.5f;
        targetScale = grow ? 1.5f : 1.0f;
        isAnimating = true;
    }

    public void ToggleHighlight(bool on)
    {
        ToggleGrowAnimation(on);
        buttonImage.color = on ? Color.green: Color.white;
    }

    public void Dissappear()
    {
        targetScale = 0.0f;
        isAnimating = true;
        isDisappearing = true;
        active = false;
    }
}

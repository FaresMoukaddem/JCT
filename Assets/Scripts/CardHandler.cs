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

    public void Update()
    {
        if (isAnimating) 
        {
            if (currentScale < targetScale)
            {
                currentScale += 0.1f;

                currentScale = Mathf.Clamp(currentScale, 0, targetScale);
            }
            else if(currentScale > targetScale) 
            {
                currentScale = Mathf.Clamp(currentScale, targetScale, 1.5f);
                currentScale -= 0.1f;
            }
            else
            {
                if (isDisappearing) 
                {
                    isDisappearing = false;
                    Reset();
                }

                isAnimating = false;
            }

            

            buttonRectTransform.localScale = new Vector3(currentScale, currentScale, currentScale);
        }
    }

    private void ButtonPressed()
    {
        RoundManager.instance.ButtonPressed(this);
    }

    // Start is called before the first frame update
    public void Configure(int number, Sprite sprite, int x, int y, float timeToAnimate = 0)
    {
        image.sprite = sprite;
        this.number = number;
        arrayPos.x = x;
        arrayPos.y = y;

        active = true;

        StartCoroutine(AnimateAfter(timeToAnimate));
    }

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

    // Update is called once per frame
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

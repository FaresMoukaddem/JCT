using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    public Text text;
    public Button button;
    public int number;
    public Image image;

    public int x, y;

    public void Start()
    {
        ToggleHighlight(false);

        button.onClick.AddListener(ButtonPressed);
    }

    private void ButtonPressed()
    {
        RoundManager.instance.ButtonPressed(this);
    }

    // Start is called before the first frame update
    public void Configure(int number, int x, int y)
    {
        text.text = number.ToString();
        this.number = number;
        this.x = x;
        this.y = y;
    }

    // Update is called once per frame
    public void ToggleHighlight(bool on)
    {
        image.color = on ? Color.green: Color.white;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonHandler : MonoBehaviour
{
    public int levelIndex;

    public GameObject star;

    public Button button;

    public void Start()
    {
        star.SetActive(PlayerPrefs.GetInt("FinishedLevel" + levelIndex.ToString(), 0) == 1);

        button.onClick.AddListener(OnPressed);
    }

    public void OnDestroy()
    {
        button.onClick.RemoveListener(OnPressed);
    }

    public void OnPressed()
    {
        GameManager.instance.LoadGameWithLevel(levelIndex);
    }
}

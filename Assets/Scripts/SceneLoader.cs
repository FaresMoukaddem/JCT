using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneLoader : MonoBehaviour
{
    public Animator anim;

    public UnityEvent OnGameSceneLoaded;

    public UnityEvent OnMenuSceneLoaded;

    // Start is called before the first frame update
    public void LoadScene(string sceneName)
    {
        // We animate the loader in.
        anim.SetTrigger("In");

        // We wait a bit before actually loading the scene (about the time of the animation).
        StartCoroutine(WaitAndDo(0.5f, () => SceneManager.LoadScene(sceneName)));
    }

    public IEnumerator WaitAndDo(float time, System.Action callback)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }

    void OnLevelWasLoaded()
    {
        // We animate the loader out.
        anim.SetTrigger("Out");

        // We wait a bit before invoking the scene loaded events (about the time of the animation).
        if (SceneManager.GetActiveScene().name == "Game")
        {
            StartCoroutine(WaitAndDo(0.5f, () => OnGameSceneLoaded?.Invoke()));
        }
        else if (SceneManager.GetActiveScene().name == "Menu")
        {
            StartCoroutine(WaitAndDo(0.5f, () => OnMenuSceneLoaded?.Invoke()));
        }
    }
}

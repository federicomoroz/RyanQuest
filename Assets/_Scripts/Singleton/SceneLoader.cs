using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] public        Image       blackImage;
                     private       Coroutine   fadeRoutine = null;
    [SerializeField] private       float       _fadeLength = 1f;
    private void Start()
    {
        fadeRoutine = StartCoroutine(FadeInRoutine(_fadeLength));
    }
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
        fadeRoutine = StartCoroutine(FadeInRoutine(_fadeLength));

    }

    public void LoadSceneWithFade(string name)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeOutRoutine(name, _fadeLength));        
    }

    private IEnumerator FadeInRoutine(float time)
    {
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            blackImage.color = new Color(0, 0, 0, 1 - t);
            yield return null;
        }
        blackImage.gameObject.SetActive(false);
        fadeRoutine = null;
    }

    private IEnumerator FadeOutRoutine(string name, float time)
    {
        blackImage.gameObject.SetActive(true);

        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            blackImage.color = new Color(0, 0, 0, t);
            yield return null;
        }
        fadeRoutine = null;
        LoadScene(name);
    }
}



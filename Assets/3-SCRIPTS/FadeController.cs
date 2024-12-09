using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Out (1 to 0)
/// In  (0 to 1)
/// </summary>
public class FadeController : MonoBehaviour
{
    CanvasGroup canvasGroup;

    [SerializeField]
    float time = 1f;

    [SerializeField]
    float quickTime = 0.1f;

    [SerializeField]
    bool fadeOutAtStart = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (fadeOutAtStart)
        {
            FadeOut();
        }
    }

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine(time));
    }

    public void QuickFadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine(quickTime));
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine(time));
    }

    public void QuickFadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine(quickTime));
    }

    IEnumerator FadeOutCoroutine(float time)
    {
        Debug.Log("Fading out... ");

        float tracker = 1;

        while(tracker > 0)
        {
            tracker -= Time.deltaTime / time;
            canvasGroup.alpha = tracker;
            yield return 0;
        }
    }

    IEnumerator FadeInCoroutine(float time)
    {
        Debug.Log("Fading in... ");

        float tracker = 0;

        while (tracker < 1)
        {
            tracker += Time.deltaTime / time;
            canvasGroup.alpha = tracker;
            yield return 0;
        }
    }
}

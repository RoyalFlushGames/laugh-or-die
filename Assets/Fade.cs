using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float velocity;

    private bool faded;
    private bool fading;
    
    public static Fade Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void AutoFade()
    {
        StartCoroutine(faded ? WaitUntilFade(FadeOut) : WaitUntilFade(FadeIn));
    }
    
    private IEnumerator WaitUntilFade(UnityAction action)
    {
        yield return new WaitUntil(() => !fading);
        action.Invoke();
    }
    
    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
        faded = false;
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
        faded = true;
    }

    private IEnumerator FadeOutCoroutine()
    {
        fading = true;
        for (float i = 1; i > 0; i -= Time.deltaTime * velocity)
        {
            image.color = new Color(0, 0, 0, i);
            yield return null;
        }

        fading = false;
    }
    
    private IEnumerator FadeInCoroutine()
    {
        fading = true;
        for (float i = 0; i < 1; i += Time.deltaTime * velocity)
        {
            image.color = new Color(0, 0, 0, i);
            yield return null;
        }
        fading = false;
    }

    private void Update()
    {
        if (!Keyboard.current.aKey.wasPressedThisFrame) return;
        if (!faded)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }
    }
}

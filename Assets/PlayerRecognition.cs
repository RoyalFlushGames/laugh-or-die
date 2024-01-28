using System;
using TMPro;
using UnityEngine;

public class PlayerRecognition : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private void Start()
    {
        gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }
    
    public void Show(bool value)
    {
        text.gameObject.SetActive(value);
    }

    public void SetText(string value)
    {
        text.text = value;
    }
}

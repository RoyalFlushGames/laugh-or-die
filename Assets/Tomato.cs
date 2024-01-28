using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : MonoBehaviour
{
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject exploted;

    private void Start()
    {
        main.gameObject.SetActive(false);
        exploted.gameObject.SetActive(false);
    }

    public void Main(bool value)
    {
        main.SetActive(value);
    }
    
    public void Exploded(bool value)
    {
        exploted.SetActive(value);
    }
}

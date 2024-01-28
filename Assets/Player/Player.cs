using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject mainSprite;
    [SerializeField] private GameObject headSprite;
    [SerializeField] private GameObject showBegging;
    [SerializeField] private GameObject tomato;
    [SerializeField] private Hearths hearths;
    [SerializeField] private TMP_Text text;

    public Gamepad PlayerGamepad { get; set; }

    private bool _tomato;
    
    public string PlayerName
    {
        get => text.text;
        set => text.text = value;
    }
    
    public bool EnableTomato
    {
        get => _tomato;
        set
        {
            _tomato = value;
            tomato.SetActive(value);
        }
    }

    public static Queue<Player> Players { get; } = new();
    
    private int _health = 3;

    public int Health
    {
        get => _health;
        set
        {
            if (value is < 0 or > 3) return;
            _health = value;
            hearths.Value = value;
        }
    }

    private void OnEnable()
    {
        StateMachine.Instance.onStateChange.AddListener(OnStateChange);
    }
    
    private void OnDisable()
    {
        StateMachine.Instance.onStateChange.RemoveListener(OnStateChange);
    }

    public void Begging()
    {
        showBegging.SetActive(true);
    }
    
    public void NotBegging()
    {
        showBegging.SetActive(false);
    }

    public void ShowTomato()
    {
        
    }
    
    public void HideTomato()
    {
        
    }
    
    public void Head()
    {
        mainSprite.SetActive(false);
        headSprite.SetActive(true);
    }
    
    public void Body()
    {
        mainSprite.SetActive(true);
        headSprite.SetActive(false);
    }
    
    private void OnStateChange(State state)
    {
        if (state is GameStart)
        {
            gameObject.SetActive(PlayerGamepad != null);
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class King : MonoBehaviour
{
    [SerializeField] private SpriteRenderer mainSpriteRenderer;

    public static King Instance { get; private set; }
    
    public Gamepad PlayerGamepad { get; set; }
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerPick : MonoBehaviour
{
    [SerializeField] private TMP_Text controllerText;
    [SerializeField] private GameObject waiting;
    [SerializeField] private GameObject picked;
    
    [field:SerializeField] public int PlayerNumber { get; set; }
    
    private void Start()
    {
        controllerText.text = $"Player {PlayerNumber + 1}";
        waiting.SetActive(true);
        picked.SetActive(false);
    }

    public void Pick()
    {
        waiting.SetActive(false);
        picked.SetActive(true);
    }
    
    public void Unpick()
    {
        waiting.SetActive(true);
        picked.SetActive(false);
    }
}

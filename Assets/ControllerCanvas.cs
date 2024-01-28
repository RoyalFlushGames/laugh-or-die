using UnityEngine;

public class ControllerCanvas : MonoBehaviour
{
    private void Start()
    {
        StateMachine.Instance.onStateChange.AddListener(OnStateChange);
    }

    private void OnStateChange(State newState)
    {
        gameObject.SetActive(newState is MainMenuState);
    }
}

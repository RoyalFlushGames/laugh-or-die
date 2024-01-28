using UnityEngine;

public class TutorialCanvas : MonoBehaviour
{
    private void Start()
    {
        StateMachine.Instance.onStateChange.AddListener(OnStateChange);
        gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        StateMachine.Instance.onStateChange.RemoveListener(OnStateChange);
    }
    
    private void OnStateChange(State state)
    {
        gameObject.SetActive(state is Tutorial);
    }
}

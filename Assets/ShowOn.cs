using UnityEngine;

public class ShowOn : MonoBehaviour
{
    protected CanvasGroup canvasGroup;

    private void Start() => canvasGroup = gameObject.AddComponent<CanvasGroup>();
    private void OnEnable() => StateMachine.Instance.onStateChange.AddListener(OnStateChange);
    private void OnDisable() => StateMachine.Instance.onStateChange.RemoveListener(OnStateChange);

    protected virtual void OnStateChange(State state) { }
}
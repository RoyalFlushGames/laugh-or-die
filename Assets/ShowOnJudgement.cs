using System;
using UnityEngine;

public class ShowOnJudgement : MonoBehaviour
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
        gameObject.SetActive(state is Judgement);
    }
}
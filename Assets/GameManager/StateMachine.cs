using UnityEngine;
using UnityEngine.Events;

public class StateMachine : MonoBehaviour
{
    public static StateMachine Instance { get; private set; }
    
    private State _previousState;
    public State CurrentState { get; private set; }

    public UnityEvent<State> onStateChange = new();
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        ChangeState(new MainMenuState());
    }

    public void ChangeState(State newState)
    {
        CurrentState?.End();
        newState.stateMachine = this;
        _previousState = CurrentState;
        CurrentState = newState;
        CurrentState.Start();
        Debug.Log(CurrentState.ToString());
        if (CurrentState != null) onStateChange.Invoke(CurrentState);
    }

    private void Update() => CurrentState?.Update();
}

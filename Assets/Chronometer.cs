using System.Collections;
using TMPro;
using UnityEngine;

public class Chronometer : MonoBehaviour
{
    [SerializeField] private TMP_Text timer;
    [SerializeField] private int timeInSeconds;
    [SerializeField] private CanvasGroup canvasGroup;
    
    private IEnumerator _timerEnumerator;

    private void OnEnable()
    {
        canvasGroup.alpha = 0;
        StateMachine.Instance.onStateChange.AddListener(OnStateChange);
    }

    private void OnDisable()
    {
        StateMachine.Instance.onStateChange.RemoveListener(OnStateChange);
        _timerEnumerator = null;
        StopAllCoroutines();
    }

    private void OnStateChange(State state)
    {
        if (state is Joking)
        {
            canvasGroup.alpha = 1;
            _timerEnumerator = Timer();
            StartCoroutine(_timerEnumerator);
        }
        else
        {
            if (_timerEnumerator == null) return;
            canvasGroup.alpha = 0;
            StopCoroutine(_timerEnumerator);
            _timerEnumerator = null;
        }
    }

    private IEnumerator Timer()
    {
        int time = timeInSeconds;

        while (time > 0)
        {
            yield return new WaitForSeconds(1);
            time--;
            timer.text = $"{time}s";
        }
        
        StateMachine.Instance.ChangeState(new Fail());
    }
}

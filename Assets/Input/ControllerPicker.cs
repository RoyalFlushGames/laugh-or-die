using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class ControllerPicker : MonoBehaviour
{
    private Dictionary<Gamepad, IEnumerator> _gamepads = new();
    private List<Gamepad> _joinedGamepads = new();
    private IEnumerator _startButton;
    
    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
        
        foreach (var gamepad in Gamepad.all)
        {
            _gamepads.Add(gamepad, WaitToButton(gamepad));
            StartCoroutine(_gamepads[gamepad]);
        }
        
        _startButton = WaitToStartButton();
        if (_startButton == null) return;
        StartCoroutine(_startButton);
    }

    private IEnumerator WaitToStartButton()
    {
        yield return new WaitUntil(() =>
        {
            var gamePads = Gamepad.all.ToList();
            var pressed = gamePads.Any(x => x.allControls.Any(y => y is ButtonControl && y.IsPressed() && y.path == x.startButton.path));
            return pressed && _joinedGamepads.Count > 1;
        });

        var players = FindObjectsOfType<Player>();
        
        FindObjectOfType<King>().PlayerGamepad = _joinedGamepads[0];
        
        Debug.Log(players.Length);
        Debug.Log(_joinedGamepads.Count);
        
        for(var i = 1; i < _joinedGamepads.Count; i++)
        {
            players[i - 1].PlayerGamepad = _joinedGamepads[i];
        }
        
        StateMachine.Instance.ChangeState(new Tutorial());
    }

    private IEnumerator WaitToButton(Gamepad gamepad)
    {
        yield return new WaitUntil(() => gamepad.allControls.Where(x => x is ButtonControl).All(y => !y.IsPressed()));
        yield return new WaitUntil(() => gamepad.allControls.Any(x => x is ButtonControl && x.IsPressed() && !x.synthetic && x.path != gamepad.startButton.path && x.path != gamepad.selectButton.path));
        if (_joinedGamepads.Count >= 4) yield break;
        _joinedGamepads.Add(gamepad);
        FindObjectsOfType<ControllerPick>(true).FirstOrDefault(x => x.PlayerNumber == _joinedGamepads.IndexOf(gamepad))?.Pick();
        _gamepads[gamepad] = SelectedController(gamepad);
        StartCoroutine(_gamepads[gamepad]);
    }

    private IEnumerator SelectedController(Gamepad gamepad)
    {
        yield return new WaitUntil(() => gamepad.allControls.Where(x => x is ButtonControl).All(y => !y.IsPressed()));
        yield return new WaitUntil(() => gamepad.buttonEast.wasPressedThisFrame);
        FindObjectsOfType<ControllerPick>(true).FirstOrDefault(x => x.PlayerNumber == _joinedGamepads.IndexOf(gamepad))?.Unpick();
        _joinedGamepads.Remove(gamepad);
        _gamepads[gamepad] = WaitToButton(gamepad);
        StartCoroutine(_gamepads[gamepad]);
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
        _startButton = null;
        StopAllCoroutines();
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
            {
                if (device is Gamepad gamepad)
                {
                    _gamepads.Add(gamepad, WaitToButton(gamepad));
                    StartCoroutine(_gamepads[gamepad]);
                }

                break;
            }
            case InputDeviceChange.Removed:
            {
                if (device is Gamepad gamepad)
                {
                    StopCoroutine(_gamepads[gamepad]);
                    _gamepads.Remove(gamepad);
                }

                break;
            }
        }
    }
}

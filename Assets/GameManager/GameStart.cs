using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Positioning : State
{
    public override void Start()
    {
        Fade.Instance.FadeOut();
        stateMachine.StartCoroutine(Wait());
        base.Start();
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
        stateMachine.ChangeState(new Joking());
    }
}

public class Judgement : State
{
    public override void Update()
    {
        if (King.Instance.PlayerGamepad.buttonEast.wasPressedThisFrame)
        {
            stateMachine.ChangeState(new Disapproved());
        }
        if (King.Instance.PlayerGamepad.buttonSouth.wasPressedThisFrame)
        {
            stateMachine.ChangeState(new Approved());
        }
        
        base.Update();
    }
}

public class Approved : State
{
    public override void Start()
    {
        stateMachine.StartCoroutine(Sequential());
        base.Start();
    }

    private IEnumerator Sequential()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(new Reordening());
    }
}

public class Disapproved : State
{
    public override void Start()
    {
        stateMachine.StartCoroutine(Sequential());
        base.Start();
    }

    private IEnumerator Sequential()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(new Begging());
    }
}

public class Begging : State
{
    private Player _joker;
    
    public override void Start()
    {
        _joker = Player.Players.Peek();
        _joker.Begging();
        base.Start();
    }

    public override void Update()
    {
        if (_joker.PlayerGamepad.buttonSouth.wasPressedThisFrame)
        {
            stateMachine.ChangeState(new Punishing());
        }
        if (_joker.PlayerGamepad.buttonEast.wasPressedThisFrame)
        {
            stateMachine.ChangeState(new Fail());
        }
        base.Update();
    }

    public override void End()
    {
        _joker.NotBegging();
        base.End();
    }
}

public class Punishing : State
{
    public override void Update()
    {
        if (King.Instance.PlayerGamepad.buttonSouth.wasPressedThisFrame)
        {
            stateMachine.ChangeState(new Reordening());
        }
        if (King.Instance.PlayerGamepad.buttonEast.wasPressedThisFrame)
        {
            stateMachine.ChangeState(new Fail(2));
        }
        base.Update();
    }
}

public class NextRound : State
{
    public override void Start()
    {
        stateMachine.StartCoroutine(Animation());
        base.Start();
    }

    private IEnumerator Animation()
    {
        yield return new WaitForSeconds(1f);
        Fade.Instance.AutoFade();
        yield return new WaitForSeconds(1f);
        stateMachine.ChangeState(new Positioning());
    }
}

public class Tutorial : State
{
    public override void Start()
    {
        stateMachine.StartCoroutine(WaitTillEndOfFrame());
        base.Start();
    }

    private IEnumerator WaitTillEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        Object.FindObjectsOfType<Player>().ToList().ForEach(x =>
        {
            Object.FindObjectOfType<TutorialCanvas>(true).StartCoroutine(WaitToButton(x.PlayerGamepad));
        });
        Object.FindObjectOfType<TutorialCanvas>(true).StartCoroutine(WaitToButton(King.Instance.PlayerGamepad));
    }
    
    private IEnumerator WaitToButton(Gamepad gamepad)
    {
        yield return new WaitUntil(() => gamepad.allControls.Where(x => x is ButtonControl).All(y => !y.IsPressed()));
        yield return new WaitUntil(() => gamepad.allControls.Any(x => x is ButtonControl && x.IsPressed() && !x.synthetic && x.path == gamepad.buttonSouth.path));
        stateMachine.ChangeState(new Recognition());
    }

    public override void End()
    {
        Object.FindObjectOfType<TutorialCanvas>(true).StopAllCoroutines();
        base.End();
    }
}

public class Recognition : State
{
    private PlayerRecognition _playerRecognition;
    
    public override void Start()
    {
        _playerRecognition = Object.FindObjectOfType<PlayerRecognition>(true);
        _playerRecognition.gameObject.SetActive(true);
        _playerRecognition.StartCoroutine(Sequencer());
        
        base.Start();
    }
    
    private IEnumerator Sequencer()
    {
        yield return new WaitForSeconds(1);
        King.Instance.PlayerGamepad.SetMotorSpeeds(1f, 1f);
        _playerRecognition.Show(true);
        _playerRecognition.SetText($"You are the King and the Player 4");
        yield return new WaitForSeconds(2);
        King.Instance.PlayerGamepad.SetMotorSpeeds(0f, 0f);
        yield return new WaitForSeconds(2);
        
        foreach (var player in Object.FindObjectsOfType<Player>().Where(x => x.PlayerGamepad != null))
        {
            player.PlayerGamepad.SetMotorSpeeds(1f, 1f);
            _playerRecognition.Show(true);
            _playerRecognition.SetText($"You are a Jester and the {player.PlayerName}");
            yield return new WaitForSeconds(2);
            player.PlayerGamepad.SetMotorSpeeds(0f, 0f);
            yield return new WaitForSeconds(2);
        }
        
        _playerRecognition.Show(false);
        stateMachine.ChangeState(new GameStart());
    }
    
    public override void End()
    {
        _playerRecognition.gameObject.SetActive(false);
        base.End();
    }
}

public class GameStart : State
{
    public override void Start()
    {
        stateMachine.StartCoroutine(Animation());
        base.Start();
    }

    private void PlayersOrder()
    {
        var players = Object.FindObjectsOfType<Player>();
        players = players.OrderBy(_ => Random.Range(0, 5)).ToArray();
        
        foreach (var player in players)
        {
            Player.Players.Enqueue(player);
        }
    }

    private IEnumerator Animation()
    {
        yield return new WaitForSeconds(1f);
        PlayersOrder();
        yield return new WaitForSeconds(2f);
        Fade.Instance.AutoFade();
        yield return new WaitForSeconds(1f);
        stateMachine.ChangeState(new Positioning());
    }
}
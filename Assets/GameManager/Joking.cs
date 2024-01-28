using System.Collections;
using System.Linq;
using UnityEngine;

public class Fail : State
{
    private int _lives;

    public Fail(int lives = 1)
    {
        _lives = lives;
    }
    
    public override void Start()
    {
        base.Start();
        stateMachine.StartCoroutine(Sequencer());
    }
    
    private IEnumerator Sequencer()
    {
        yield return new WaitForEndOfFrame();
        var joker = Player.Players.Peek();
        joker.Health -= _lives;
        stateMachine.ChangeState(new Reordening());
    }
}

public class Reordening : State
{
    public override void Start()
    {
        base.Start();
        stateMachine.StartCoroutine(Sequencer());
    }
    
    private IEnumerator Sequencer()
    {
        yield return new WaitForEndOfFrame();
        var joker = Player.Players.Dequeue();
        Player.Players.Enqueue(joker);
        stateMachine.ChangeState(new NextRound());
    }
}

public class Joking : State
{
    private Player _joker;
    private bool _tomating;
    private int _tomatoHits;
    
    public override void Start()
    {
        base.Start();
        _joker = Player.Players.Peek();
        stateMachine.StartCoroutine(Sequencer());
        var waitingPlayers = Player.Players.Where(x => x != _joker);
        foreach (var player in waitingPlayers)
        {
            Object.FindObjectOfType<TomatoController>().StartCoroutine(Tomatoer(player));
        }
    }

    private IEnumerator Tomatoer(Player player)
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            player.EnableTomato = true;
            yield return new WaitUntil(() => player.PlayerGamepad.buttonSouth.wasPressedThisFrame && !_tomating);
            _tomating = true;
            player.EnableTomato = false;
            _joker.PlayerGamepad.SetMotorSpeeds(1f, 1f);
            yield return new WaitForSeconds(0.5f);
            _joker.PlayerGamepad.SetMotorSpeeds(0, 0);
            yield return Evading();
            _tomating = false;
            yield return null;
        }
    }

    private IEnumerator Evading()
    {
        var t = 0f;
        var list = Object.FindObjectOfType<TomatoController>().Tomatoes;
        while (t < 2f)
        {
            if (_joker.PlayerGamepad.buttonSouth.wasPressedThisFrame)
            {
                list[_tomatoHits].Main(true);
                yield return new WaitForSeconds(0.3f);
                list[_tomatoHits].Main(false);
                yield break;
            }
            t += Time.deltaTime;
            yield return null;
        }
        
        list[_tomatoHits].Main(true);
        yield return new WaitForSeconds(0.3f);
        list[_tomatoHits].Main(false);
        list[_tomatoHits].Exploded(true);
        yield return new WaitForSeconds(0.4f);
        _tomatoHits++;
    }

    public override void Update()
    {
        base.Update();
        if (_joker.PlayerGamepad.startButton.wasPressedThisFrame)
        {
            stateMachine.ChangeState(new Judgement());
        }

        if (_tomatoHits >= 3)
        {
            stateMachine.ChangeState(new Fail());
        }
    }
    
    public override void End()
    {
        base.End();
        foreach (var player in Player.Players)
        {
            player.EnableTomato = false;
        }
        Object.FindObjectOfType<TomatoController>().Tomatoes.ForEach(x =>
        {
            x.Exploded(false);
            x.Main(false);
        });
        _tomating = false;
        _tomatoHits = 0;
        
        Object.FindObjectOfType<TomatoController>().StopAllCoroutines();
    }
    
    private IEnumerator Sequencer()
    {
        yield return Rumble();
    }
    
    private IEnumerator Rumble()
    {
       _joker.PlayerGamepad.SetMotorSpeeds(1f, 1f);
        yield return new WaitForSeconds(0.5f);
        _joker.PlayerGamepad.SetMotorSpeeds(0, 0);
    }
}
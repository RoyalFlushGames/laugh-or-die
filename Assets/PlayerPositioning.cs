using System.Linq;
using UnityEngine;

public class PlayerPositioning : MonoBehaviour
{
    [SerializeField] private int index;
    
    private void OnEnable()
    {
        StateMachine.Instance.onStateChange.AddListener(OnStateChange);
    }
    
    private void OnDisable()
    {
        StateMachine.Instance.onStateChange.RemoveListener(OnStateChange);
    }

    private void OnStateChange(State state)
    {
        if (state is not Positioning) return;
        var players = Player.Players.ToArray();
        if (players.Length <= index) return;
        var player = players[index];
        player.transform.position = transform.position;
        Player.Players.ToList().ForEach(x => x.Body());
        Player.Players.Peek().Head();
    }
}

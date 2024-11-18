using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    /*LISTENS FOR GAME EVENTS AND RESPONDS WITH INVOKING UNITY EVENT*/

    [Tooltip("Event to register with.")]
    public GameEvent gameEvent;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        response.Invoke();
    }
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "Events/GameEvent")]
public class GameEvent : ScriptableObject
{
    /*THIS IS A SCRIPTABLE OBJECT THAT ALLOWS TO MANAGE LIST OF LISTENERS AND NOTIFY THEM WHEN EVENT RAISED  */

    private List<GameEventListener> listeners = new List<GameEventListener>(); //list of listeners

    public void Raise() //when event occurs it notifies all listeners 
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener listener) //if listener is not in the list it adds it
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener) //if listener is in the list it removes it
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
}

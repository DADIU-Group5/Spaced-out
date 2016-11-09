using UnityEngine;
using System.Collections.Generic;

//Invokes the notificaton method
public class Subject : Singleton<Subject>
{
    //A list with observers that are waiting for something to happen
    List<Observer> observers = new List<Observer>();

    //Send notifications if something has happened
    public void Notify(GameObject entity, ObserverEvent evt)
    {
        foreach (var observer in observers)
        {
            //Notify all observers even though some may not be interested in what has happened
            //Each observer should check if it is interested in this event
            observer.OnNotify(entity, evt);
        }
    }

    //Add observer to the list
    public void AddObserver(Observer observer)
    {
        observers.Add(observer);
    }

    //Remove observer from the list
    public void RemoveObserver(Observer observer)
    {
        observers.Remove(observer);
    }
}

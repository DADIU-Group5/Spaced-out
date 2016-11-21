using UnityEngine;
using System.Collections;

public class KeyObject : MonoBehaviour {

    /// <summary>
    /// Check for player collision
    /// </summary>
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player")
        {
            Debug.Log("found player!");
            var evt = new ObserverEvent(EventName.PlayerWon);
            Subject.instance.Notify(gameObject, evt);
            Debug.Log("destroying key");
            Destroy(this.gameObject);
        }
    }
}

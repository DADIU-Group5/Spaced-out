using UnityEngine;
using System.Collections;

public class CollectibleElement : MonoBehaviour {

    bool added = false;

    void Awake()
    {
        if (!added)
        {
            added = true;
            ScoreManager.instance.AddComics();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            var evt = new ObserverEvent(EventName.ComicPickup);
            Subject.instance.Notify(gameObject, evt);
            Destroy(gameObject);
        }
    }
}

using UnityEngine;
using System.Collections;

public class CollectibleElement : MonoBehaviour {

    bool added = false;

    void Start()
    {
        if (!added)
        {
            added = true;
            Debug.Log("adding comic...");
            ScoreManager.instance.AddComics();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            ScoreManager.instance.ComicCollected();
            var evt = new ObserverEvent(EventName.ComicPickup);
            Subject.instance.Notify(gameObject, evt);
            Destroy(gameObject);
        }
    }
}

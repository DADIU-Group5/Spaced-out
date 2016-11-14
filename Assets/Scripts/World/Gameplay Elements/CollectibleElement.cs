using UnityEngine;
using System.Collections;

public class CollectibleElement : MonoBehaviour {

    void Start()
    {
        ScoreManager.instance.AddCollectibleToLevel();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            ScoreManager.instance.AddCollectibles();
            Destroy(gameObject);
        }
    }
}

﻿using UnityEngine;
using System.Collections;

public class CollectibleElement : MonoBehaviour {

    void Start()
    {
        Debug.Log("adding comic...");
        ScoreManager.instance.AddComics();
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            ScoreManager.instance.ComicCollected();
            Destroy(gameObject);
        }
    }
}

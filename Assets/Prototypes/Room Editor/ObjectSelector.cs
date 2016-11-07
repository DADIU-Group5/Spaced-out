using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ObjectSelector : MonoBehaviour {

    public List<Object> canBe;
    GameObject GOshowing;
    int showing;

	// Use this for initialization
	void Start () {
        LoadObjects();
	}

    public virtual void LoadObjects()
    {
        //Debug.Log("canbe: " + canBe.Count);
    }

    public void ShowNext()
    {
        showing++;
        if (showing >= canBe.Count)
        {
            showing = 0;
        }
        ShowModel();
    }

    public void ShowPrev()
    {
        showing--;
        if (showing <= -1)
        {
            showing = canBe.Count - 1;
        }
        ShowModel();
    }

    public void ShowDefualt()
    {
        DestroyShowingModel();
        gameObject.GetComponent<Renderer>().enabled = true;
    }

    public void ShowRandom()
    {
        showing = Random.Range(0, canBe.Count);
        ShowModel();
    }

    void ShowModel()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        DestroyShowingModel();
        GOshowing = Instantiate((GameObject)canBe[showing],transform.position,Quaternion.identity,transform) as GameObject;
        GOshowing.transform.rotation = gameObject.transform.rotation;
    }

    void DestroyShowingModel()
    {
        if (GOshowing != null)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(GOshowing);
            }
            else
            {
                Destroy(GOshowing);
            }
        }
    }

    public void Replace()
    {
        if(canBe.Count == 0)
        {
            return;
        }
        showing = Random.Range(0, canBe.Count);
        GOshowing = Instantiate((GameObject)canBe[showing], transform.position, Quaternion.identity, transform.parent) as GameObject;
        if(gameObject.transform.localScale != Vector3.one){
            GOshowing.transform.localScale = gameObject.transform.localScale;
        }
        GOshowing.transform.rotation = gameObject.transform.rotation;
        Destroy(gameObject);
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ObjectSelector : MonoBehaviour {

    public List<Object> canBe;
    GameObject GOshowing;
    int showing;
    public bool lockObject = false;
    public Object lockedAs;

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

    /// <summary>
    /// Displays a random model that this object can be.
    /// </summary>
    public void ShowRandom()
    {
        showing = Random.Range(0, canBe.Count);
        ShowModel();
    }

    /// <summary>
    /// Creates the model for the selected object.
    /// </summary>
    void ShowModel()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        DestroyShowingModel();
        GOshowing = Instantiate((GameObject)canBe[showing],transform.position,Quaternion.identity,transform) as GameObject;
        GOshowing.transform.rotation = gameObject.transform.rotation;
    }

    /// <summary>
    /// Removes the visual model.
    /// </summary>
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

    /// <summary>
    /// Used in level generation to replace the object with a gameplay object.
    /// </summary>
    public void Replace()
    {
        if(canBe.Count == 0)
        {
            Debug.LogError("This object CANNOT become a object, is the object in the correct resources folder?");
            return;
        }
        //If locked use the locked object.
        if (lockObject)
        {
            ReplaceModel(lockedAs);
        }
        //Selects a random object to become.
        else
        {
            showing = Random.Range(0, canBe.Count);
            ReplaceModel(canBe[showing]);
        }
        Destroy(gameObject);
    }

    //Replaces the 'dummy' object with a gameplay object.
    void ReplaceModel(Object obj)
    {
        GOshowing = Instantiate((GameObject)obj, transform.position, Quaternion.identity, transform.parent) as GameObject;
        if (gameObject.transform.localScale != Vector3.one)
        {
            GOshowing.transform.localScale = gameObject.transform.localScale;
        }
        GOshowing.transform.rotation = gameObject.transform.rotation;
    }

    public void LockObject()
    {
        lockObject = true;
        lockedAs = (Object)GOshowing;
    }

    public void UnlockObject()
    {
        lockObject = false;
        if(GOshowing == null)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            else
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            gameObject.GetComponent<Renderer>().enabled = true;
        }
        lockedAs = null;
    }
}

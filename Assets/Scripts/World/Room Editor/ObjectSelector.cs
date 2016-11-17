using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectSelector : MonoBehaviour {

    public List<GameObject> canBe;
    GameObject GOshowing;
    int showing;
    public bool lockObject = false;
    public GameObject lockedAs;

    // Use this for initialization
    void Start () {
        //LoadObjects();
	}

    public virtual void LoadObjects()
    {
        /*if(gameObject.name[0] == 'x')
        {
            Debug.Log(gameObject.name);
        }*/
        //Debug.Log("canbe: " + canBe.Count);
        //Debug.Log(canBe[0].name);
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
        GOshowing = Instantiate(canBe[showing],transform.position,Quaternion.identity,transform) as GameObject;
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
    public virtual void Replace(Room r)
    {
        LoadObjects();
        if(canBe.Count == 0)
        {
            Debug.Log("is this why?");
            LoadObjects();
            if (canBe.Count == 0)
            {
                Debug.LogError("This object CANNOT become a object, is the object in the correct resources folder?" + gameObject.name);
                return;
            }
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
        if(GOshowing == null)
        {
            return;
        }
        if(GOshowing.GetComponent<HazardState>() != null)
        {
            r.AddHazard(GOshowing.GetComponent<HazardState>());
        }
        if(GOshowing.GetComponent<SwitchItem>() != null)
        {
            GOshowing.GetComponent<SwitchItem>().AssignRoom(r);
        }
        Destroy(gameObject);
    }

    //Replaces the 'dummy' object with a gameplay object.
    void ReplaceModel(GameObject obj)
    {

        if(obj == null)
        {
            Debug.Log(lockObject);
            Debug.Log(this.name+"error");
            return;
        }
        
        GOshowing = Instantiate(obj, transform.position, Quaternion.identity, transform.parent) as GameObject;
        if (transform.localScale != Vector3.one)
        {
            GOshowing.transform.localScale = transform.localScale;
        }
        GOshowing.transform.rotation = transform.rotation;
    }

    public void LockObject()
    {
        lockObject = true;
        lockedAs = GOshowing;
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

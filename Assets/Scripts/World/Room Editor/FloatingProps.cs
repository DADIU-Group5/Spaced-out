using UnityEngine;
using System.Collections;

public class FloatingProps : ObjectSelector {

    float density = 0.1f;

    public override void LoadObjects()
    {
        canBe = ObjectDatabase.instance.GetFloatingObjects();
        base.LoadObjects();
    }

    public override void Replace(Room r)
    {
        LoadObjects();
        transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
        Vector3 halfSize = transform.localScale;
        halfSize /= 2;
        float boxSize = transform.localScale.x * transform.localScale.y * transform.localScale.z;
        float toSpawn = boxSize * density;
        float x = 0;
        float y = 0;
        float z = 0;
        for (int i = 0; i < (int)toSpawn; i++)
        {
            x = Random.Range(-halfSize.x, halfSize.x);
            y = Random.Range(-halfSize.y, halfSize.y);
            z = Random.Range(-halfSize.z, halfSize.z);
            Instantiate(canBe[Random.Range(0, canBe.Count)], transform.position+new Vector3(x, y, z), Random.rotation, transform.parent);
        }
        Destroy(gameObject);
    }
}

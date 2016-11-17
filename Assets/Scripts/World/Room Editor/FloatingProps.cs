using UnityEngine;
using System.Collections;

public class FloatingProps : ObjectSelector {

    float density = 1;

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
            x = Random.Range(-halfSize.x + 0.5f, halfSize.x - 0.5f);
            y = Random.Range(-halfSize.y + 0.5f, halfSize.y - 0.5f);
            z = Random.Range(-halfSize.z + 0.5f, halfSize.z - 0.5f);
            Instantiate(canBe[Random.Range(0, canBe.Count)], transform.position+new Vector3(x, y, z), Quaternion.identity,transform.parent);
        }
        Destroy(gameObject);
    }
}

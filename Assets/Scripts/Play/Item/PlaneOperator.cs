using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

// plane movement & drop vaccine
public class Plane : PoolAble
{
    public Vector3 dropPos;

    private Vector3 direction = new Vector3(-1, 0, 0);
    private float speed = 5.0f;
    private bool alreadyDrop = false;

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (!alreadyDrop && transform.position.x < dropPos.x)
        {
            DropVaccine();
            alreadyDrop = true;
        }

        if(transform.position.x < -20.0f)
        {
            alreadyDrop = false;
            ReleaseObject();
        }
    }

    // vaccine pool get
    private void DropVaccine()
    {
        var vaccine = ObjectPoolManager.Instance.GetObject("Vaccine");
        vaccine.transform.position = gameObject.transform.position;
    }
}

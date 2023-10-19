using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

// plane movement & drop vaccine
public class Plane : PoolAble
{
    private Vector3 direction = new Vector3(-1, 0, 0);
    private float speed = 5.0f;
    private float time = 0f;
    private float dropTime = 3.0f;
    private bool alreadyDrop = false;

    private void Update()
    {
        gameObject.transform.Translate(direction * speed * Time.deltaTime);
        time += Time.deltaTime;

        if (!alreadyDrop && time > dropTime)
        {
            DropVaccine();
            alreadyDrop = true;
        }

        if(gameObject.transform.position.x < -20.0f)
        {
            time = 0f;
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

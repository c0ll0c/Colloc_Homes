using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// game manage script
public class PlayManager : MonoBehaviour
{
    public static PlayManager Instance;
    public Tilemap[] layer;

    private int[] randomDropTime = new int[3];
    private float time = 0f;
    private int index = 0;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        Init();
    }

    private void Init()
    {
        for (int i = 0; i < 3; i++)
        {
            randomDropTime[i] = Random.Range(0, 30);
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
        if(index < 3 && time > randomDropTime[index])
        {
            var plane = ObjectPoolManager.Instance.GetObject("Plane");
            plane.transform.position = new Vector3(25.0f, Random.Range(-8.0f, 17.0f), 0f);
            index++;
        }
    }
}

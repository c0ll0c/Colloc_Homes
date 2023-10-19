using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// object pooling manage script
public class ObjectPoolManager : MonoBehaviour
{

    [System.Serializable] private class ObjectInfo
    {
        public string name;
        public GameObject prefab;
    } 

    public static ObjectPoolManager Instance;

    [SerializeField] private ObjectInfo[] objectInfos = null;
    private Dictionary<string, IObjectPool<GameObject>> poolDic = new Dictionary<string, IObjectPool<GameObject>>();
    private Dictionary<string, GameObject> objectDic = new Dictionary<string, GameObject>();
    private int defaultCapacity = 1;
    private int maxPoolSize = 4;
    private string objectName;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        Init();
    }

    // pool �⺻ ����
    private void Init()
    {
        for (int i = 0; i < objectInfos.Length; i++)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, 
                OnDestroyPoolObject, true, defaultCapacity, maxPoolSize);
            
            objectDic.Add(objectInfos[i].name, objectInfos[i].prefab);
            poolDic.Add(objectInfos[i].name, pool);

            objectName = objectInfos[i].name;
            PoolAble poolAbleGo = CreatePooledItem().GetComponent<PoolAble>();
            poolAbleGo.Pool.Release(poolAbleGo.gameObject);
        }
    }

    // ����
    private GameObject CreatePooledItem()
    {
        GameObject poolGo = Instantiate(objectDic[objectName]);
        poolGo.GetComponent<PoolAble>().Pool = poolDic[objectName];
        return poolGo;
    }

    // ���
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }

    // ��ȯ
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }

    // ����
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }

    public GameObject GetObject(string name)
    {
        objectName = name;
        return poolDic[name].Get();
    }
}

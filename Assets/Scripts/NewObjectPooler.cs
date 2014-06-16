using UnityEngine;
using System.Collections;

public class NewObjectPooler : MonoBehaviour{
    public int objectAmount = 10;

    public BetterList<GameObject> pooledObjects;
    public GameObject pooledObject;

    public void Init (GameObject obj, int objAmount = 10){
        if (pooledObjects != null) return;
        pooledObjects = new BetterList<GameObject>();
        pooledObject = obj;
        objectAmount = objAmount;
        for (int i = 0; i < objectAmount; i++){
            GameObject go = (GameObject)Instantiate(pooledObject);
            go.SetActive(false);
            pooledObjects.Add(go);
        }
    }

    public GameObject GetPooledObject(){
        GameObject obj = null;
        for (int i = 0; i < pooledObjects.size; i++){
            obj = pooledObjects[i];
            if (!obj.activeInHierarchy){
                return obj;
            }
        }
        if (obj == null){
            obj = CreateNewObj();
            pooledObjects.Add(obj);
        }
        return obj;
    }

    GameObject CreateNewObj (){
        GameObject obj = (GameObject)Instantiate(pooledObject);
        obj.SetActive(false);
        return obj;
    }
}


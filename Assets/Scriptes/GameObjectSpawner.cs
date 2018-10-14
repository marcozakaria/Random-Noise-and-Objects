using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSpawner : PersistableObject
{
    public PersistableObject prefap;

    public KeyCode createKey = KeyCode.C;
    public KeyCode newGameKey = KeyCode.N;
    public KeyCode saveKey = KeyCode.S;
    public KeyCode loadKey = KeyCode.L;

    public PersistentStorage storage;

    List<PersistableObject> objects;

    private void Awake()
    {
        objects = new List<PersistableObject>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(createKey))
        {
            CreateObject();
        }
        else if (Input.GetKeyDown(newGameKey))
        {
            BeginNewGame();
        }
        else if (Input.GetKeyDown(saveKey))
        {
            storage.Save(this);
        }
        else if (Input.GetKeyDown(loadKey))
        {
            BeginNewGame();
            storage.Load(this);
        }
    }

    void BeginNewGame()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            Destroy(objects[i].gameObject);
        }
        objects.Clear(); 
    }

    void CreateObject()
    {
        PersistableObject o = Instantiate(prefap);
        Transform objectT = o.transform;
        objectT.localPosition = Random.onUnitSphere * 5f;
        objectT.localRotation = Random.rotation;
        objectT.localScale = Vector3.one * Random.Range(0.1f, 1f);
        objects.Add(o);   
    }
}

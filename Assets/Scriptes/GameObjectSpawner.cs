using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSpawner : PersistableObject
{
    //public PersistableObject prefap;
    public ShapeFactory shapeFactory;  // shape factory contains our shapes

    public KeyCode createKey = KeyCode.C;
    public KeyCode newGameKey = KeyCode.N;
    public KeyCode saveKey = KeyCode.S;
    public KeyCode loadKey = KeyCode.L;

    public PersistentStorage storage;

    List<Shape> shapes;

    const int saveVersion = 1;

    private void Awake()
    {
        shapes = new List<Shape>();
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
            storage.Save(this); // this refer to persistanceObject class
        }
        else if (Input.GetKeyDown(loadKey))
        {
            BeginNewGame();
            storage.Load(this);
        }
    }

    void BeginNewGame()
    {
        for (int i = 0; i < shapes.Count; i++)
        {
            Destroy(shapes[i].gameObject);
        }
        shapes.Clear(); 
    }

    void CreateObject()
    {   // create a random object
        Shape instance = shapeFactory.GetRandom();
        Transform objectT = instance.transform;
        objectT.localPosition = Random.onUnitSphere * 5f;
        objectT.localRotation = Random.rotation;
        objectT.localScale = Vector3.one * Random.Range(0.1f, 1f);
        instance.SetColor(Random.ColorHSV());
        shapes.Add(instance);   // add shape to the list
    }

    public override void Save(GameDataWritter writer)
    {
        writer.Write(-saveVersion); // put version in negative to help us detect if the sve file wasdone on older versions of the game
        writer.Write(shapes.Count);
        for (int i = 0; i < shapes.Count; i++)
        {
            writer.Write(shapes[i].ShapeID);    // save shapeId and materialID first 
            writer.Write(shapes[i].MaterialID);
            shapes[i].Save(writer);         // then call th save function in shape class 
        }
    }

    public override void Load(GameDataReader reader)
    {
        int version = -reader.ReadInt();
        if (version > saveVersion) // see the version if it is eiher return
        {
            Debug.LogError("Uspported Version");
            return;
        }
        int count = version < 0 ? -version : reader.ReadInt();
        for (int i = 0; i < count; i++)
        {
            int shapeid = version > 0 ? reader.ReadInt() : 0;  // to avoid old save file conflicts
            int materialid = version > 0 ? reader.ReadInt() : 0;
            Shape instance = shapeFactory.Get(shapeid,materialid);
            instance.Load(reader);
            shapes.Add(instance);
        }
    }
}

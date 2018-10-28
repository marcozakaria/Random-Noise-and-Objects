//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameObjectSpawner : PersistableObject
{
    //public PersistableObject prefap;
    public ShapeFactory shapeFactory;  // shape factory contains our shapes

    public KeyCode createKey = KeyCode.C;
    public KeyCode newGameKey = KeyCode.N;
    public KeyCode saveKey = KeyCode.S;
    public KeyCode loadKey = KeyCode.L;
    public KeyCode destroyKey = KeyCode.X;

    public int levelCount; // number of levels

    public float CreationSpeed { get; set; } // to get and set value from slider in the gui
    public float DistructionSpeed { get; set; } // to get value from slider in the gui

    float creationProgress , distructionProgress; // when it reaches one a new shape will be created

    public PersistentStorage storage;

    List<Shape> shapes;

    const int saveVersion = 2;

    int loadedLevelBuildIndex; // keep track which level is loaded

    private void Start()
    {
        shapes = new List<Shape>();

        if (Application.isEditor)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)  // scene count total numbers of loaded scenes
            {
                Scene loadedLevel = SceneManager.GetSceneAt(i);
                if (loadedLevel.name.Contains("Level "))  // see if level 1,2 is loaded or not  before loading it in editor mode
                {
                    SceneManager.SetActiveScene(loadedLevel);
                    loadedLevelBuildIndex = loadedLevel.buildIndex;
                    return;
                }
            }          
        }

        StartCoroutine( LoadLevel(1));
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
        else if (Input.GetKeyDown(destroyKey))
        {
            DestroyShape();
        }
        else
        {
            for (int i = 1; i <= levelCount; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    BeginNewGame(); // clean every thing first
                    StartCoroutine(LoadLevel(i));
                    return;
                }
            }
        }

        creationProgress += Time.deltaTime * CreationSpeed;
        while (creationProgress >= 1f)  // we used while to avoid frame dimed situations
        {
            creationProgress -= 1f;
            CreateObject();
        }

        distructionProgress += Time.deltaTime * DistructionSpeed;
        while (distructionProgress >= 1f)  // we used while to avoid frame dimed situations
        {
            distructionProgress -= 1f;
            DestroyShape();
        }
    }

    IEnumerator LoadLevel(int levelBuildIndex)
    {
        enabled = false; // disable until we load the scene to avoid errors
        if (loadedLevelBuildIndex > 0)
        {           // remove exisint scene first before adding new one
            yield return SceneManager.UnloadSceneAsync(loadedLevelBuildIndex);
        }

        // wait a  for the scene to be loaded first
        yield return SceneManager.LoadSceneAsync(levelBuildIndex, LoadSceneMode.Additive); // add scene to current opened one

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelBuildIndex)); // make it active because it contains our light
        loadedLevelBuildIndex = levelBuildIndex;
        enabled = true;
    }

    void BeginNewGame()
    {
        for (int i = 0; i < shapes.Count; i++)
        {
            //Destroy(shapes[i].gameObject);
            shapeFactory.Reclaim(shapes[i]);
        }
        shapes.Clear(); 
    }

    void DestroyShape()
    {
        if (shapes.Count > 0)
        {
            int index = Random.Range(0, shapes.Count);
            //Destroy(shapes[index].gameObject);
            shapeFactory.Reclaim(shapes[index]); // whether to return to the pool or destroy it

            // when we destroy object a gab in the list occur 
            // we shift last element to it then we remove refrence of last one
            int lastIndex = shapes.Count - 1;
            shapes[index] = shapes[lastIndex];
            shapes.RemoveAt(lastIndex);     // remove refrence of last object
        }
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
        writer.Write(loadedLevelBuildIndex);  // save which level we were on
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
        StartCoroutine(LoadLevel(version < 2 ? 1 : reader.ReadInt())); // Load Level index saved in file
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

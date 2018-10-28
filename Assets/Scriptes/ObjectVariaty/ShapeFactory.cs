using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject
{
    [SerializeField]
    Shape[] prefaps;

    [SerializeField]
    Material[] materials;

    [SerializeField]
    bool Recycle;

    List<Shape>[] pools;

    Scene poolScene;

    void CreatePools()
    {
        pools = new List<Shape>[prefaps.Length];
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<Shape>();
        }

        if (Application.isEditor)
        {
            // Scene is a struct not a direct reference to the actual scene. As it is not serializable, 
            //A recompilation resets the struct to its default values, which indicates an unloaded scene.
            poolScene = SceneManager.GetSceneByName(name);
            if (poolScene.isLoaded)
            {
                GameObject[] rootObjects = poolScene.GetRootGameObjects(); // to get instances that webt inactive before recompilation
                for (int i = 0; i < rootObjects.Length; i++)
                {
                    Shape pooledShape = rootObjects[i].GetComponent<Shape>();
                    if (!pooledShape.gameObject.activeSelf)  // if not active
                    {
                        pools[pooledShape.ShapeID].Add(pooledShape);
                    }
                }
                return;
            }
        }

        poolScene = SceneManager.CreateScene(name);  // give it the name of the object "ShapeFactory"
    }

    // it returns an instance of shape with specific shape and materil
    public Shape Get ( int shapeID=0 ,int materialID=0) // default value of zero if we didnt give it a value
    {
        Shape instance;

        if (Recycle)
        {
            if (pools == null)
            {
                CreatePools();
            }

            List<Shape> pool = pools[shapeID]; // make a refrence to the pool list
            int lastIndex = pool.Count - 1;

            if (lastIndex > 0) // check if pool not empty
            {
                instance = pool[lastIndex];
                pool.RemoveAt(lastIndex);
            }
            else
            {   // if empty instanciate new object 
                instance = Instantiate(prefaps[shapeID]);
                instance.ShapeID = shapeID;

                SceneManager.MoveGameObjectToScene(instance.gameObject, poolScene);
            }
            instance.gameObject.SetActive(true);
            
        }
        else // not using pools
        {
            instance = Instantiate(prefaps[shapeID]);
            instance.ShapeID = shapeID;           
        }

        instance.SetMaterial(materials[materialID], materialID);
        return instance;
    }

    public void Reclaim(Shape shapeToRecycle)   //reclaim shapes that are no longer needed to the pool
    {
        if (Recycle)
        {
            if (pools == null)
            {
                CreatePools();
            }
            pools[shapeToRecycle.ShapeID].Add(shapeToRecycle); // add shape to the pool and disable it
            shapeToRecycle.gameObject.SetActive(false);
        }
        else
        {
            Destroy(shapeToRecycle.gameObject);
        }
    }

    // returns a random shape with random material type
    public Shape GetRandom()
    {
        return Get(Random.Range(0, prefaps.Length), Random.Range(0, materials.Length));
    }
}

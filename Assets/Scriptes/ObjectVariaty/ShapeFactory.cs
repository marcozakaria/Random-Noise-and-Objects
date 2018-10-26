using UnityEngine;
using System.Collections.Generic;

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

    void CreatePools()
    {
        pools = new List<Shape>[prefaps.Length];
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<Shape>();
        }
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

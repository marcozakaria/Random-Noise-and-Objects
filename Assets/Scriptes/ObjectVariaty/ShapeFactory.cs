﻿using UnityEngine;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject
{
    [SerializeField]
    Shape[] prefaps;

    [SerializeField]
    Material[] materials;

    // it returns an instance of shape with specific shape and materil
    public Shape Get ( int shapeID=0 ,int materialID=0) // default value of zero if we didnt give it a value
    {   
        Shape instance = Instantiate(prefaps[shapeID]);
        instance.ShapeID = shapeID;
        instance.SetMaterial(materials[materialID], materialID);
        return instance;
    }

    // returns a random shape with random material type
    public Shape GetRandom()
    {
        return Get(Random.Range(0, prefaps.Length), Random.Range(0, materials.Length));
    }
}

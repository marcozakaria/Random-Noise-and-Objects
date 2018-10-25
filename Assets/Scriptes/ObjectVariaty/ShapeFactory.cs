using UnityEngine;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject
{
    [SerializeField]
    Shape[] prefaps;

    public Shape Get ( int shapeID)
    {   
        Shape instance = Instantiate(prefaps[shapeID]);
        instance.ShapeID = shapeID;
        return instance;
    }

    public Shape GetRandom()
    {
        return Get(Random.Range(0, prefaps.Length));
    }
}

using UnityEngine;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject
{
    [SerializeField]
    Shape[] prefaps;

    public Shape Get ( int shapeID)
    {
        return Instantiate(prefaps[shapeID]);
    }

    public Shape GetRandom()
    {
        return Get(Random.Range(0, prefaps.Length));
    }
}

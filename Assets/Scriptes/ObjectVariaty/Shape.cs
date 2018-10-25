using UnityEngine;

public class Shape : PersistableObject
{
    public int ShapeID
    {
        get { return shapeID; }
        set
        {
            if (shapeID == int.MinValue && value != int.MinValue)  // we check if it has its default value at the assignment time   
            {
                shapeID = value;
            }
            else
            {
                Debug.LogError("Not allowed to change shape id");
            }
        }
    }

    public int MaterialID
    {
        get; private set;
    }

    public void SetMaterial(Material material,int matrialID)
    {
        GetComponent<MeshRenderer>().material = material;
        MaterialID = matrialID; 
    }

    int shapeID = int.MinValue;
	
}

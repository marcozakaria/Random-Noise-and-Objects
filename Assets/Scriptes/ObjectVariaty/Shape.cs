using UnityEngine;

public class Shape : PersistableObject
{
    Color color;

    int shapeID = int.MinValue;

    MeshRenderer meshRenderer;
    static int colorPropertyId = Shader.PropertyToID("_Color"); // for materialPropertBlock.setcolor()
    static MaterialPropertyBlock sharedPropertyBlock; // to be made once per mat

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

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
        meshRenderer.material = material;
        MaterialID = matrialID; 
    }

    public void SetColor(Color color)
    {
        this.color = color;
        //meshRenderer.material.color = color;
        if (sharedPropertyBlock == null) 
        {
            sharedPropertyBlock = new MaterialPropertyBlock(); // use material property to improve performance becuse objects usees the same materials but with diffrent colors
        }
        sharedPropertyBlock.SetColor(colorPropertyId, color);
        meshRenderer.SetPropertyBlock(sharedPropertyBlock);
    }

    public override void Save(GameDataWritter writer) // override from persistanceObject Class to add save color functionaliy
    {
        base.Save(writer);
        writer.Write(color);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        SetColor(reader.ReadColor());
    }
}

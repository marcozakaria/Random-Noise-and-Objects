using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour
{
    public Mesh[] meshs;
    public Material material;

    public float childScale;
    [Range(1,7)]
    public int maxDepth;
    public float minWaitForSeconds = 0.1f;
    public float maxWaitForSeconds = 0.5f;
    [Range(0,1)]
    public float spawnPropapilty;

    private int depth;

    private Material[,] materialsArray; // hold two colors to get random one

    private void MaterialIntilization()
    {
        materialsArray = new Material[maxDepth+1,2];
        for (int i = 0; i <= maxDepth; i++)
        {
            float t = i / (maxDepth - 1f);
            materialsArray[i, 0] = new Material(material)
            {
                color = Color.Lerp(Color.white, Color.red, t)
            };
            materialsArray[i, 1] = new Material(material)
            {
                color = Color.Lerp(Color.white, Color.cyan, t)
            };
        }
        materialsArray[maxDepth, 0].color = Color.magenta;
        materialsArray[maxDepth, 1].color = Color.yellow;
    }

    private void Start()
    {
        if (materialsArray == null)
        {
            MaterialIntilization();
        }

        gameObject.AddComponent<MeshFilter>().mesh = meshs[Random.Range(0,meshs.Length)];
        gameObject.AddComponent<MeshRenderer>().material = materialsArray[depth,Random.Range(0,2)];

        if (depth < maxDepth)
        {
            StartCoroutine(CreateChildern());
        }       
    }

    private static Vector3[] childDirections = {
            Vector3.up, Vector3.right, Vector3.left, Vector3.forward , Vector3.back
        };
    private static Quaternion[] childOrintation = {
            Quaternion.identity, Quaternion.Euler(0f, 0f, -90f), Quaternion.Euler(0f, 0f, 90f), Quaternion.Euler(90f, 0f, 0f), Quaternion.Euler(-90f, 0f, 0f)
        };


    private IEnumerator CreateChildern()
    {
        for (int i = 0; i < childDirections.Length; i++)
        {
            if (Random.value < spawnPropapilty)
            {
                yield return new WaitForSeconds(Random.Range(minWaitForSeconds, maxWaitForSeconds));
                new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
            }
        }
        
    }

    private void Initialize(Fractal parent, int index)
    {
        meshs = parent.meshs;
        materialsArray = parent.materialsArray;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;
        transform.parent = parent.transform;
        spawnPropapilty = parent.spawnPropapilty;

        transform.localScale = Vector3.one * childScale;
        transform.localPosition = childDirections[index] * (0.5f * childScale + 0.5f);
        transform.localRotation = childOrintation[index];
    }

}

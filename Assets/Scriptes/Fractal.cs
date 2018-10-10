using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour
{
    public Mesh mesh;
    public Material material;

    public float childScale;
    public int maxDepth;
    public float waitForSeconds;

    private int depth;

    private void Start()
    {
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = material;
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
            yield return new WaitForSeconds(waitForSeconds);
            new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
        }
        
    }

    private void Initialize(Fractal parent, int index)
    {
        mesh = parent.mesh;
        material = parent.material;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;
        transform.parent = parent.transform;

        transform.localScale = Vector3.one * childScale;
        transform.localPosition = childDirections[index] * (0.5f * childScale + 0.5f);
        transform.localRotation = childOrintation[index];
    }

}

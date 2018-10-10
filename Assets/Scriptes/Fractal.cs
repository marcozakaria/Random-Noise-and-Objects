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

    private IEnumerator CreateChildern()
    {
        new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, Vector3.up, Quaternion.identity);
        yield return new WaitForSeconds(waitForSeconds);
        new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, Vector3.right, Quaternion.Euler(0f, 0f, -90f));
        yield return new WaitForSeconds(waitForSeconds);
        new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, Vector3.left, Quaternion.Euler(0f, 0f, 90f));
        yield return new WaitForSeconds(waitForSeconds);
    }

    private void Initialize(Fractal parent, Vector3 direction, Quaternion oriantation)
    {
        mesh = parent.mesh;
        material = parent.material;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;
        transform.parent = parent.transform;

        transform.localScale = Vector3.one * childScale;
        transform.localPosition = direction * (0.5f * childScale + 0.5f);
        transform.localRotation = oriantation;
    }

}

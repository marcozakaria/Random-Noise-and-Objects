using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NucleonSpawner : MonoBehaviour
{
    public float timeBetweenSpawns = 0.05f;
    public float spawnDistance = 15f;
    public Nucleon[] nucleonPrephaps;

    private float timeSinceLastSpawn;

    private void FixedUpdate()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= timeBetweenSpawns)
        {
            timeSinceLastSpawn -= timeBetweenSpawns;
            SpawnNucleon();
        }
    }

    void SpawnNucleon()
    {
        Nucleon prephap = nucleonPrephaps[Random.Range(0, nucleonPrephaps.Length)];
        Nucleon spawn = Instantiate<Nucleon>(prephap);
        spawn.transform.parent = this.transform;
        //spawn.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
        spawn.transform.localPosition = Random.onUnitSphere * spawnDistance;
    }
}

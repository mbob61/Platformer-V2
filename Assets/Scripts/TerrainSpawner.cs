using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner : MonoBehaviour
{
    private GameObject spawnedCube;
    private SpriteRenderer spawnedCubeRenderer;
    public GameObject terrainCube;
    private int height = 12;
    private int width = 5;

    private void Awake()
    {
        createTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void createTerrain()
    {
        for (float i = 0f; i < height; i += 0.5f)
        {
            for (float j = -width; j < width; j += 0.5f)
            {
                spawnedCube = Instantiate(terrainCube, new Vector2(j, -i), Quaternion.identity);
                spawnedCubeRenderer = spawnedCube.GetComponent<SpriteRenderer>();
                if (i < 0.5f)
                {
                    spawnedCubeRenderer.color = new Color(0.2f, 0.9f, 0.2f, 1.0f);
                }
                else
                {
                    spawnedCubeRenderer.color = new Color(0.9f, 0.2f, 0.2f, 1.0f);
                }
            }
        }
    }
}

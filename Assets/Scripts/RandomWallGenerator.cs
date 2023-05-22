using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWallGenerator : MonoBehaviour
{
    public GameObject cubePrefab;
    public int width = 10;
    public int height = 10;
    public int maxHeight = 5;

    private void Start()
    {
        GenerateWall();
    }

    private void GenerateWall()
    {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int cubeHeight = Random.Range(1, maxHeight + 1);
                for (int z = 0; z < cubeHeight; z++) {
                    GameObject cube = Instantiate(cubePrefab, transform.position + new Vector3(x, z, y), Quaternion.identity);
                    cube.transform.SetParent(transform);
                }
            }
        }
    }
}

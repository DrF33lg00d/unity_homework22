using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class SpawnBalls : MonoBehaviour
{

    public GameObject myTest;

    // Start is called before the first frame update
    void Start()
    {
        for (int y = 1; y < 11; y++)
        {
            for (int x = -5; x < 5; x++)
            {
                for (int z = -5; z < 5; z++)
                {
                    Instantiate(myTest, new Vector3(x, y, z), Quaternion.identity);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

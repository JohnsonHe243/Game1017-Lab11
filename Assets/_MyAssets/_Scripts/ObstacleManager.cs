using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Make this whole script. :)
public class ObstacleManager : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] List<GameObject> obstacles;
    [SerializeField] float moveSpeed;
    [SerializeField] Sprite[] sprites;

    private int obsCtr = 0; // Used to manage the gaps between obstacles.
    void Start()
    {
        obstacles = new List<GameObject>(); 
        for (int i = 0; i < 9; i++)
        {
            GameObject obsInst = GameObject.Instantiate(obstaclePrefab, new Vector3(i * 4f, -16f, 0f), Quaternion.identity);
            obsInst.transform.parent = transform;
            obstacles.Add(obsInst);
        }
        // Start the InvokeRepeating method.
        InvokeRepeating("MoveObstacles", 0f, Time.fixedDeltaTime);
    }
    private void MoveObstacles()
    {
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.transform.Translate(moveSpeed * Time.fixedDeltaTime, 0f, 0f);
        }
        if (obstacles[0].transform.position.x <= -4f)
        {
            // Remove the first obstacle.
            Destroy(obstacles[0]);
            obstacles.RemoveAt(0);
            // Push a new obstacle at the end.
            GameObject obsInst = GameObject.Instantiate(obstaclePrefab, new Vector3(32f, -16f, 0f), Quaternion.identity);
            if (obsCtr++ % 3 == 0)
            {
                obsInst.GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
                obsInst.AddComponent<BoxCollider2D>();
            }
            obsInst.transform.parent = transform;
            obsInst.layer = 3; // So we can land on obstacles for this lab.
            obstacles.Add(obsInst);
        }
    }
}

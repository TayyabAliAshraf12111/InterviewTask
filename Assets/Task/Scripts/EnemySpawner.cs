using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{


    public Vector3 moveset;
    public GameObject Enemy;
    public float Time = 5;
    public float Range = 5;

    Transform Player;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SpawnEnemyWithDelay());
    }
    // Update is called once per frame
    private IEnumerator SpawnEnemyWithDelay()
    {
        while (true)
        {
            yield return new WaitUntil(() => Vector3.Distance(transform.position, Player.position) < Range);
            yield return new WaitForSeconds(Time);
            Instantiate(Enemy, transform.position + moveset, Quaternion.identity);
        }
    }
}

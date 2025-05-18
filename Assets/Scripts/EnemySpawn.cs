using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("Spawn Enemigos")]

    [Tooltip("Prefabs de enemigos")]
    [SerializeField] private GameObject[] _enemiesPrefab;
    [Tooltip("Numero de enemigos que van a spawnear")]
    [SerializeField] private int _enemiesToSpawn;
    [SerializeField] private Transform[] _spawnPoint;
    private int _enemyIndex;


    private BoxCollider2D _collider;

    void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if(_enemiesToSpawn <= 0)
        {
            CancelInvoke();
        }*/
    }

    IEnumerator SpawnEnemy()
    {   
        for (int i = 0; i < _enemiesToSpawn; i++)
        {
            foreach (Transform spawn in _spawnPoint)
            {
                _enemyIndex = Random.Range(0, _enemiesPrefab.Length);
                Instantiate(_enemiesPrefab[_enemyIndex], spawn.position, spawn.rotation);

                yield return new WaitForSeconds(1);
            }

            yield return new WaitForSeconds(1);
        } 
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            _collider.enabled = false;
            //InvokeRepeating("SpawnEnemy", 0, 2);
            StartCoroutine(SpawnEnemy());
        }
    }
}

using Assets.Scripts.Enemies.Interfañes;
using Assets.Scripts.Enemies.Parts;
using Assets.Scripts.Interfañes;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour, IPoolOwner
{
  [Header("Spawn Settings")]
  [SerializeField] private Enemy enemyPrefab;
  [SerializeField] private int initialPoolSize = 20;
  [SerializeField] private float spawnInterval = 2f;

  private Stack<IPooledObject> pool;
  private float spawnTimer;

  private void Start()
  {
    pool = new Stack<IPooledObject>();

    for (int i = 0; i < initialPoolSize; i++)
    {
      var go = Instantiate(enemyPrefab, transform.position, Quaternion.identity).gameObject;
      go.SetActive(false);

      if (go.TryGetComponent<IPooledObject>(out var pooled))
      {
        pooled.Init(this);
        pool.Push(pooled);
      }
      else
      {
        Debug.LogError("Enemy Prefab íå ñîäåðæèò IPooledObject!");
        Destroy(go);
      }
    }
  }

  private void Update()
  {
    spawnTimer += Time.deltaTime;
    if (spawnTimer < spawnInterval) return;
    spawnTimer = 0f;
    SpawnEnemy();
  }

  private void SpawnEnemy()
  {
    if (pool.Count == 0) return; // ïóë èñ÷åðïàí, æä¸ì âîçâðàòà

    var pooled = pool.Pop();
    var go = (pooled as MonoBehaviour).gameObject;

    go.transform.position = transform.position;
    pooled.OnSpawn();
  }

  // IPoolOwner
  public void ReturnToPool(IPooledObject obj)
  {
    obj.OnReturnedToPool();
    pool.Push(obj);
  }
}
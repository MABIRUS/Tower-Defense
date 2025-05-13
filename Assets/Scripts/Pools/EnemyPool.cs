using System.Collections.Generic;
using Assets.Scripts.Enemies.Parts;
using Assets.Scripts.Enemies.Parts.Enums;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
  public static EnemyPool Instance { get; private set; }

  [Header("Префабы врагов")]
  public GameObject trainingPrefab;
  public GameObject basicPrefab;
  public GameObject armoredPrefab;
  public GameObject swarmPrefab;

  [Header("Настройки пуллов")]
  [SerializeField] private int trainingPoolCapacity;
  [SerializeField] private int basicPoolCapacity;
  [SerializeField] private int armoredPoolCapacity;
  [SerializeField] private int swarmPoolCapacity;
  [SerializeField] private int maximumPoolSize = 100;

  private ObjectPool<Enemy> trainingPool;
  private ObjectPool<Enemy> basicPool;
  private ObjectPool<Enemy> armoredPool;
  private ObjectPool<Enemy> swarmPool;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;

      trainingPool = CreatePool(trainingPrefab, trainingPoolCapacity);
      basicPool = CreatePool(basicPrefab, basicPoolCapacity);
      armoredPool = CreatePool(armoredPrefab, armoredPoolCapacity);
      swarmPool = CreatePool(swarmPrefab, swarmPoolCapacity);

      Prewarm(trainingPrefab, trainingPool, trainingPoolCapacity);
      Prewarm(basicPrefab, basicPool, basicPoolCapacity);
      Prewarm(armoredPrefab, armoredPool, armoredPoolCapacity);
      Prewarm(swarmPrefab, swarmPool, swarmPoolCapacity);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  private ObjectPool<Enemy> CreatePool(GameObject prefab, int poolCapacity)
  {
    return new ObjectPool<Enemy>(
        createFunc: () =>
        {
          GameObject go = Instantiate(prefab);
          go.SetActive(false);
          return go.GetComponent<Enemy>();
        },
        actionOnGet: enemy =>
        {
          enemy.gameObject.SetActive(true);
          enemy.Initialize();
        },
        actionOnRelease: enemy =>
        {
          enemy.gameObject.SetActive(false);
        },
        actionOnDestroy: enemy =>
        {
          Destroy(enemy.gameObject);
        },
        defaultCapacity: poolCapacity,
        maxSize: maximumPoolSize
    );
  }


  private void Prewarm(GameObject prefab, ObjectPool<Enemy> pool, int capacity)
  {
    for (var i = 0; i < capacity; i++)
    {
      GameObject go = Instantiate(prefab);
      var enemy = go.GetComponent<Enemy>();
      pool.Release(enemy);
    }
  }

  public Enemy SpawnEnemy(EnemyType type, Vector2 position)
  {
    var pool = Get(type);
    Enemy enemy = pool.Get();
    enemy.transform.position = position;
    return enemy;
  }

  public List<Enemy> SpawnMultipleEnemies(EnemyType type, int count, Vector2 center, float radius)
  {
    var result = new List<Enemy>(count);

    for (var i = 0; i < count; i++)
    {
      Vector2 offset = Random.insideUnitSphere * radius;
      var enemy = SpawnEnemy(type, center + offset);
      if (enemy != null) result.Add(enemy);
    }
    return result;
  }

  public void ReturnEnemyToPool(Enemy enemy)
  {
    if (enemy == null) return;

    var pool = Get(enemy.EnemyType);

    if (pool != null)
      pool.Release(enemy);
    else
      Destroy(enemy.gameObject);
  }

  private ObjectPool<Enemy> Get(EnemyType type)
  {
    return type switch
    {
      EnemyType.Training => trainingPool,
      EnemyType.Basic => basicPool,
      EnemyType.Armored => armoredPool,
      EnemyType.Swarm => swarmPool,
      _ => null
    };
  }
}

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Enemies.Parts.Enums;
using Assets.Scripts.Managers.Parts;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
  public static WaveManager Instance { get; private set; }

  [Header("Настройки волн")]
  [SerializeField] private List<WaveData> waves;
  private int currentWaveIndex = 0;

  [Header("Точки спавна")]
  [SerializeField] private GameObject spawnPointsContainer;
  private Transform[] spawnPoints;

  [Header("Радиус появления врагов")]
  [SerializeField] private float packSpawnRadius = 1f;

  [Header("Ссылки на ScriptableObject врагов")]
  [SerializeField] private EnemySO trainingSO;
  [SerializeField] private EnemySO basicSO;
  [SerializeField] private EnemySO armoredSO;
  [SerializeField] private EnemySO swarmSO;

  private Dictionary<EnemyType, EnemySO> soMap;

  private void Awake()
  {
    Instance = this;

    soMap = new Dictionary<EnemyType, EnemySO>
      {
        { EnemyType.Training, trainingSO },
        { EnemyType.Basic,    basicSO    },
        { EnemyType.Armored,  armoredSO  },
        { EnemyType.Swarm,    swarmSO    },
      };

    spawnPoints = spawnPointsContainer
            .GetComponentsInChildren<Transform>()
            .Where(transform => transform != spawnPointsContainer.transform)
            .ToArray();
  }

  public void StartNextWave()
  {
    if (currentWaveIndex < waves.Count)
    {
      StartWave(waves[currentWaveIndex]);
      currentWaveIndex++;
    }
    else
    {
      Debug.Log("Все волны завершены!");
    }
  }

  private void StartWave(WaveData wave)
  {
    List<EnemyType> packs = GenerateRandomCombination(
        wave.waveDifficulty,
        wave.minPackDifficulty,
        wave.maxPackDifficulty
    );

    foreach (var type in packs)
    {
      Vector2 point = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
      var packSize = soMap[type].packSize;
      EnemyPool.Instance.SpawnMultipleEnemies(type, packSize, point, packSpawnRadius);
    }
  }

  private List<EnemyType> GenerateRandomCombination(int totalDifficulty, int minPD, int maxPD)
  {
    var result = new List<EnemyType>();
    var remaining = totalDifficulty;

    var allowed = soMap
        .Where(kv => kv.Value.packDifficulty >= minPD && kv.Value.packDifficulty <= maxPD)
        .Select(kv => kv.Key)
        .ToList();

    while (remaining > 0)
    {
      var choices = allowed
          .Where(t => soMap[t].packDifficulty <= remaining)
          .ToList();

      if (choices.Count == 0)
      {
        break;
      }

      var pick = choices[Random.Range(0, choices.Count)];
      result.Add(pick);
      remaining -= soMap[pick].packDifficulty;
    }

    return result;
  }
}

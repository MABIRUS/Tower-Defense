using UnityEngine;

namespace Assets.Scripts.Managers.Parts
{
  [System.Serializable]
  public class WaveData
  {
    [Header("Диапазон сложности пачки")]
    public int minPackDifficulty;
    public int maxPackDifficulty;
    [Header("Суммарная сложность волны")]
    public int waveDifficulty;
  }
}

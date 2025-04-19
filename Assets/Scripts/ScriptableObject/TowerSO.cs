using UnityEngine;

[CreateAssetMenu(fileName = "TowerSO", menuName = "Scriptable Objects/TowerSO")]
public class TowerSO : ScriptableObject
{
  [SerializeField] public float attackRange;
  [SerializeField] public float attackSpeed;
  [SerializeField] public float damage;
  [SerializeField] public float maxTargets;
  [SerializeField] public TowerTypes towerType;
}

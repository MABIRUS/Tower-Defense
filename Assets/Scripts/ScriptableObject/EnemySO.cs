using Assets.Scripts.Enemies.Parts.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
  [SerializeField] public float health;
  [SerializeField] public float armor;
  [SerializeField] public float speed;
  [SerializeField] public float attackRange;
  [SerializeField] public float attackSpeed;
  [SerializeField] public float damage;
  [SerializeField] public EnemyType enemyType;
  [SerializeField] public int reward;
}

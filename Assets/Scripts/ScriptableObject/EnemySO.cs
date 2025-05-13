using Assets.Scripts.Enemies.Parts.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
  public float health;
  public float armor;
  public float speed;
  public float attackRange;
  public float attackSpeed;
  public float damage;
  public EnemyType enemyType;
  public int reward;
  public int packSize;
  public int packDifficulty;
}
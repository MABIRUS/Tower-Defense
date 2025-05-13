using Assets.Scripts.Enemies.Interfaсes;
using Assets.Scripts.Enemies.Parts.Enums;
using UnityEngine;

namespace Assets.Scripts.Enemies.Parts
{
  public class Enemy : MonoBehaviour, IAttackable, IMoveable, IDestroyable
  {
    [SerializeField] private EnemySO enemyScriptableObject;

    private float currentHealth;
    private float currentArmor;
    private float currentSpeed;
    private float currentAttackSpeed;
    private float currentDamage;
    private float currentReward;

    private float attackRange;
    private EnemyType enemyType;

    private float lastAttackTime;
    private static Vector2 playerBasePosition;
    private static PlayerBase playerBase;

    private void Start()
    {
      playerBase = LevelManager.Instance.playerBase;
      playerBasePosition = playerBase.transform.position;
    }

    private void FixedUpdate()
    {
      var distanceToBase = Vector2.Distance(transform.position, playerBasePosition);

      if (distanceToBase <= attackRange)
      {
        if (Time.time - lastAttackTime >= 1f / currentAttackSpeed)
        {
          Attack();
          lastAttackTime = Time.time;
        }
      }
      else
      {
        Move(playerBasePosition);
      }
    }

    public float Health => currentHealth;
    public float Armor => currentArmor;
    public float Speed => currentSpeed;
    public float AttackRange => attackRange;
    public float AttackSpeed => currentAttackSpeed;
    public float Damage => currentDamage;
    public EnemyType EnemyType => enemyType;

    public void Attack(Collider2D target = null)
    {
      playerBase.TakeDamage(Damage);
    }

    public void Die()
    {
      playerBase.ChangeMoney(currentReward);
      EnemyPool.Instance.ReturnEnemyToPool(this);
    }

    public void Move(Vector2 targetPosition)
    {
      transform.position = Vector2.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
      currentHealth -= damage * (1f - Armor / 100f);
      if (currentHealth <= 0)
      {
        Die();
      }
    }

    public void Initialize()
    {
      currentHealth = enemyScriptableObject.health;
      currentArmor = enemyScriptableObject.armor;
      currentSpeed = enemyScriptableObject.speed;
      currentAttackSpeed = enemyScriptableObject.attackSpeed;
      currentDamage = enemyScriptableObject.damage;
      attackRange = enemyScriptableObject.attackRange;
      currentReward = enemyScriptableObject.reward;
      enemyType = enemyScriptableObject.enemyType;
    }
  }
}

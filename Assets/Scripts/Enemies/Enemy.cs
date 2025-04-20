using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Assets.Scripts.Enemies.Interfaсes;
using Assets.Scripts.Enemies.Parts.Enums;
using Assets.Scripts.Interfaсes;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Scripts.Enemies.Parts
{
  public class Enemy : MonoBehaviour, IAttackable, IMoveable, IDestroyable, IPooledObject
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

    private EnemyState enemyState = EnemyState.Idle;
    private float lastAttackTime = 0f;
    private static Vector2 playerBasePosition;
    private static PlayerBase playerBase;
    private IPoolOwner poolOwner;

    private readonly List<(IEffect effect, float endTime)> activeEffects = new();

    private void Start()
    {
      playerBase = LevelManager.main.playerBase;
      playerBasePosition = playerBase.transform.position;
      currentHealth = enemyScriptableObject.health;
      currentArmor = enemyScriptableObject.armor;
      currentSpeed = enemyScriptableObject.speed;
      currentAttackSpeed = enemyScriptableObject.attackSpeed;
      currentDamage = enemyScriptableObject.damage;
      attackRange = enemyScriptableObject.attackRange;
      currentReward = enemyScriptableObject.reward;
      enemyType = enemyScriptableObject.enemyType;
    }

    private void Update()
    {
      // Обработка эффектов
      for (int i = activeEffects.Count - 1; i >= 0; i--)
      {
        var (effect, endTime) = activeEffects[i];
        if (endTime < Time.time)
        {
          effect.RemoveEffect(this);
          activeEffects.RemoveAt(i);
        }
        else
        {
          effect.ApplyEffect(this);
        }
      }
    }

    private void FixedUpdate()
    {
      // Вычисляем расстояние до базы, используя текущее положение объекта
      var distanceToBase = Vector2.Distance(transform.position, playerBasePosition);

      if (distanceToBase <= attackRange)
      {
        // Если база в зоне атаки и прошло достаточно времени с предыдущей атаки
        if (Time.time - lastAttackTime >= 1f / currentAttackSpeed)
        {
          Attack();
          lastAttackTime = Time.time;
        }
      }
      else
      {
        // Перемещаемся в сторону базы
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
    public EnemyState EnemyState => enemyState;

    public void Attack(Collider2D target = null)
    {
      playerBase.TakeDamage(Damage);
    }
    public void Die()
    {
      //ToDo: Поигрывается анимация смерти и дает деньги
      playerBase.ChangeMoney(currentReward);
      OnReturnedToPool();
    }

    public void Move(Vector2 targetPosition)
    {
      transform.position = Vector2.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
    }

    public void ApplyEffect(IEffect effect)
    {
      activeEffects.Add((effect, Time.time + effect.Duration));
      effect.ApplyEffect(this);
    }

    public void RemoveEffect(IEffect effect)
    {
      effect.RemoveEffect(this);
      activeEffects.RemoveAll(e => e.effect == effect);
    }

    public void TakeDamage(float damage)
    {
      currentHealth -= damage;
      if (currentHealth <= 0)
      {
        Die();
      }
    }

    public void SetState(EnemyState state) => enemyState = state;
    public void ModifyArmor(float multiplier) => currentArmor *= multiplier;
    public void ModifySpeed(float multiplier) => currentSpeed *= multiplier;
    public void ModifyDamage(float multiplier) => currentDamage *= multiplier;
    public void ModifyAttackSpeed(float multiplier) => currentAttackSpeed *= multiplier;

    public void Init(IPoolOwner poolOwner)
    {
      this.poolOwner = poolOwner;
      activeEffects.Clear();

      currentHealth = enemyScriptableObject.health;
      currentArmor = enemyScriptableObject.armor;
      currentSpeed = enemyScriptableObject.speed;
      currentAttackSpeed = enemyScriptableObject.attackSpeed;
      currentDamage = enemyScriptableObject.damage;
    }

    public void OnSpawn()
    {
      gameObject.SetActive(true);
    }

    public void OnReturnedToPool()
    {
      gameObject.SetActive(false);

      if (poolOwner is MonoBehaviour mb)
      {
        transform.position = mb.transform.position;
      }
    }
  }
}

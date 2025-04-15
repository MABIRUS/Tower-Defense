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
    [SerializeField] private float health;
    [SerializeField] private float armor;
    [SerializeField] private float speed;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float damage;
    [SerializeField] private EnemyType enemyType;

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
        if (Time.time - lastAttackTime >= 1f / attackSpeed)
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

    public float Health => health;
    public float Armor => armor;
    public float Speed => speed;
    public float AttackRange => attackRange;
    public float AttackSpeed => attackSpeed;
    public float Damage => damage;
    public EnemyType EnemyType => enemyType;
    public EnemyState EnemyState => enemyState;

    public void Attack(Collider2D target = null)
    {
      playerBase.TakeDamage(Damage);
    }
    public void Die()
    {
      speed = 0;
      Destroy(gameObject);
    }

    public void Move(Vector2 targetPosition)
    {
      transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
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
      health -= damage;
      if (health <= 0)
      {
        Die();
      }
    }

    public void SetState(EnemyState state) => enemyState = state;
    public void ModifyArmor(float multiplier) => armor *= multiplier;
    public void ModifySpeed(float multiplier) => speed *= multiplier;
    public void ModifyDamage(float multiplier) => damage *= multiplier;
    public void ModifyAttackSpeed(float multiplier) => attackSpeed *= multiplier;
    public void SetEnemyType(EnemyType type) => enemyType = type;

    public void Init(IPoolOwner poolOwner)
    {
      this.poolOwner = poolOwner;
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

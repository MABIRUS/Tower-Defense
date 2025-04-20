using System;
using Assets.Scripts.Enemies.Interfañes;
using UnityEngine;

public class PlayerBase : MonoBehaviour, IDestroyable
{

  [SerializeField] private float money;
  private float health = 100;

  public event Action<float> OnHealthChanged;
  public event Action<float> OnMoneyChanged;

  public float Money => money;
  public float Health => health;

  public void TakeDamage(float damage)
  {
    health -= damage;

    OnHealthChanged?.Invoke(health);

    if (health <= 0)
    {
      Die();
    }
  }

  public void ChangeMoney(float delta)
  {
    money += delta;
    OnMoneyChanged?.Invoke(money);
  }

  public void Die()
  {
    Debug.Log("Base Destoryer");
    Destroy(gameObject);
  }
}
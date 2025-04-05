using System;
using Assets.Scripts.Enemies.Interfañes;
using UnityEngine;

public class PlayerBase : MonoBehaviour, IDestroyable
{

  [SerializeField] private float health;

  public event Action<float> OnHealthChanged;

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

  public void Die()
  {
    Debug.Log("Base Destoryer");
    Destroy(gameObject);
  }

}

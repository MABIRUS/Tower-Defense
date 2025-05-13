using System;
using Assets.Scripts.Enemies.Interfañes;
using UnityEngine;

public class PlayerBase : MonoBehaviour, IDestroyable
{

  [SerializeField] private float money;
  [SerializeField] private float health;

  [SerializeField] private SpriteRenderer SpriteRenderer;
  [SerializeField] private Sprite FullBase;
  [SerializeField] private Sprite DestroyedBase;

  private bool destroyed = false;

  public event Action<float> OnHealthChanged;
  public event Action<float> OnMoneyChanged;

  public float Money => money;
  public float Health => health;

  private void Start()
  {
    SpriteRenderer.sprite = FullBase;
  }

  public void TakeDamage(float damage)
  {
    if(destroyed) return;

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
    SpriteRenderer.sprite = DestroyedBase;
    destroyed = true;
  }
}
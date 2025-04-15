using Assets.Scripts.Enemies.Interfaсes;
using Assets.Scripts.Enemies.Parts;
using UnityEngine;

public class Tower : MonoBehaviour, IAttackable
{
  [SerializeField] private float attackRange;
  [SerializeField] private float attackSpeed;
  [SerializeField] private float damage;

  private CircleCollider2D attackRangeCollider;

  private void Start()
  {
    attackRangeCollider = this.GetComponent<CircleCollider2D>();
  }

  public float AttackRange => attackRange;

  public float AttackSpeed => attackSpeed;

  public float Damage => damage;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.CompareTag("Enemy"))
    {
      Attack(collision);
    }

  }

  public void Attack(Collider2D target = null)
  {
    Debug.Log("Tower Attack");
    if (target != null)
    {
      // Пытаемся получить компонент Enemy с игрового объекта
      var enemy = target.GetComponent<Enemy>();
      if (enemy != null)
      {
        enemy.TakeDamage(Damage);
      }
    }
  }
}
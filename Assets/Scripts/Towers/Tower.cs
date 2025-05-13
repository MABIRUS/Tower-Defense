using System.Collections;
using Assets.Scripts.Enemies.Interfañes;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Tower : MonoBehaviour, IAttackable
{
  [SerializeField] private TowerSO towerScriptableObject;

  private float currentAttackRange;
  private float currentAttackSpeed;
  private float currentDamage;
  private float lastAttackTime;
  private CircleCollider2D attackRangeCollider;

  public Platform CurrentPlatform { get; set; }
  public bool IsMoving { get; private set; }

  public void StartMove(Platform target, float speed)
  {
    if (IsMoving) return;
    IsMoving = true;

    if (CurrentPlatform != null)
      CurrentPlatform.OccupiedTower = null;

    target.OccupiedTower = this;
    CurrentPlatform = target;

    StartCoroutine(MoveRoutine(target.transform.position, speed));
  }

  private IEnumerator MoveRoutine(Vector3 dest, float speed)
  {
    while ((transform.position - dest).sqrMagnitude > 0.0001f)
    {
      transform.position = Vector3.MoveTowards(transform.position, dest, speed * Time.deltaTime);
      yield return null;
    }
    transform.position = dest;
    IsMoving = false;
  }

  private void Start()
  {
    currentAttackRange = towerScriptableObject.attackRange;
    currentAttackSpeed = towerScriptableObject.attackSpeed;
    currentDamage = towerScriptableObject.damage;

    attackRangeCollider = GetComponent<CircleCollider2D>();
    attackRangeCollider.radius = currentAttackRange;
  }
  public float AttackRange => currentAttackRange;
  public float AttackSpeed => currentAttackSpeed;
  public float Damage => currentDamage;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Enemy"))
      Attack(collision);
  }

  private void OnTriggerStay2D(Collider2D collision)
  {
    if (collision.CompareTag("Enemy"))
      Attack(collision);
  }

  public void Attack(Collider2D target = null)
  {
    if (IsMoving) return;
    if (Time.time - lastAttackTime < 1f / currentAttackSpeed)
      return;

    lastAttackTime = Time.time;

    var bullet = BulletPool.Instance.Get(transform);
    bullet.SetParameters(target.transform, currentDamage);
  }
}

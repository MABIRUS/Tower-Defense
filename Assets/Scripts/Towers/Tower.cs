using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enemies.Interfa�es;
using Assets.Scripts.Interfa�es;
using Assets.Scripts.Towers.Parts;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Tower : MonoBehaviour, IAttackable, IPoolOwner
{
  [SerializeField] private TowerSO towerScriptableObject;

  private float currentAttackRange;
  private float currentAttackSpeed;
  private float currentDamage;

  [Header("Projectile Pool")]
  [Tooltip("Prefab ����, ������ ��������� ��������� Bullet")]
  [SerializeField] private GameObject bulletPrefab;
  [Tooltip("������ ���� ����")]
  [SerializeField] private int poolSize = 5;

  private Queue<Bullet> bulletPool;
  private CircleCollider2D attackRangeCollider;
  private float lastAttackTime;

  // ��� ���������, �� ������� ������ ����� �����
  public Platform CurrentPlatform { get; set; }
  // ���������� �������� �� ����� �����������
  public bool IsMoving { get; private set; }

  public void StartMove(Platform target, float speed)
  {
    if (IsMoving) return;
    IsMoving = true;

    // �������� ����� ���������, ����������� ������
    if (CurrentPlatform != null) CurrentPlatform.OccupiedTower = null;
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
    // ��������� ��������� �� ScriptableObject
    currentAttackRange = towerScriptableObject.attackRange;
    currentAttackSpeed = towerScriptableObject.attackSpeed;
    currentDamage = towerScriptableObject.damage;

    // ����������� �������-��������� ��� ����������� ������
    attackRangeCollider = GetComponent<CircleCollider2D>();
    attackRangeCollider.radius = currentAttackRange;

    // ������������� ����
    bulletPool = new Queue<Bullet>(poolSize);
    for (int i = 0; i < poolSize; i++)
    {
      var obj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
      obj.SetActive(false);

      if (obj.TryGetComponent<Bullet>(out var bullet))
      {
        bullet.Init(this);
        bulletPool.Enqueue(bullet);
      }
      else
      {
        Debug.LogError("Bullet Prefab �� �������� ��������� Bullet!");
        Destroy(obj);
      }
    }
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

    if (target == null)
      return;

    // ��������� �������� ����� ����������
    if (Time.time - lastAttackTime < 1f / currentAttackSpeed) 
      return;

    // ���� ���� �� ����, ���� ���� ���������
    if (bulletPool.Count == 0)
      return;

    lastAttackTime = Time.time;

    var bullet = bulletPool.Dequeue();
    // ����� ��������� ����: ����, ���� � �������� �����
    bullet.SetParameters(target.transform, currentDamage);
    // ������ ���� � ������� ����� � ����������
    bullet.transform.position = transform.position;
    bullet.OnSpawn();
  }

  // ���������� ����� ��� ��������� ��� ������ ����
  public void ReturnToPool(IPooledObject obj)
  {
    obj.OnReturnedToPool();
    bulletPool.Enqueue(obj as Bullet);
  }
}

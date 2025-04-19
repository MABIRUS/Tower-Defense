using Assets.Scripts.Enemies.Interfaсes;
using Assets.Scripts.Enemies.Parts;
using Assets.Scripts.Interfaсes;
using UnityEngine;

namespace Assets.Scripts.Towers.Parts
{
  public class Bullet : MonoBehaviour, IPooledObject
  {
    private Tower poolOwner;
    private Transform target;
    private float damage;
    private float speed = 20;

    public void Init(IPoolOwner poolOwner)
    {
      this.poolOwner = poolOwner as Tower;
    }

    public void OnReturnedToPool()
    {
      target = null;
      gameObject.SetActive(false);
    }

    public void OnSpawn()
    {
      gameObject.SetActive(true);
    }

    public void SetParameters(Transform target, float damage)
    {
      this.target = target;
      this.damage = damage;
    }

    private void Update()
    {
      if (target == null)
      {
        ReturnToPool();
        return;
      }

      // Летим к цели
      var dir = (target.position - transform.position).normalized;
      var step = speed * Time.deltaTime;

      if (Vector3.Distance(transform.position, target.position) <= step)
      {
        Hit();
        return;
      }

      transform.Translate(dir * step, Space.World);
    }

    private void Hit()
    {
      if (target.TryGetComponent<Enemy>(out var enemy))
        enemy.TakeDamage(damage);

      ReturnToPool();
    }

    private void ReturnToPool()
    {
      // Сообщаем башне, что можем вернуться в пул
      poolOwner?.ReturnToPool(this);
    }

  }
}

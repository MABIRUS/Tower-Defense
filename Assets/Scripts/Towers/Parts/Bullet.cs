using Assets.Scripts.Enemies.Parts;
using UnityEngine;

namespace Assets.Scripts.Towers.Parts
{
  public class Bullet : MonoBehaviour
  {
    private Transform target;
    private float damage;
    private readonly float speed = 15;

    public void OnReturnedToPool()
    {
      target = null;
      gameObject.SetActive(false);
    }

    public void SetParameters(Transform target, float damage)
    {
      this.target = target;
      this.damage = damage;
    }

    private void FixedUpdate()
    {
      if (target == null || !target.gameObject.activeInHierarchy)
      {
        BulletPool.Instance.Release(this);
        return;
      }

      Vector3 dir = (target.position - transform.position).normalized;
      var step = speed * Time.deltaTime;

      if ((target.position - transform.position).sqrMagnitude <= step * step)
      {
        if (target.TryGetComponent<Enemy>(out var enemy))
          enemy.TakeDamage(damage);

        BulletPool.Instance.Release(this);
        return;
      }

      transform.Translate(dir * step, Space.World);
    }
  }
}

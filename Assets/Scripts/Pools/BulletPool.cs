using Assets.Scripts.Towers.Parts;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
  public static BulletPool Instance { get; private set; }

  [Header("Префабы пуль")]
  [SerializeField] private GameObject bulletPrefab;

  [Header("Настройки пуллов")]
  [SerializeField] private int initialPoolSize;
  [SerializeField] private int maxPoolSize;

  private ObjectPool<Bullet> pool;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      pool = new ObjectPool<Bullet>(
          createFunc: () =>
          {
            var go = Instantiate(bulletPrefab);
            go.SetActive(false);
            return go.GetComponent<Bullet>();
          },
          actionOnGet: bullet => 
          {
            bullet.gameObject.SetActive(true);
          },
          actionOnRelease: bullet =>
          {
            bullet.OnReturnedToPool();
          },
          actionOnDestroy: bullet =>
          {
            Destroy(bullet.gameObject);
          },
          defaultCapacity: initialPoolSize,
          maxSize: maxPoolSize
      );
      Prewarm(initialPoolSize);
    }
    else Destroy(gameObject);
  }

  private void Prewarm(int count)
  {
    for (var i = 0; i < count; i++)
    {
      var b = pool.Get();
      pool.Release(b);
    }
  }

  public Bullet Get(Transform origin)
  {
    var bullet = pool.Get();
    bullet.transform.position = origin.position;
    return bullet;
  }

  public void Release(Bullet bullet)
  {
    pool.Release(bullet);
  }
}

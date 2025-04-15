using Assets.Scripts.Interfañes;

namespace Assets.Scripts.Enemies.Interfañes
{
  public interface IPooledObject
  {
    void Init(IPoolOwner poolOwner);
    void OnSpawn();
    void OnReturnedToPool();
  }
}
using Assets.Scripts.Interfa�es;

namespace Assets.Scripts.Enemies.Interfa�es
{
  public interface IPooledObject
  {
    void Init(IPoolOwner poolOwner);
    void OnSpawn();
    void OnReturnedToPool();
  }
}
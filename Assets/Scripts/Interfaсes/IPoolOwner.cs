using Assets.Scripts.Enemies.Interfaсes;
using UnityEngine;

namespace Assets.Scripts.Interfaсes
{
  public interface IPoolOwner
  {
    void ReturnToPool(IPooledObject obj);
  }
}

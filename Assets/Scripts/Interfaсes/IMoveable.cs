using UnityEngine;

namespace Assets.Scripts.Enemies.Interfañes
{
  public interface IMoveable
  {
    float Speed { get; }
    void Move(Vector2 direction);
  }
};

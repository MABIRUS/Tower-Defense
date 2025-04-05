using UnityEngine;

namespace Assets.Scripts.Enemies.Interfa�es
{
  public interface IMoveable
  {
    float Speed { get; }
    void Move(Vector2 direction);
  }
};

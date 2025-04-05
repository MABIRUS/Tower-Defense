using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Enemies.Parts;

namespace Assets.Scripts.Interfaсes
{
  public interface IEffect
  {
    string Name { get; }
    int MaxStacks { get; }
    float Duration { get; }

    void ApplyEffect(Enemy enemy);
    void RemoveEffect(Enemy enemy);
  }
}

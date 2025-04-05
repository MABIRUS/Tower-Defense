using Assets.Scripts.Enemies.Parts;
using Assets.Scripts.Interfaсes;

namespace Assets.Scripts
{
  class FireEffect : IEffect
  {
    public string Name => "Fire";
    public int MaxStacks => 1;
    public float Duration => 5f;
    public int Damage => 10;
    public void ApplyEffect(Enemy enemy)
    {
      enemy.TakeDamage(Damage);
    }
    public void RemoveEffect(Enemy enemy)
    {
    }
  }
}

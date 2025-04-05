namespace Assets.Scripts.Enemies.Interfaсes
{
  public interface IAttackable
  {
    float AttackRange { get; }
    float AttackSpeed { get; }
    float Damage { get; }
    void Attack();
  }
}

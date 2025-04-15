namespace Assets.Scripts.Enemies.Interfaсes
{
  public interface IDestroyable
  {
    float Health { get; }

    void TakeDamage(float damage);
    void Die();
  }
}

namespace Assets.Scripts.Enemies.Interfaсes
{
  public interface IDestroyable
  {
    float Health { get; }

    public void TakeDamage(float damage);
    void Die();
  }
}

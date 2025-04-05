using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
  public static LevelManager main;

  public PlayerBase playerBase;
  public Text healthText;

  private void Awake()
  {
    main = this;
    healthText.text = playerBase.Health.ToString();
  }

  private void OnEnable()
  {
    if (playerBase != null)
    {
      playerBase.OnHealthChanged += OnBaseHealthChanged;
    }
  }

  private void OnDisable()
  {
    if (playerBase != null)
    {
      playerBase.OnHealthChanged -= OnBaseHealthChanged;
    }
  }

  private void OnBaseHealthChanged(float newHealth)
  {
    if (healthText != null)
    {
      healthText.text = $"{newHealth}";
    }
  }

}
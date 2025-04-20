using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
  [SerializeField] private Text healthText;
  [SerializeField] private Text moneyText;

  public static LevelManager main;

  public PlayerBase playerBase;
  

  private void Awake()
  {
    main = this;
    healthText.text = playerBase.Health.ToString();
    moneyText.text = playerBase.Money.ToString();
  }

  private void OnEnable()
  {
    if (playerBase != null)
    {
      playerBase.OnHealthChanged += OnBaseHealthChanged;
      playerBase.OnMoneyChanged += OnMoneyChanged;
    }
  }

  private void OnDisable()
  {
    if (playerBase != null)
    {
      playerBase.OnHealthChanged -= OnBaseHealthChanged;
      playerBase.OnMoneyChanged -= OnMoneyChanged;
    }
  }

  private void OnBaseHealthChanged(float newHealth)
  {
    if (healthText != null)
    {
      healthText.text = $"{newHealth}";
    }
  }

  private void OnMoneyChanged(float newMoney)
  {
    if (moneyText != null)
    {
      moneyText.text = $"{newMoney}";
    }
  }
}
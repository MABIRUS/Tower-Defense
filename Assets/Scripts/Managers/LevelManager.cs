using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
  public static LevelManager Instance { get; private set; }
  public PlayerBase playerBase;

  [Header("UI")]
  [SerializeField] private Text healthText;
  [SerializeField] private Text moneyText;


  private void Awake()
  {
    Instance = this;
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
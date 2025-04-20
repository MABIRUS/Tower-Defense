using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
  public class ShopManager : MonoBehaviour
  {
    [Header("References")]
    public PlayerBase playerBase;

    [Header("Shop Configuration")]
    [Tooltip("Список товаров магазина")]
    public List<ShopItemSO> shopItems;

    [Header("UI Prefabs & Container")]
    [Tooltip("Prefab кнопки магазина с компонентом Button и текстом")]
    public GameObject shopButtonPrefab;
    [Tooltip("Контейнер для кнопок (UI Panel) в Canvas")]
    public Transform buttonContainer;

    // Вспомогательная структура для хранения кнопок и данных
    private struct ShopButtonInfo
    {
      public ShopItemSO item;
      public Button button;
      public TMP_Text costText;
    }

    private List<ShopButtonInfo> buttonInfos = new List<ShopButtonInfo>();
    private Camera mainCam;
    private GameObject previewInstance;
    private TowerPreview previewScript;
    private ShopItemSO selectedItem;

    private void Start()
    {
      mainCam = Camera.main;

      // Генерация кнопок магазина
      foreach (var item in shopItems)
      {
        // 1) Создаём саму кнопку
        var btnObj = Instantiate(shopButtonPrefab, buttonContainer);
        var btn = btnObj.GetComponent<Button>();

        // 2) Иконка башни
        // Предполагаем, что в shopButtonPrefab есть дочерний объект "Icon" с Image
        var iconImg = btnObj.transform.Find("Icon")?.GetComponent<Image>();
        if (iconImg != null)
        {
          // Берём спрайт из префаба башни
          var towerSprite = item.towerPrefab.GetComponentInChildren<SpriteRenderer>()?.sprite;
          if (towerSprite != null)
            iconImg.sprite = towerSprite;
        }

        // 3) Текст цены
        var txt = btnObj.GetComponentInChildren<TMP_Text>();
        if (txt != null)
          txt.text = $"{item.price}";

        // 4) Подписка на клик
        btn.onClick.AddListener(() => BeginPlacement(item));

        // 5) Сохраняем инфу для блокировки
        buttonInfos.Add(new ShopButtonInfo
        {
          item = item,
          button = btn,
          costText = txt,
        });
      }

      // Подписка на изменение денег и первоначальная проверка
      playerBase.OnMoneyChanged += OnMoneyChanged;
      OnMoneyChanged(playerBase.Money);
    }


    // Обновление доступности кнопок по деньгам
    private void OnMoneyChanged(float newMoney)
    {
      foreach (var info in buttonInfos)
      {
        bool affordable = newMoney >= info.item.price;
        info.button.interactable = affordable;
        if (info.costText != null)
          info.costText.color = affordable ? Color.white : Color.red;
      }
    }

    private void Update()
    {
      if (previewInstance == null)
        return;

      Vector2 worldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
      previewInstance.transform.position = worldPos;

      if (Input.GetMouseButtonDown(0))
      {
        if (previewScript.CanPlace && playerBase.Money >= selectedItem.price)
        {
          playerBase.ChangeMoney(-selectedItem.price);
          Instantiate(selectedItem.towerPrefab, worldPos, Quaternion.identity);
        }
        EndPlacement();
      }

      if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        EndPlacement();
    }

    private void BeginPlacement(ShopItemSO item)
    {
      if (previewInstance != null)
        Destroy(previewInstance);

      selectedItem = item;
      previewInstance = Instantiate(item.previewPrefab);
      previewScript = previewInstance.GetComponent<TowerPreview>();
    }

    private void EndPlacement()
    {
      if (previewInstance != null)
        Destroy(previewInstance);

      previewInstance = null;
      previewScript = null;
      selectedItem = null;
    }
  }
}
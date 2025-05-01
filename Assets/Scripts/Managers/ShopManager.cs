using System.Collections.Generic;
using Assets.Scripts.Managers.Parts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
  public class ShopManager : MonoBehaviour
  {
    public PlayerBase playerBase;
    public List<ShopItemSO> shopItems;
    public GameObject shopButtonPrefab;
    public Transform buttonContainer;

    List<ShopItemInfo> infos = new List<ShopItemInfo>();
    Camera cam;

    void Start()
    {
      cam = Camera.main;

      foreach (var item in shopItems)
      {
        var go = Instantiate(shopButtonPrefab, buttonContainer);
        var btn = go.GetComponent<Button>();
        var txt = go.GetComponentInChildren<TMP_Text>();
        var icon = go.transform.Find("Icon")?.GetComponent<Image>();

        // Устанавливаем иконку из ShopItemSO
        if (icon != null && item.icon != null)
          icon.sprite = item.icon;

        // Текст цены
        if (txt != null)
          txt.text = $"{item.price}";

        btn.onClick.AddListener(() => BuildingManager.Instance.BeginPlacement(item));

        infos.Add(new ShopItemInfo { item = item, btn = btn, txt = txt });
      }

      playerBase.OnMoneyChanged += UpdateButtons;
      UpdateButtons(playerBase.Money);
    }

    void UpdateButtons(float money)
    {
      foreach (var f in infos)
      {
        bool ok = money >= f.item.price;
        f.btn.interactable = ok;
        f.txt.color = ok ? Color.white : Color.red;
      }
    }
  }
}
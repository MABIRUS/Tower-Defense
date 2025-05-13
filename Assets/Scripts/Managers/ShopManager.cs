using System.Collections.Generic;
using Assets.Scripts.Managers.Parts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
  public class ShopManager : MonoBehaviour
  {
    public static ShopManager Instance { get; private set; }

    public PlayerBase playerBase;
    public List<ShopItemSO> shopItems;
    public GameObject shopButtonPrefab;
    public Transform buttonContainer;

    List<ShopItemInfo> infos = new();
    Camera cam;
    void Start()
    {
      Instance = this;

      cam = Camera.main;

      foreach (var item in shopItems)
      {
        var GO = Instantiate(shopButtonPrefab, buttonContainer);
        var btn = GO.GetComponent<Button>();
        var txt = GO.GetComponentInChildren<TMP_Text>();
        var icon = GO.transform.Find("Icon").GetComponent<Image>();

        if (icon != null && item.icon != null)
          icon.sprite = item.icon;

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
      foreach (var info in infos)
      {
        var ok = money >= info.item.price;
        info.btn.interactable = ok;
        info.txt.color = ok ? Color.white : Color.red;
      }
    }
  }
}
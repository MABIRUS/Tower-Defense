using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemSO", menuName = "Scriptable Objects/ShopItemSO")]
public class ShopItemSO : ScriptableObject
{
  public string itemName;
  public GameObject towerPrefab;
  public GameObject previewPrefab;
  public float price;
}

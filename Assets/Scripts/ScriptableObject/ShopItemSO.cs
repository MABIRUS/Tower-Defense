using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemSO", menuName = "Scriptable Objects/ShopItemSO")]
public class ShopItemSO : ScriptableObject
{
  public float price;
  public BuildType buildType;
  public Sprite icon;

  [Header("Prefabs")]
  public GameObject buildPrefab;
  public GameObject previewPrefab;
}

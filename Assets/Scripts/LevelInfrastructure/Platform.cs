using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class Platform : MonoBehaviour
{
  [SerializeField] private GameObject highlightCircle;

  public Tower OccupiedTower;

  public List<Rail> connectedRails = new();

  void Awake()
  {
    var col = GetComponent<Collider2D>();
    col.isTrigger = false;
  }

  public void AddRail(Rail rail) 
  { 
    if (!connectedRails.Contains(rail))
      connectedRails.Add(rail); 
  }
  public void RemoveRail(Rail rail) 
  { 
    connectedRails.Remove(rail); 
  }

  public void ShowHighlight(bool show)
  {
    if (highlightCircle != null)
      highlightCircle.SetActive(show);
  }
  public bool HasTower => OccupiedTower != null;
}


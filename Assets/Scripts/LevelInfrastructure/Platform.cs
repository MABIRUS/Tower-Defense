using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class Platform : MonoBehaviour
{
  public int cost;
  [HideInInspector] public Tower OccupiedTower;
  public bool HasTower => OccupiedTower != null;
  public List<Rail> connectedRails = new List<Rail>();

  [Header("Highlight")]
  [Tooltip("Child-объект с кольцом")]
  [SerializeField] private GameObject highlightCircle;

  void Awake()
  {
    var col = GetComponent<Collider2D>();
    col.isTrigger = false;
  }

  public void AddRail(Rail r) 
  { 
    if (!connectedRails.Contains(r))
      connectedRails.Add(r); 
  }
  public void RemoveRail(Rail r) 
  { 
    connectedRails.Remove(r); 
  }

  // ¬ключить/выключить ободок
  public void ShowHighlight(bool show)
  {
    if (highlightCircle != null)
      highlightCircle.SetActive(show);
  }
}


using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(EdgeCollider2D))]
public class Rail : MonoBehaviour
{
  public Platform firstPlatform;
  public Platform secondPlatform;

  void Awake()
  {
    GetComponent<EdgeCollider2D>().isTrigger = true;
  }

  public void Initialize(Platform firstPlatform, Platform secondPlatform)
  {
    this.firstPlatform = firstPlatform; this.secondPlatform = secondPlatform;
    this.firstPlatform.AddRail(this);
    this.secondPlatform.AddRail(this);

    var lineRenderer = GetComponent<LineRenderer>();
    lineRenderer.positionCount = 2;
    lineRenderer.SetPosition(0, this.firstPlatform.transform.position);
    lineRenderer.SetPosition(1, this.secondPlatform.transform.position);

    var edgeCollider = GetComponent<EdgeCollider2D>();
    edgeCollider.points = new Vector2[]
    {
      this.firstPlatform.transform.position, this.secondPlatform.transform.position
    };
  }

  void OnDestroy()
  {
    if (firstPlatform) firstPlatform.RemoveRail(this);
    if (secondPlatform) secondPlatform.RemoveRail(this);
  }
}


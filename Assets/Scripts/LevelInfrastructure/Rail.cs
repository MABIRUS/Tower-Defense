using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(EdgeCollider2D))]
public class Rail : MonoBehaviour
{
  public Platform A, B;
  public int cost;
  public float costPerUnit = 1f;

  void Awake()
  {
    GetComponent<EdgeCollider2D>().isTrigger = true;
  }

  public void Initialize(Platform a, Platform b)
  {
    A = a; B = b;
    A.AddRail(this);
    B.AddRail(this);

    var lr = GetComponent<LineRenderer>();
    lr.positionCount = 2;
    lr.SetPosition(0, A.transform.position);
    lr.SetPosition(1, B.transform.position);

    var ec = GetComponent<EdgeCollider2D>();
    ec.points = new Vector2[]{
            A.transform.position, B.transform.position
        };

    var dist = Vector3.Distance(A.transform.position, B.transform.position);
    cost = Mathf.Max(1, Mathf.RoundToInt(dist * costPerUnit));
  }

  void OnDestroy()
  {
    if (A) A.RemoveRail(this);
    if (B) B.RemoveRail(this);
  }
}


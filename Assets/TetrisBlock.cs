using UnityEngine;
using System.Collections;

public class TetrisBlock {
  public const int WIDTH = TetrisBehavior.WIDTH;
  public const int LENGTH = TetrisBehavior.LENGTH;

  private readonly GameObject obj;
  private IntVector3 gridPosition;
  private readonly TetrisBehavior tetris;

  private object currentPositionAnimation = null;
  private object currentScaleAnimation = null;

  public TetrisBlock(GameObject obj, TetrisBehavior tetris,
    IntVector3 gridPosition) {
    GameObject clone = (GameObject)MonoBehaviour.Instantiate(obj,
      GetPosition(gridPosition),
      Quaternion.identity);

    this.obj = clone;
    this.gridPosition = gridPosition;
    this.tetris = tetris;

    clone.transform.localScale = new Vector3(0f, 0f, 0f);
    tetris.StartCoroutine(GrowIn());
  }

  Vector3 GetPosition(IntVector3 pt) {
    return new Vector3(pt.x - WIDTH / 2.0f + 0.5f,
      pt.y + 0.5f,
      pt.z - LENGTH / 2.0f + 0.5f);
  }

  public GameObject GetObject() {
    return obj;
  }

  public IntVector3 GetPosition() {
    return gridPosition;
  }

  public void MoveToPosition(IntVector3 newPosition) {
    this.gridPosition = newPosition;
    tetris.StartCoroutine(Slide(GetPosition(newPosition)));
  }

  public void Destroy() {
    tetris.StartCoroutine(ShrinkDestroy());
  }

  private IEnumerator Slide(Vector3 newPos) {
    object token = new object();
    currentPositionAnimation = token;

    Vector3 oldPos = obj.transform.position;

    for (int i = 0; i <= 5; i++) {
      if (currentPositionAnimation != token) {
        return false;
      }

      obj.transform.position = Vector3.Lerp(oldPos, newPos, i / 5f);
      yield return null;
    }

    currentPositionAnimation = null;
  }

  private IEnumerator GrowIn() {
    object token = new object();
    currentScaleAnimation = token;

    Vector3 oldScale = obj.transform.localScale;
    Vector3 newScale = new Vector3(1f, 1f, 1f);

    for (int i = 0; i <= 10; i++) {
      if (currentScaleAnimation != token) {
        return false;
      }

      obj.transform.localScale = Vector3.Lerp(oldScale, newScale, i / 10f);
      yield return null;
    }

    currentScaleAnimation = null;
  }

  private IEnumerator ShrinkDestroy() {
    object token = new object();
    currentScaleAnimation = token;

    Vector3 oldScale = obj.transform.localScale;
    Vector3 newScale = new Vector3(0f, 0f, 0f);

    for (int i = 0; i <= 10; i++) {
      if (currentScaleAnimation != token) {
        return false;
      }

      obj.transform.localScale = Vector3.Lerp(oldScale, newScale, i / 10f);
      yield return null;
    }

    currentScaleAnimation = null;

    MonoBehaviour.Destroy(obj);
  }
}

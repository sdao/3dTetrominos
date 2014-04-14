using UnityEngine;
using System.Collections;

public class CameraRotateBehavior : MonoBehaviour {
  readonly Vector3 UP = new Vector3(0f, 1f, 0f);
  readonly Vector3 RIGHT = new Vector3(1f, 0f, 0f);

  public Vector3 lookAt;
  public float pitch;
  public float yaw;

	// Use this for initialization
	void Start () {
    AlignCamera();
	}

	// Update is called once per frame
	void Update () {
    float horiz = Input.GetAxis("Horizontal");
    float vert = Input.GetAxis("Vertical");

    yaw = yaw - horiz;
    pitch = pitch + vert;
    AlignCamera();
	}

  void AlignCamera(float p, float y) {
    pitch = p;
    yaw = y;
    AlignCamera();
  }

  void AlignCamera() {
    transform.rotation = Quaternion.identity;
    transform.position = lookAt;

    transform.RotateAround(lookAt, RIGHT, pitch);
    transform.RotateAround(lookAt, UP, yaw);
    transform.Translate(0f, 0f, -15f);
  }
}

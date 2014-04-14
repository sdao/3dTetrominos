using UnityEngine;
using System.Collections.Generic;

public class InstantiateRandom : MonoBehaviour {
  public GameObject cube;
  List<Object> clones = new List<Object>();

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
    if (Input.GetKey(KeyCode.X)) {
      foreach (Object o in clones) {
        Destroy(o);
      }
    } else if (Random.Range(0, 100) == 0) {
      Debug.Log("yay!");
      float x = Random.Range(-10.0f, 10.0f);
      float y = Random.Range(-10.0f, 10.0f);
      float z = Random.Range(-10.0f, 10.0f);
      Object clone = Instantiate(cube, new Vector3(x, y, z), Quaternion.identity);
      clones.Add(clone);
    }
	}
}

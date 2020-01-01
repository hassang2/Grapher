using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {
   Vector3 point;

   [SerializeField] float size = 0.1f;

   Camera camera;

   void Awake() {
      camera = Camera.main;
   }
   void Start() {
      float scale = Vector3.Distance(camera.transform.position, transform.position) * size;
      transform.localScale = new Vector3(scale, scale, scale);
   }

   void Update() {
      float scale = Vector3.Distance(camera.transform.position, transform.position) * size;

      transform.localScale = new Vector3(scale, scale, scale);
   }
}

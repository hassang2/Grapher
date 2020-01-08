using UnityEngine;

public class AxisIndicatorCam : MonoBehaviour {
   Quaternion desiredRotation;

   [SerializeField] float rotationSmoothing = 5.0f;
   [SerializeField] Transform indicatorPos;
   [SerializeField] float distance = 10.0f;

   void Start() {
      desiredRotation = transform.rotation;
   }

   void Update() {
      transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSmoothing);
      transform.position = indicatorPos.position - (transform.rotation * Vector3.forward * distance);
   }

   public void SetRotattion(Quaternion rotation) {
      desiredRotation = rotation;
   }
}

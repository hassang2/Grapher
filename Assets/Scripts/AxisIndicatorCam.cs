using UnityEngine;

public class AxisIndicatorCam : MonoBehaviour {
   // Start is called before the first frame update

   CamControls mainCam;

   Quaternion desiredRotation;

   [SerializeField] float rotationSmoothing = 5.0f;
   [SerializeField] Transform indicatorPos;
   [SerializeField] float distance = 10.0f;

   void Start() {
      desiredRotation = transform.rotation;
   }

   // Update is called once per frame
   void Update() {
      transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSmoothing);

      transform.position = indicatorPos.position - (transform.rotation * Vector3.forward * distance);
   }

   public void SetRotattion(Quaternion rotation) {
      desiredRotation = rotation;
   }
}

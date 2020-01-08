//
// original: http://www.unifycommunity.com/wiki/index.php?title=MouseOrbitZoom

using UnityEngine;
using System.Collections;


[AddComponentMenu("Camera-Control/3dsMax Camera Style")]
public class CamControls : MonoBehaviour {
   [SerializeField] Transform target;
   [SerializeField] Vector3 targetOffset;
   [SerializeField] float distance = 5.0f;
   [SerializeField] float maxDistance = 20;
   [SerializeField] float minDistance = 5;
   [SerializeField] float rotateSpeed = 200.0f;
   [SerializeField] float moveSpeed = 100.0f;
   [SerializeField] int yMinLimit = -80;
   [SerializeField] int yMaxLimit = 80;
   [SerializeField] int zoomRate = 40;
   [SerializeField] float zoomDampening = 5.0f;

   [SerializeField] AxisIndicatorCam axisCam;

   private float xDeg = 0.0f;
   private float yDeg = 0.0f;
   private float currentDistance;
   private float desiredDistance;
   private Quaternion desiredRotation;

   void Start() { Init(); }
   void OnEnable() { Init(); }

   public void Init() {
      //If there is no target, create a temporary target at 'distance' from the cameras current viewpoint
      if (!target) {
         GameObject go = new GameObject("Cam Target");
         go.transform.position = transform.position + (transform.forward * distance);
         target = go.transform;
      }

      distance = Vector3.Distance(transform.position, target.position);
      currentDistance = distance;
      desiredDistance = distance;

      desiredRotation = transform.rotation;

      xDeg = Vector3.Angle(Vector3.right, transform.right);
      yDeg = Vector3.Angle(Vector3.up, transform.up);
   }

   /*
    * Camera logic on LateUpdate to only update after all character movement logic has been handled. 
    */
   void Update() {
      if (Input.GetMouseButton(0)) {
         xDeg += Input.GetAxis("Mouse X") * rotateSpeed;
         yDeg -= Input.GetAxis("Mouse Y") * rotateSpeed;

         desiredRotation = Quaternion.Euler(Input.GetAxis("Mouse Y") * rotateSpeed, Input.GetAxis("Mouse X") * rotateSpeed, 0) * transform.rotation;

         Quaternion origRotation = transform.rotation;

         transform.RotateAround(transform.position, transform.rotation * Vector3.up, Input.GetAxis("Mouse X") * rotateSpeed);
         transform.RotateAround(transform.position, transform.rotation * Vector3.left, Input.GetAxis("Mouse Y") * rotateSpeed);

         axisCam.SetRotattion(transform.rotation);
         desiredRotation = transform.rotation;
         transform.rotation = origRotation;

      } else if(Input.GetMouseButton(1)) {
         float xMove = Input.GetAxis("Mouse X") * moveSpeed;
         float yMove = Input.GetAxis("Mouse Y") * moveSpeed;

         Vector3 move = (transform.rotation * Vector3.left).normalized * xMove + (transform.rotation * Vector3.down).normalized * yMove;

         target.Translate(move); 
      }

      // Apply rotation smoothing
      transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * zoomDampening);

      // Zooming
      desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
      
      desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);

      currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);

      // calculate position based on the new currentDistance
      transform.position = target.position - (transform.rotation * Vector3.forward * currentDistance + targetOffset);
   }
}
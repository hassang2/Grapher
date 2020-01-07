//
// original: http://www.unifycommunity.com/wiki/index.php?title=MouseOrbitZoom

using UnityEngine;
using System.Collections;


[AddComponentMenu("Camera-Control/3dsMax Camera Style")]
public class CamControls : MonoBehaviour {
   public Transform target;
   public Vector3 targetOffset;
   public float distance = 5.0f;
   public float maxDistance = 20;
   public float minDistance = 5;
   public float rotateSpeed = 200.0f;
   public float moveSpeed = 100.0f;
   public int yMinLimit = -80;
   public int yMaxLimit = 80;
   public int zoomRate = 40;
   public float panSpeed = 0.3f;
   public float zoomDampening = 5.0f;

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
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
   public float xSpeed = 200.0f;
   public float ySpeed = 200.0f;
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
   void LateUpdate() {
      if (Input.GetMouseButton(0)) {
         xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
         yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

         // set camera rotation 
         desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
         //desiredRotation = Quaternion.rot

      } else if(Input.GetMouseButton(1)) {
         float xMove = Input.GetAxis("Mouse X");
         float yMove = Input.GetAxis("Mouse Y");

         Vector3 move = (transform.rotation * Vector3.left).normalized * xMove + (transform.rotation * Vector3.down).normalized * yMove;

         target.Translate(move); 
      }

      // Apply rotation
      transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * zoomDampening);

      // Zooming
      desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
      
      desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);

      currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);

      // calculate position based on the new currentDistance
      transform.position = target.position - (transform.rotation * Vector3.forward * currentDistance + targetOffset);
   }
}
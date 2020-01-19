using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using org.mariuszgromada.math.mxparser;
using System;

public class ValueIndicator {
   List<Plot> plots;

   [SerializeField] float interval = 0.1f;

   public ValueIndicator() {
      plots = new List<Plot>();
   }

   /*
    * Checks if we need to display the value for any of our plots
    * It finds the distance between the camera and each plot within their bounding box
    * and shows the value for the closest one that intersects with the camera to mouse position vector
    */
   public void IndicatePoint(Vector3 mousePosition) {
      float minDistance = float.MaxValue;
      Vector3 point = Vector3.zero;

      for (int i = 0; i < plots.Count; i++) {
         if (plots[i].Equation == null) 
            continue;
         
         Vector3 thisPoint = GetIntersection(plots[i], mousePosition);
         if (thisPoint == Vector3.positiveInfinity)
            continue;

         float thisDistance = Vector3.Distance(Camera.main.transform.position, thisPoint);
         
         if (thisDistance < minDistance) {
            minDistance = thisDistance;
            point = thisPoint;
         }
      }

      //if (minDistance != float.MaxValue)
      //   Debug.Log("point is: " + point.ToString());

   }

   // Checks the value of the function @eqn at s + t*v
   float GetFunctionValue(Vector3 s, Vector3 v, float t, Function eqn) {
      // y and z are swapped because visually "up" is Z but in unity and in the code "up" is y
      return (float)eqn.calculate(s[0] + v[0] * t, s[2] + v[2] * t, s[1] + v[1] * t);
   }

   /* Finds the intersection between the plot and the camera to mouse position vector
   * It checks for the sign of the functon value at the start point and points in between start and end 
   * at an @interval. If any of those intermediate function values have different sign than the values at the start
   * then we know there is a root in that interval so we run our root finding algorithm to find it.
   * returns Vector3.positiveInfinity if such point doesn't exist
   */
   Vector3 GetIntersection(Plot plt, Vector3 mousePosition) {
      //mousePosition.z = Camera.main.nearClipPlane;
      //Vector3 mouseWorld = Camera.main.ScreenToViewportPoint(mousePosition);
      Ray ray = Camera.main.ScreenPointToRay(mousePosition);
      Debug.DrawRay(ray.origin, ray.direction * 300, Color.red, 50.0f);
      Tuple<Vector3, Vector3> endPoints = GetEndPoints(plt, ray.origin, ray.direction);

      if (endPoints == null)
         return Vector3.positiveInfinity;

      Vector3 s = endPoints.Item1;
      Vector3 e = endPoints.Item2;

      Debug.Log("start: " + ray.origin.ToString());
      Debug.Log("direction: " + ray.direction.ToString());
      Debug.Log("s: " + endPoints.Item1.ToString());
      Debug.Log("e: " + endPoints.Item2.ToString());

      float t = interval;
      Vector3 diff = e - s;
      
      float val0 = GetFunctionValue(s, diff, 0, plt.IndicatorEquation);

      while (t <= 1) {
         float val1 = GetFunctionValue(s, diff, t, plt.IndicatorEquation);
         if (Mathf.Sign(val0) != Mathf.Sign(val1)) {
            Debug.Log("EYYY");
            return RegulaFalsi(0, t, plt);
         }

         t += interval;
      }

      return Vector3.positiveInfinity;

   }

   /* Given a plot, a start point and a direction it finds the intersection of the segment with the bounding
    * box of the plot. the first item of the return value is the first side of the box that is intersected and 
    * the second item is the second side
    */
   Tuple<Vector3, Vector3> GetEndPoints(Plot plt, Vector3 start, Vector3 v) {
      List<Vector3> endPoints = new List<Vector3>();

      float t;
      Vector3 point;

      // X
      t = (plt.XBound.Item1 - start[0]) / v[0];
      point = start + t * v;

      if (t >= 0 && IsInBounds(plt, point))
         endPoints.Add(point);

      t = (plt.XBound.Item2 - start[0]) / v[0];
      point = start + t * v;

      if (t >= 0 && IsInBounds(plt, point))
         endPoints.Add(point);

      // Y
      t = (plt.YBound.Item1 - start[1]) / v[1];
      point = start + t * v;

      if (t >= 0 && IsInBounds(plt, point))
         endPoints.Add(point);

      t = (plt.YBound.Item2 - start[1]) / v[1];
      point = start + t * v;

      if (t >= 0 && IsInBounds(plt, point))
         endPoints.Add(point);

      // Z
      t = (plt.ZBound.Item1 - start[2]) / v[2];
      point = start + t * v;

      if (t >= 0 && IsInBounds(plt, point))
         endPoints.Add(point);

      t = (plt.ZBound.Item2 - start[2]) / v[2];
      point = start + t * v;

      if (t >= 0 && IsInBounds(plt, point))
         endPoints.Add(point);

      if (endPoints.Count == 1) {
         Debug.LogError("ERROR: Only 1 starting point");
         return null;
      }
      if (endPoints.Count > 2) {
         Debug.LogError("ERROR: More than 2 starting points!");
         return null;
      }

      if (endPoints.Count == 0)
         return null;

      if (Vector3.Distance(start, endPoints[0]) > Vector3.Distance(start, endPoints[1]))
         return new Tuple<Vector3, Vector3>(endPoints[1], endPoints[0]);
      else
         return new Tuple<Vector3, Vector3>(endPoints[0], endPoints[1]);
   }

   bool IsInBounds(Plot plt, Vector3 point) {
      return point[0] >= plt.XBound.Item1 && point[0] <= plt.XBound.Item2 &&
             point[1] >= plt.YBound.Item1 && point[1] <= plt.YBound.Item2 &&
             point[2] >= plt.ZBound.Item1 && point[2] <= plt.ZBound.Item2;
   }

   Vector3 RegulaFalsi(float x0, float x1, Plot plt) {
      return Vector3.zero;
   }

   public void AddPlot(Plot newPlot) {
      plots.Add(newPlot);
   }

   public void RemovePlot(Plot plt) {
      plots.Remove(plt);
   }


}

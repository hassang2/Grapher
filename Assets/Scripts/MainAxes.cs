using UnityEngine;
using System.Collections;

public class MainAxes : MonoBehaviour {
   public Color lineColor = new Color(0.0f, 1.0f, 1.0f);
   [SerializeField] float lineWidth = 30;

   public Material lineMaterial;
   [SerializeField] Shader shader;
   [SerializeField] float length = 50;

   void Start() {
      if (lineMaterial == null) 
         lineMaterial = new Material(shader);

      lineMaterial.hideFlags = HideFlags.HideAndDontSave;
      lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
   }

   // to simulate thickness, draw line as a quad scaled along the camera's vertical axis.
   void DrawQuad(Vector3 p1, Vector3 p2) {
      float thisWidth = 1.0f / Screen.width * lineWidth * 0.5f;
      Vector3 edge1 = Camera.main.transform.position - (p2 + p1) / 2.0f;    //vector from line center to camera
      Vector3 edge2 = p2 - p1;    //vector from point to point
      Vector3 perpendicular = Vector3.Cross(edge1, edge2).normalized * thisWidth;

      GL.Vertex(p1 - perpendicular);
      GL.Vertex(p1 + perpendicular);
      GL.Vertex(p2 + perpendicular);
      GL.Vertex(p2 - perpendicular);
   }


   void OnRenderObject() {
      lineMaterial.SetPass(0);
      

      GL.Begin(GL.QUADS);
      GL.Color(lineColor);

      float a = length / 2;

      Vector3 x1 = new Vector3(-a, 0, 0);
      Vector3 x2 = new Vector3(a, 0, 0);

      Vector3 y1 = new Vector3(0, a, 0);
      Vector3 y2 = new Vector3(0, -a, 0);

      Vector3 z1 = new Vector3(0, 0, a);
      Vector3 z2 = new Vector3(0, 0, -a);

      DrawQuad(x1, x2);
      DrawQuad(y1, y2);
      DrawQuad(z1, z2);

      GL.End();
   
   }
}
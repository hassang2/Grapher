using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WireframeMode {
   Full,
   Grid,
   Off
}

public class Wireframe : MonoBehaviour {

   public bool render_mesh_normaly = true;

   public Color lineColor = new Color(0.0f, 1.0f, 1.0f);
   public Color backgroundColor = new Color(0.0f, 0.5f, 0.5f);

   public float lineWidth = 3;
   public int size = 0;

   private Vector3[] lines;
   private ArrayList lines_List;
   public Material lineMaterial;
   //private MeshRenderer meshRenderer; 
   [SerializeField] Shader shader;

   [SerializeField] float hoverOffset = 0.01f;
   [SerializeField] WireframeMode frameMode = WireframeMode.Grid;

   bool shouldDraw = false;

   public void Init() {
      Start();
   }

   public void Init(Vector3[] v, int x, int z) {
      Start();
      SetParameters(v, x, z);
      shouldDraw = true;
   }
   void Start() {
      //meshRenderer = gameObject.GetComponent<MeshRenderer>();
      if (lineMaterial == null) {
         lineMaterial = new Material(shader);

         /*lineMaterial = new Material ("Shader \"Lines/Colored Blended\" {" +
                                     "SubShader { Pass {" +
                                     "    Blend SrcAlpha OneMinusSrcAlpha" +
                                     "    ZWrite Off Cull Front Fog { Mode Off }" +
                                     "} } }");*/
      }
      lineMaterial.hideFlags = HideFlags.HideAndDontSave;
      lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
      lines_List = new ArrayList();

      MeshFilter filter = gameObject.GetComponent<MeshFilter>();
      Mesh mesh = filter.mesh;
      Vector3[] vertices = mesh.vertices;
      int[] triangles = mesh.triangles;

      for (int i = 0; i + 2 < triangles.Length; i += 3) {
         lines_List.Add(vertices[triangles[i]]);
         lines_List.Add(vertices[triangles[i + 1]]);
         lines_List.Add(vertices[triangles[i + 2]]);
      }

      //lines_List.CopyTo(lines);//arrays are faster than array lists
      lines = (Vector3[])lines_List.ToArray(typeof(Vector3));
      lines_List.Clear();//free memory from the arraylist
      size = lines.Length;
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

   Vector3 ToWorld(Vector3 vec) {
      return gameObject.transform.TransformPoint(vec);
   }

   Vector3[] vertices;
   int numX;
   int numZ;

   void SetParameters(Vector3[] v, int x, int z) {
      vertices = v;
      numX = x;
      numZ = z;
   }

   void OnRenderObject() {
      GetComponent<Renderer>().enabled = render_mesh_normaly;

      if (!shouldDraw)
         return;

      if (frameMode == WireframeMode.Full)
         FullWireframe();
      else if (frameMode == WireframeMode.Grid)
         GridWireframe();
   }
   void GridWireframe() {
      int index;

      // a horizontal zig-zag
      lineMaterial.SetPass(0);
      GL.Begin(GL.LINES);
      GL.Color(lineColor);

      Vector3 last = Vector3.zero;

      for (int i = 0; i <= numX; i++) {
         for (int j = 0; j <= numZ; j++) {
            index = i * (numZ + 1) + j;
            if (i == 0 && j == 0)
               last = vertices[index];
            else {
               Vector3 newPoint = ToWorld(vertices[index]);
               GL.Vertex(last);
               GL.Vertex(newPoint);

               last = newPoint;
            }
         }
         i++;
         if (i == numX + 1) break;

         for (int j = numZ; j >= 0; j--) {
            index = i * (numZ + 1) + j;
            Vector3 newPoint = ToWorld(vertices[index]);
            GL.Vertex(last);
            GL.Vertex(newPoint);

            last = newPoint;
         }
      }


      //a vertical zig - zag
      if (numZ % 2 == 0) {
         for (int i = numZ; i >= 0; i--) {
            for (int j = numX; j >= 0; j--) {
               index = j * (numZ + 1) + i;
               Vector3 newPoint = ToWorld(vertices[index]);
               GL.Vertex(last);
               GL.Vertex(newPoint);

               last = newPoint;
            }

            i--;
            if (i == -1) break;

            for (int j = 0; j <= numX; j++) {
               index = j * (numZ + 1) + i;
               Vector3 newPoint = ToWorld(vertices[index]);
               GL.Vertex(last);
               GL.Vertex(newPoint);

               last = newPoint;
            }

         }
      } else {
         for (int i = 0; i <= numZ; i++) {
            for (int j = 0; j <= numX; j++) {
               index = j * (numZ + 1) + i;
               Vector3 newPoint = ToWorld(vertices[index]);
               GL.Vertex(last);
               GL.Vertex(newPoint);

               last = newPoint;
            }

            i++;
            if (i == numX + 1) break;

            for (int j = numX; j >= 0; j++) {
               index = j * (numZ + 1) + i;
               Vector3 newPoint = ToWorld(vertices[index]);
               GL.Vertex(last);
               GL.Vertex(newPoint);

               last = newPoint;
            }
         }
      }

      GL.End();
   }

   void FullWireframe() {
      if (lines == null || lines.Length < lineWidth) {
         //print("No lines");
      } else {
         lineMaterial.SetPass(0);
         

         if (lineWidth == 1) {
            GL.Begin(GL.LINES);
            GL.Color(lineColor);
            for (int i = 0; i + 2 < lines.Length; i += 3) {
               Vector3 vec1 = ToWorld(lines[i]) + new Vector3(0, hoverOffset, 0);
               Vector3 vec2 = ToWorld(lines[i + 1]) + new Vector3(0, hoverOffset, 0);
               Vector3 vec3 = ToWorld(lines[i + 2]) + new Vector3(0, hoverOffset, 0);

               GL.Vertex(vec1);
               GL.Vertex(vec2);

               GL.Vertex(vec2);
               GL.Vertex(vec3);

               GL.Vertex(vec3);
               GL.Vertex(vec1);

            }
         } else {
            GL.Begin(GL.QUADS);
            for (int i = 0; i + 2 < lines.Length; i += 3) {
               Vector3 vec1 = ToWorld(lines[i]);
               Vector3 vec2 = ToWorld(lines[i + 1]);
               Vector3 vec3 = ToWorld(lines[i + 2]);
               DrawQuad(vec1, vec2);
               DrawQuad(vec2, vec3);
               DrawQuad(vec3, vec1);
            }
         }
         GL.End();
      }
   }

}

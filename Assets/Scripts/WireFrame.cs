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
   Mesh mesh;
   public Material lineMaterial;


   [SerializeField] Shader shader;

   [SerializeField] float hoverOffset = 0.01f;
   [SerializeField] WireframeMode frameMode = WireframeMode.Full;

   bool shouldDraw = false;

   Vector3[] meshVertices;
   int[] meshTriangles;
   int numX;
   int numZ;
   
   void Awake() {
      mesh = GetComponent<MeshFilter>().mesh;
   }

   public void Init() {
      Start();
   }

   public void Init(Vector3[] v, int[] t, int x, int z) {
      Start();
      SetParameters(v, t, x, z);
      shouldDraw = true;
   }

   void SetParameters(Vector3[] v, int[] t, int x, int z) {
      meshVertices = v;
      meshTriangles = t;
      numX = x;
      numZ = z;
   }

   void Start() {
      frameMode = WireframeMode.Full;
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
      List<int> indices = new List<int>();
      int index;

      // a horizontal zig-zag
      for (int i = 0; i <= numX; i++) {
         for (int j = 0; j <= numZ; j++) {
            index = i * (numZ + 1) + j;
            indices.Add(index);
         }
         i++;
         if (i == numX + 1) break;

         for (int j = numZ; j >= 0; j--) {
            index = i * (numZ + 1) + j;
            indices.Add(index);
         }
      }


      //a vertical zig - zag
      if (numZ % 2 == 0) {
         for (int i = numZ; i >= 0; i--) {
            for (int j = numX; j >= 0; j--) {
               index = j * (numZ + 1) + i;
               indices.Add(index);
            }

            i--;
            if (i == -1) break;

            for (int j = 0; j <= numX; j++) {
               index = j * (numZ + 1) + i;
               indices.Add(index);
            }

         }
      } else {
         for (int i = 0; i <= numZ; i++) {
            for (int j = 0; j <= numX; j++) {
               index = j * (numZ + 1) + i;
               indices.Add(index);
            }

            i++;
            if (i == numX + 1) break;

            for (int j = numX; j >= 0; j++) {
               index = j * (numZ + 1) + i;
               indices.Add(index);
            }
         }
      }

      mesh.Clear();
      mesh.vertices = meshVertices;
      mesh.SetIndices(indices.ToArray(), MeshTopology.LineStrip, 0);
   }

   void FullWireframe() {
      List<int> indices = new List<int>();

      for (int i = 0; i < meshTriangles.Length; i += 3) {
         indices.Add(meshTriangles[i]);
         indices.Add(meshTriangles[i + 1]);

         indices.Add(meshTriangles[i + 1]);
         indices.Add(meshTriangles[i + 2]);

         indices.Add(meshTriangles[i + 2]);
         indices.Add(meshTriangles[i]);
      }

      List<Color32> colors = new List<Color32>();

      for (int i = 0; i < meshVertices.Length; i++) {
         colors.Add(new Color32(125, 0, 15, 1));
      }

      mesh.Clear();
      mesh.vertices = meshVertices;
      mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
      mesh.SetColors(colors);

   }

}

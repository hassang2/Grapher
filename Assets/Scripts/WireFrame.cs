using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WireframeMode {
   Full,
   Grid,
   Off
}

public class Wireframe : MonoBehaviour {
   [SerializeField] Color lineColor = new Color(0.0f, 1.0f, 1.0f);
   [SerializeField] WireframeMode frameMode = WireframeMode.Full;
   public float hoverOffset = 0.01f;

   Mesh mesh;

   Vector3[] meshVertices;
   int[] meshTriangles;
   int numX;
   int numZ;
   
   void Awake() {
      mesh = GetComponent<MeshFilter>().mesh;
   }

   public void Init() {
      transform.Translate(0, hoverOffset, 0);
      Draw();
   }

   public void Init(Vector3[] v, int[] t, int x, int z, float hoverOffset = 0.01f, WireframeMode mode = WireframeMode.Grid) {
      SetParameters(v, t, x, z);
      frameMode = mode;
      transform.Translate(0, hoverOffset, 0);
      Draw();
   }

   void SetParameters(Vector3[] v, int[] t, int x, int z) {
      meshVertices = v;
      meshTriangles = t;
      numX = x;
      numZ = z;
   }

   void Draw() {
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

      mesh.Clear();
      mesh.vertices = meshVertices;
      mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
   }

}

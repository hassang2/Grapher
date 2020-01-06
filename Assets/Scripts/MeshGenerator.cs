using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using org.mariuszgromada.math.mxparser;

public enum ShadingMode {
   heightmap
}

public struct PlotMeshT {
   public Mesh topMesh;
   public Mesh botMesh;
   public List<Vector3> framePositions;
}
public static class MeshGenerator {

   /*
    *
    * smoothness = numX + numY
    */
   public static PlotMeshT MakePlot(GameObject obj, float minX, float maxX, float minZ, float maxZ, int smoothness, ShadingMode mode) {
      float diffX = maxX - minX;
      float diffZ = maxZ - minZ;

      int numX = (int)(diffX * smoothness / (diffX + diffZ));
      int numZ = smoothness - numX;

      float deltaX = diffX / numX;
      float deltaZ = diffZ / numZ;

      float curX = minX;
      float curZ = minZ;

      List<Vector3> vertices = new List<Vector3>();
      List<int> faces = new List<int>();
      List<Color32> colors = new List<Color32>();

      Function func = obj.GetComponent<Plot>().Equation;
      for (int i = 0; i <= numX; i++) {
         for (int j = 0; j <= numZ; j++) {
            Vector3 newVertex = obj.transform.InverseTransformVector(new Vector3(curX, (float)func.calculate(curX, curZ), curZ));
            vertices.Add(newVertex);

            colors.Add(GetColor(newVertex, mode));

            curX += deltaX;
         }
         curX = minX;
         curZ += deltaZ;
      }

      int k = 0;

      for (int i = 0; i < numX; i++) {
         for (int j = 0; j < numZ; j++) {
            faces.Add(k + 1);
            faces.Add(k);
            faces.Add(k + numX + 1);

            faces.Add(k + 1);
            faces.Add(k + numX + 1);
            faces.Add(k + numX + 2);

            k++;
         }  
         k++;
      }

      List<Vector3> normals = new List<Vector3>();
      for (int i = 0; i < vertices.Count; i++)
         normals.Add(new Vector3(0, 1, 0));

      Mesh top = new Mesh();
      top.Clear();
      top.SetVertices(vertices);
      top.SetTriangles(faces, 0);
      top.SetNormals(normals);
      top.SetColors(colors);

      Mesh bot = new Mesh();
      bot.Clear();
      bot.SetVertices(vertices);

      faces.Reverse();
      bot.SetTriangles(faces, 0);

      for (int i = 0; i < normals.Count; i++)
         normals[i] = -normals[i];

      bot.SetNormals(normals);
      bot.SetColors(colors);

      PlotMeshT ret = new PlotMeshT {
         topMesh = top,
         botMesh = bot,
      };

      return ret;
   }

   public static void CopyMesh(Mesh dest, Mesh src) {
      dest.Clear();
      dest.vertices = src.vertices;
      dest.triangles = src.triangles;
      dest.normals = src.normals;
      dest.colors = src.colors;
   }

   static Color32 GetColor(Vector3 vertex, ShadingMode mode) {
      //if (mode == ShadingMode.heightmap)
         return HeightmapColor(vertex);

      //return new Color32(255, 255, 255, 255);
   }

   static Color32 HeightmapColor(Vector3 vertex) {
      float h = 0.7f - vertex.y / 20.0f;
      h = vertex.x + vertex.y + vertex.z;
      h /= 20.0f;
      Mathf.Clamp(h, 0.3f, 0.7f);
      return Color.HSVToRGB(h, 1.0f, 0.5f);
   }

}

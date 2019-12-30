using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using org.mariuszgromada.math.mxparser;

public static class MeshGenerator {

   /*
    *
    * smoothness = numX + numY
    */
   public static List<Mesh> MakePlot(GameObject obj, float minX, float maxX, float minZ, float maxZ, int smoothness) {
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

      Function func = obj.GetComponent<Plot>().equation;
      for (int i = 0; i <= numX; i++) {
         for (int j = 0; j <= numZ; j++) {
            Vector3 newVertex = obj.transform.InverseTransformVector(new Vector3(curX, (float)func.calculate(curX, curZ), curZ));
            //Vector3 newVertex = obj.transform.InverseTransformVector(new Vector3(curX, curX * curZ, curZ));
            vertices.Add(newVertex);

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


      Mesh bot = new Mesh();
      bot.Clear();
      bot.SetVertices(vertices);

      faces.Reverse();
      bot.SetTriangles(faces, 0);

      for (int i = 0; i < normals.Count; i++) 
         normals[i] = -normals[i];

      bot.SetNormals(normals);

      return new List<Mesh>() { top, bot };
   }
}

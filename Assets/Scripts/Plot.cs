using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using org.mariuszgromada.math.mxparser;

public class Plot : MonoBehaviour {

   public Function equation { get; set; }
   public InputField input { get; set; }

   Mesh topMesh;
   Mesh botMesh;

   // Start is called before the first frame update
   void Awake() {
      topMesh = transform.Find("Mesh/Top").GetComponent<MeshFilter>().mesh;
      botMesh = transform.Find("Mesh/Bot").GetComponent<MeshFilter>().mesh;
   }

   // Update is called once per frame
   void Update() {
      
   }

   public void Display() {
      equation = Parser.Parse(input.text);

      List<Mesh> meshes = MeshGenerator.MakePlot(gameObject, -2, 2, -2, 2, 100);

      CopyMesh(topMesh, meshes[0]);
      CopyMesh(botMesh, meshes[1]);

   }

   void CopyMesh(Mesh dest, Mesh src) {
      dest.Clear();
      dest.vertices = src.vertices;
      dest.triangles = src.triangles;
      dest.normals = src.normals;
   }
}

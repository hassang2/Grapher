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

   public void Display() {
      equation = Parser.Parse(input.text);

      List<Mesh> meshes = MeshGenerator.MakePlot(gameObject, -10, 10, -10, 10, 300, ShadingMode.heightmap);

      MeshGenerator.CopyMesh(topMesh, meshes[0]);
      MeshGenerator.CopyMesh(botMesh, meshes[1]);
   }

}

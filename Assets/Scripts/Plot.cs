using UnityEngine;
using UnityEngine.UI;
using org.mariuszgromada.math.mxparser;

public class Plot : MonoBehaviour {

   public Function equation { get; set; }
   public InputField input { get; set; }

   Mesh topMesh;
   Mesh botMesh;

   Wireframe frame;

   // Start is called before the first frame update
   void Awake() {
      topMesh = transform.Find("Mesh/Top").GetComponent<MeshFilter>().mesh;
      botMesh = transform.Find("Mesh/Bot").GetComponent<MeshFilter>().mesh;

      frame = transform.Find("Frame").GetComponent<Wireframe>();
   }

   public void Display() {
      Display(input.text);
   }

   public void Display(string inpTxt) {
      equation = Parser.Parse(inpTxt);

      PlotMeshT plotMesh = MeshGenerator.MakePlot(gameObject, -10, 10, -10, 10, 100, ShadingMode.heightmap);

      MeshGenerator.CopyMesh(topMesh, plotMesh.topMesh);
      MeshGenerator.CopyMesh(botMesh, plotMesh.botMesh);

      frame.Init(plotMesh.topMesh.vertices, plotMesh.topMesh.triangles, 50, 50);
   }

}

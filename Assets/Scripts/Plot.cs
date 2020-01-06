using UnityEngine;
using UnityEngine.UI;
using org.mariuszgromada.math.mxparser;

public class Plot : MonoBehaviour {

   public Function Equation { get; set; }
   public InputField Input { get; set; }

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
      Display(Input.text);
   }

   public void Display(string inpTxt) {
      Equation = Parser.Parse(inpTxt);
      if (Equation == null)
         return;

      PlotMeshT plotMesh = MeshGenerator.MakePlot(gameObject, -10, 10, -10, 10, 100, ShadingMode.heightmap);

      MeshGenerator.CopyMesh(topMesh, plotMesh.topMesh);
      MeshGenerator.CopyMesh(botMesh, plotMesh.botMesh);

      frame.Init(plotMesh.topMesh.vertices, plotMesh.topMesh.triangles, 50, 50);
   }

}

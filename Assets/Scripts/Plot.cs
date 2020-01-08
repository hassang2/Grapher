using UnityEngine;
using UnityEngine.UI;
using org.mariuszgromada.math.mxparser;
using System.Collections;

public class Plot : MonoBehaviour {

   public Function Equation { get; set; }
   public InputField Input { get; set; }

   readonly float displayDelay = 0.5f;
   float displayTimer = 0.0f;
   bool isWaiting = false;

   Mesh topMesh;
   Mesh botMesh;

   Wireframe topFrame;
   Wireframe botFrame;

   // Start is called before the first frame update
   void Awake() {
      topMesh = transform.Find("Mesh/Top").GetComponent<MeshFilter>().mesh;
      botMesh = transform.Find("Mesh/Bot").GetComponent<MeshFilter>().mesh;

      topFrame = transform.Find("TopFrame").GetComponent<Wireframe>();
      botFrame = transform.Find("BotFrame").GetComponent<Wireframe>();

   }

   void Update() {
      if (isWaiting) {
         displayTimer += Time.deltaTime;

         if (displayTimer >= displayDelay) {
            isWaiting = false;
            Display();
         }
      }
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

      topFrame.Init(plotMesh.topMesh.vertices, plotMesh.topMesh.triangles, 50, 50);
      botFrame.Init(plotMesh.botMesh.vertices, plotMesh.topMesh.triangles, 50, 50, hoverOffset: -0.01f);
   }

   public void RestartDisplayTimer() {
      isWaiting = true;
      displayTimer = 0;
   }

}

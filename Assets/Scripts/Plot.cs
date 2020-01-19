using UnityEngine;
using UnityEngine.UI;
using org.mariuszgromada.math.mxparser;
using System;

public class Plot : MonoBehaviour {

   public Function Equation { get; set; }
   public InputField Input { get; set; }

   public Function IndicatorEquation { get; set; }

   readonly float displayDelay = 0.5f;
   float displayTimer = 0.0f;
   bool isWaiting = false;

   Mesh topMesh;
   Mesh botMesh;

   Wireframe topFrame;
   Wireframe botFrame;

   public Tuple<float, float> XBound { get; set; }
   public Tuple<float, float> YBound { get; set; }
   public Tuple<float, float> ZBound { get; set; }

   // Start is called before the first frame update
   void Awake() {
      topMesh = transform.Find("Mesh/Top").GetComponent<MeshFilter>().mesh;
      botMesh = transform.Find("Mesh/Bot").GetComponent<MeshFilter>().mesh;

      topFrame = transform.Find("TopFrame").GetComponent<Wireframe>();
      botFrame = transform.Find("BotFrame").GetComponent<Wireframe>();

      Equation = null;

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
      Tuple<Function, Function> Equations = Parser.Parse(inpTxt);

      Equation = Equations.Item1;
      IndicatorEquation = Equations.Item2;

      if (Equation == null)
         return;

      PlotMeshT plotMesh = MeshGenerator.MakePlot(gameObject, XBound, ZBound, 100, ShadingMode.heightmap);

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

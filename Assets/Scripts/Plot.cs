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

   LineRenderer frame;

   // Start is called before the first frame update
   void Awake() {
      topMesh = transform.Find("Mesh/Top").GetComponent<MeshFilter>().mesh;
      botMesh = transform.Find("Mesh/Bot").GetComponent<MeshFilter>().mesh;

      frame = transform.Find("Mesh").GetComponent<LineRenderer>();
   }

   public void Display() {
      equation = Parser.Parse(input.text);

      PlotMeshT plotMesh = MeshGenerator.MakePlot(gameObject, -10, 10, -10, 10, 100, ShadingMode.heightmap);

      MeshGenerator.CopyMesh(topMesh, plotMesh.topMesh);
      MeshGenerator.CopyMesh(botMesh, plotMesh.botMesh);

      frame.positionCount = plotMesh.framePositions.Count;
      frame.SetPositions(plotMesh.framePositions.ToArray());

      transform.Find("Mesh/Top").GetComponent<Wireframe>().Init(plotMesh.topMesh.vertices, 50, 50);
   }

}

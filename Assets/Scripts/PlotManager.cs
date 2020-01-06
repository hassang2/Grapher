using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlotManager : MonoBehaviour {

   [SerializeField] GameObject plotList;
   [SerializeField] Transform plotsParent;
   [SerializeField] Transform axesParent;


   UIPlotList UIPlotList;

   GameObject defaultMesh;
   

   void Awake() {
      UIPlotList = plotList.GetComponent<UIPlotList>();

      defaultMesh = Resources.Load<GameObject>("PlotMesh");
   }

   void Start() {
      MakeAxes();
      AddPlot();
   }

   void MakeAxes() {
      GameObject xyAxis = Instantiate(defaultMesh);
      xyAxis.GetComponent<Plot>().equation = Parser.Parse("0");
      xyAxis.transform.SetParent(axesParent);
      DisplayAxis(xyAxis);

      GameObject yzAxis = Instantiate(defaultMesh);
      yzAxis.GetComponent<Plot>().equation = Parser.Parse("0");
      yzAxis.transform.SetParent(axesParent);
      DisplayAxis(yzAxis);
      yzAxis.transform.Rotate(new Vector3(0, 0, 90), Space.Self);

      GameObject xzAxis = Instantiate(defaultMesh);
      xzAxis.GetComponent<Plot>().equation = Parser.Parse("0");
      xzAxis.transform.SetParent(axesParent);
      DisplayAxis(xzAxis);
      xzAxis.transform.Rotate(new Vector3(90, 0, 0), Space.Self);
   }

   void DisplayAxis(GameObject obj) {
      PlotMeshT plotMesh = MeshGenerator.MakePlot(obj, -50, 50, -50, 50, 100, ShadingMode.heightmap);

      Transform frameT = obj.transform.Find("Frame");
      if (frameT == null) {
         Debug.LogError("Object has no child with this name");
         return;
      }
      frameT.GetComponent<Wireframe>().Init(plotMesh.topMesh.vertices, plotMesh.topMesh.triangles, 50, 50, 0, WireframeMode.Grid);
   }

   public void AddPlot() {
      GameObject newPlotMesh = Instantiate(defaultMesh);
      newPlotMesh.transform.SetParent(plotsParent);

      InputField input = UIPlotList.AddEmptyPlot(newPlotMesh);

      newPlotMesh.GetComponent<Plot>().input = input;
   }
}

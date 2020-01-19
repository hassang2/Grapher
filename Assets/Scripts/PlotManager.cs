using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlotManager : MonoBehaviour {

   [SerializeField] GameObject plotList;
   [SerializeField] Transform plotsParent;
   [SerializeField] Transform axesParent;
   [SerializeField] Material axesMat;
   [SerializeField] Tuple<float, float> axesBounds = new Tuple<float, float>(-25, 25);

   UIPlotList UIPlotList;

   GameObject defaultMesh;
   ValueIndicator valueIndicator;

   public List<Plot> Plots { get; set; }
   

   void Awake() {
      UIPlotList = plotList.GetComponent<UIPlotList>();

      defaultMesh = Resources.Load<GameObject>("PlotMesh");
      valueIndicator = new ValueIndicator();

      Plots = new List<Plot>();
   }

   void Start() {
      MakeAxes();
   }

   void Update() {
      if (Input.GetMouseButtonDown(0))
         valueIndicator.IndicatePoint(Input.mousePosition);
   }

   void MakeAxes() {
      GameObject xyAxis = Instantiate(defaultMesh);
      xyAxis.GetComponent<Plot>().Equation = Parser.Parse("0").Item1;
      xyAxis.transform.SetParent(axesParent);
      DisplayAxis(xyAxis);

      GameObject yzAxis = Instantiate(defaultMesh);
      yzAxis.GetComponent<Plot>().Equation = Parser.Parse("0").Item1;
      yzAxis.transform.SetParent(axesParent);
      DisplayAxis(yzAxis);
      yzAxis.transform.Rotate(new Vector3(0, 0, 90), Space.Self);

      GameObject xzAxis = Instantiate(defaultMesh);
      xzAxis.GetComponent<Plot>().Equation = Parser.Parse("0").Item1;
      xzAxis.transform.SetParent(axesParent);
      DisplayAxis(xzAxis);
      xzAxis.transform.Rotate(new Vector3(90, 0, 0), Space.Self);
   }

   void DisplayAxis(GameObject obj) {
      PlotMeshT plotMesh = MeshGenerator.MakePlot(obj, axesBounds, axesBounds, 100, ShadingMode.heightmap);

      Transform frameT = obj.transform.Find("TopFrame");
      if (frameT == null) {
         Debug.LogError("Object has no child with this name");
         return;
      }
      frameT.GetComponent<Wireframe>().Init(plotMesh.topMesh.vertices, plotMesh.topMesh.triangles, 50, 50, 0, WireframeMode.Grid);
      frameT.GetComponent<Renderer>().material = axesMat;
   }

   public void AddPlot() {
      GameObject newPlotMesh = Instantiate(defaultMesh);
      newPlotMesh.transform.SetParent(plotsParent);

      InputField input = UIPlotList.AddEmptyPlot(newPlotMesh);

      Plot plt = newPlotMesh.GetComponent<Plot>();
      plt.XBound = new Tuple<float, float>(-10, 10);
      plt.YBound = new Tuple<float, float>(-10, 10);
      plt.ZBound = new Tuple<float, float>(-10, 10);

      plt.Input = input;
      plt.Input.onValueChanged.AddListener(delegate { plt.RestartDisplayTimer(); });

      Plots.Add(plt);
      valueIndicator.AddPlot(plt);
   }
}

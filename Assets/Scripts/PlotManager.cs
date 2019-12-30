using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlotManager : MonoBehaviour {

   [SerializeField] GameObject plotList;
   [SerializeField] Transform plotsParent;

   UIPlotList UIPlotList;

   GameObject defaultMesh;
   

   void Awake() {
      UIPlotList = plotList.GetComponent<UIPlotList>();

      defaultMesh = Resources.Load<GameObject>("PlotMesh");
   }

   void Start() {
      AddPlot();
   }

   public void AddPlot() {
      GameObject newPlotMesh = Instantiate(defaultMesh);
      newPlotMesh.transform.SetParent(plotsParent);

      InputField input = UIPlotList.AddEmptyPlot(newPlotMesh);

      newPlotMesh.GetComponent<Plot>().input = input;
   }
}

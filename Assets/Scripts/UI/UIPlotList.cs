using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlotList : MonoBehaviour {

   // horizontal space between plot equations
   [SerializeField] float hPad;

   List<GameObject> plots;

   GameObject defaultPlot;
   float plotHeight;

   Vector3 topLeft;

   GameObject addButton;


   void Awake() {
      hPad = 0.0f;
      plots = new List<GameObject>();

      defaultPlot = Resources.Load<GameObject>("PlotEquation");
      topLeft = transform.Find("TopLeft").position;
      addButton = transform.Find("AddButton").gameObject;
      plotHeight = defaultPlot.GetComponent<RectTransform>().rect.height;
   }

   public InputField AddEmptyPlot(GameObject newPlotMesh) {
      // Instantiate a new UI element
      GameObject newPlotEquation = Instantiate(defaultPlot, transform, false);

      plots.Add(newPlotEquation);

      // Set the position of the new element
      Vector3 newPos = new Vector3(newPlotEquation.transform.position.x,
                                   topLeft.y - (plots.Count) * (plotHeight + hPad),
                                   newPlotEquation.transform.position.z);

      newPlotEquation.GetComponent<RectTransform>().position = newPos;

      // Adjust the position of the Add button
      addButton.transform.Translate(new Vector3(0, -plotHeight - hPad, 0));

      // Subscribe the Display function to the "plot" button
      newPlotEquation.transform.Find("Button").GetComponent<Button>().
                                               onClick.AddListener(
                                               newPlotMesh.GetComponent<Plot>().Display);

      return newPlotEquation.transform.Find("InputField").GetComponent<InputField>();
   }
}

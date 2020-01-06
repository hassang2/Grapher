using System.Collections;
using System.Collections.Generic;
using org.mariuszgromada.math.mxparser;
using UnityEngine;

public static class Parser {
   
   public static Function Parse(string exp) {;
      Function function = new Function("f(x,y) = " + exp);
      if (!function.checkSyntax()) {
         Debug.LogError("Invalid equation");
         return null;
      }
      return function;
   }

}

using System.Collections;
using System.Collections.Generic;
using org.mariuszgromada.math.mxparser;

public static class Parser {
   
   public static Function Parse(string exp) {
      Function function = new Function("f(x,y) = " + exp);
      return function;
   }

}

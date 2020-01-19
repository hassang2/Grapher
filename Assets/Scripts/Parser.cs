using System.Collections;
using System.Collections.Generic;
using org.mariuszgromada.math.mxparser;
using UnityEngine;
using System.Linq;
using System.Text;
using System;

public static class Parser {

   const string operators = "-=*/^";

   public static Tuple<Function, Function> Parse(string exp) {
      string normalized = Normalize(exp);

      if (normalized.Length == 0)
         return null;

      Function function = new Function("f(x,y) = " + normalized);
      Function indicatorFunction = new Function("f(x,y,z) = z - " + normalized);

      if (!function.checkSyntax()) {
         Debug.LogError("Invalid equation");
         return null;
      }
      return new Tuple<Function, Function>(function, indicatorFunction);
   }

   static string Normalize(string exp) {
      if (exp.Length == 0)
         return exp;

      StringBuilder normalized = new StringBuilder();

      for (int i = 0; i < exp.Length - 1; i++) {
         if (Char.IsWhiteSpace(exp[i]))
            continue;

         normalized.Append(exp[i]);

         if ((IsChar(exp[i]) || IsChar(exp[i + 1])) &&
             !(IsSign(exp[i]) || IsSign(exp[i + 1])))
            normalized.Append("*");
      }

      if (!Char.IsWhiteSpace(exp[exp.Length - 1]))
         normalized.Append(exp[exp.Length - 1]);

      return normalized.ToString();
   }

   //static string Reformat(string exp, int pos) {

   //}
   static bool IsSign(char c) {
      return operators.Contains(c);
   }
   static bool IsChar(char c) {
      if (Char.IsLetter(c))
         return true;

      if (c == 'π')
         return true;

      return false;

   }

}

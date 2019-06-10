using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GraphNode : BaseNode
{
   private BehaviourGraph previousGraph;

   public override void DrawWindow()
   {
      if (BehaviorEditor.currentGraph == null)
      {
         EditorGUILayout.LabelField("Add graph to modify:");
      }
      
      BehaviorEditor.currentGraph = (BehaviourGraph)EditorGUILayout.ObjectField(BehaviorEditor.currentGraph, typeof(BehaviourGraph), false);


      if (BehaviorEditor.currentGraph == null)
      {
         if (previousGraph != null)
         {
            previousGraph = null;
         }
         EditorGUILayout.LabelField("No graph assigned");
         return;
      }

      if (previousGraph != BehaviorEditor.currentGraph)
      {
         previousGraph = BehaviorEditor.currentGraph;
      }
   }

   public override void DrawCurve()
   {
      base.DrawCurve();
   }
}

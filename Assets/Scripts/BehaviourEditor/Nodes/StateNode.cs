using UnityEditor;
using UnityEngine;


    public class StateNode : BaseNode
    {
        private bool collapse;
        public State currentState;
        public override void DrawWindow()
        {
            if (currentState == null)
            {
                EditorGUILayout.LabelField("Add state to modify:");
            }
            else
            {
                if (!collapse)
                    windowRect.height = 300;
                else
                    windowRect.height = 100;
                collapse = EditorGUILayout.Toggle("Collapse", collapse);
            }
            
            currentState = EditorGUILayout.ObjectField(currentState, typeof(State), false) as State;
        }
        public override void DrawCurve()
        {
            base.DrawCurve();
        }

      
    }

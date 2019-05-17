using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


    public class StateNode : BaseNode
    {
        private bool collapse;
        public State currentState;
        private State previousState;
        private List<BaseNode> dependencies = new List<BaseNode>();
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

            if (previousState != currentState)
            {
                previousState = currentState;
                ClearReferences();
                for (int i = 0; i < currentState.transitions.Count; i++)
                {
                    dependencies.Add(BehaviorEditor.AddTransitionNode(i, currentState.transitions[i], this));
                }
            }

            if (currentState != null)
            {
                
            }
        }
        public override void DrawCurve()
        {
            base.DrawCurve();
        }

        public Transition AddTransition()
        {
            return currentState.AddTransition();
        }

        public void ClearReferences()
        {
            BehaviorEditor.ClearWindowsFromList(dependencies);
            dependencies.Clear();
        }
    }

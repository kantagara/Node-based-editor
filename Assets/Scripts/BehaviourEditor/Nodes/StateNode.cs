using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


    public class StateNode : BaseNode
    {
        private bool collapse;
        public State currentState;
        private State previousState;
        private List<BaseNode> dependencies = new List<BaseNode>();

        private SerializedObject serializedState;
        private ReorderableList onStateList;
        private ReorderableList onEnterList;
        private ReorderableList onExitList;
        
        
        public override void DrawWindow()
        {
            if (currentState == null)
            {
                EditorGUILayout.LabelField("Add state to modify:");
            }
            else
            {
                if (collapse)
                    windowRect.height = 100;
                collapse = EditorGUILayout.Toggle("Collapse", collapse);
            }
            
            currentState = EditorGUILayout.ObjectField(currentState, typeof(State), false) as State;

            if (previousState != currentState)
            {
                serializedState = null;
                previousState = currentState;
                ClearReferences();
                for (int i = 0; i < currentState.transitions.Count; i++)
                {
                    dependencies.Add(BehaviorEditor.AddTransitionNode(i, currentState.transitions[i], this));
                }
            }

            if (currentState != null)
            {
                if(serializedState == null){
                    serializedState = new SerializedObject(currentState);
                    onStateList = new ReorderableList(serializedState, serializedState.FindProperty("onState"),
                        true,true, true, true);
                    onEnterList = new ReorderableList(serializedState, serializedState.FindProperty("onEnter"),
                        true,true, true, true);
                    onExitList = new ReorderableList(serializedState, serializedState.FindProperty("onExit"),
                        true,true, true, true);
                }

                if (!collapse)
                {
                    serializedState.Update();
                    HandleReordableList(onStateList, "On State");
                    HandleReordableList(onEnterList, "On Enter");
                    HandleReordableList(onExitList, "On Exit");
                    
                    EditorGUILayout.LabelField("");
                    onStateList.DoLayoutList();
                    EditorGUILayout.LabelField("");
                    onEnterList.DoLayoutList();
                    EditorGUILayout.LabelField("");
                    onExitList.DoLayoutList();
                    serializedState.ApplyModifiedProperties();

                    float standard = 300;
                    standard += onStateList.count * 20;
                    windowRect.height = standard;

                }
            }
        }

        void HandleReordableList(ReorderableList list, string targetName)
        {
            list.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, targetName); };
            list.drawElementCallback = (rect, index, active, focused) =>
            {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                    element, GUIContent.none);
            };
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

        public void AddTransitionNode(TransitionNode node)
        {
            dependencies.Add(node);
        }
    }

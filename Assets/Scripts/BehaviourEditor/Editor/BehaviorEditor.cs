using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BehaviorEditor : EditorWindow
    {
        #region Variables
        static List<BaseNode> windows = new List<BaseNode>();
        private Vector3 mousePosition;
        private bool makeTransition;
        private bool clickedOnAWindow;
        private BaseNode selectedNode;

        public enum UserActions
        {
            addState, addTransitionNode, deletNode, commentNode
        }

        #endregion

        #region Init

        [MenuItem("Behaviour editor/Editor")]
        static void ShowEditor()
        {
            BehaviorEditor editor = GetWindow<BehaviorEditor>();
            editor.minSize = new Vector2(800, 600);
        }
        #endregion

        #region GUI Methods

        private void OnGUI()
        {
            var e = Event.current;
            mousePosition = e.mousePosition;
            UserInput(e);
            DrawWindows();
        }

        void DrawWindows()
        {
            BeginWindows();
            foreach (var baseNode in windows)
                baseNode.DrawCurve();
            for (int i = 0; i < windows.Count; i++)
            {
                windows[i].windowRect = GUI.Window(i, windows[i].windowRect, DrawNodeWindow, windows[i].windowTitle);
            }
            EndWindows();
        }

        void DrawNodeWindow(int id)
        {
            windows[id].DrawWindow();
            GUI.DragWindow();
        }
        
        void UserInput(Event e)
        {
            if (e.button == 1 && !makeTransition)
            {
                if (e.type == EventType.MouseDown)
                    RightClick(e);
            }
            else if(e.button == 0 && !makeTransition)
            {
                if (e.type == EventType.MouseDown)
                    LeftClick(e);
            }
            
        }

        void RightClick(Event e)
        {
            selectedNode = null;
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowRect.Contains(e.mousePosition))
                {
                    clickedOnAWindow = true;
                    selectedNode = windows[i];
                    break;
                }
            }

            if (!clickedOnAWindow)
            {
                AddNewNode(e);
            }
            else
            {
                ModifyNode(e);
            }
        }

        void AddNewNode(Event e)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Add State"), false, ContextCallback, UserActions.addState);
            menu.AddItem(new GUIContent("Add Comment"), false, ContextCallback, UserActions.commentNode);
            menu.ShowAsContext();
            e.Use();
        }

        void ModifyNode(Event e)
        {
            GenericMenu menu = new GenericMenu();

            if (selectedNode is StateNode)  
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Add State"), false, ContextCallback, UserActions.addTransitionNode);
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.deletNode);
            }
            else if (selectedNode is CommentNode)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.deletNode);
            }
            
            
            menu.ShowAsContext();
            e.Use();
        }

        void ContextCallback(object o)
        {
            UserActions a = (UserActions) o;
            switch (a)
            {
                case UserActions.addState:
                    var stateNode = new StateNode();
                    stateNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 200, 300);
                    stateNode.windowTitle = "State";
                    windows.Add(stateNode);
                    break;
                case UserActions.commentNode:
                    CommentNode commentNode = new CommentNode()
                    {
                        windowRect = new Rect(mousePosition.x, mousePosition.y, 200, 100),
                        windowTitle =  "Comment",
                    };
                    windows.Add(commentNode);
                    break;
                case UserActions.deletNode:
                    if (selectedNode != null)
                    {
                        windows.Remove(selectedNode);
                    }
                    break;
                case UserActions.addTransitionNode:
                    break;
                default:
                    break;
            }
        }
        
        
        private void LeftClick(Event e)
        {
        }


        #endregion
        
        #region Helper Methods

        #endregion
    }
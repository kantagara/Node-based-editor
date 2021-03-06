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

        public static BehaviourGraph currentGraph;
        
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
            clickedOnAWindow = false;
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
                var stateNode = (StateNode) selectedNode;

                if (stateNode.currentState != null)
                {
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Add Transition"),false, ContextCallback, UserActions.addTransitionNode );
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Add State"), false, ContextCallback, UserActions.addTransitionNode);
                 
                }
                else
                {
                    menu.AddSeparator("");
                    menu.AddDisabledItem(new GUIContent("Add Transition"));
                }
                
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.deletNode);
                
              
            }
            else if (selectedNode is CommentNode)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.deletNode);
            }
            else if (selectedNode is TransitionNode)
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
                    AddStateNode(mousePosition);
                    break;
                case UserActions.commentNode:
                    AddCommentNode(mousePosition);
                    break;
                case UserActions.deletNode:
                    if (selectedNode is StateNode)
                    {
                        var target = (StateNode) selectedNode;
                        target.ClearReferences();
                        windows.Remove(target);
                    }

                    if (selectedNode is TransitionNode)
                    {
                        var target = (TransitionNode) selectedNode;
                        windows.Remove(target);

                        if (target.enterState.currentState.transitions.Contains(target.targetTransition))
                            target.enterState.currentState.transitions.Remove(target.targetTransition);
                    }

                    if (selectedNode is CommentNode)
                        windows.Remove(selectedNode);
                    break;
                case UserActions.addTransitionNode:
                    if(!(selectedNode is StateNode)) return;
                    StateNode from = (StateNode) selectedNode;
                    var transition = from.AddTransition();
                    AddTransitionNode(from.currentState.transitions.Count, transition, from);
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

        public static StateNode AddStateNode(Vector2 pos)
        {
            var stateNode = CreateInstance<StateNode>();
            stateNode.windowRect = new Rect(pos.x, pos.y, 200, 300);
            stateNode.windowTitle = "State";
            windows.Add(stateNode);

            return stateNode;
        }

        public static CommentNode AddCommentNode(Vector2 pos)
        {
            var commentNode = CreateInstance<CommentNode>();
            commentNode.windowRect = new Rect(pos.x, pos.y, 200, 100);
            commentNode.windowTitle = "Comment";
            windows.Add(commentNode);
            return commentNode;
        }
        
        

        public static TransitionNode AddTransitionNode(int index, Transition transition, StateNode from)
        {
            //from = State Node
            Rect fromRect = from.windowRect;
            float targetY = fromRect.y - fromRect.height;
            
            //Index will always be the number of  transition elements current state has 
            //so that all the 
            //in order to p
            targetY += (index * 100);
            
            fromRect.y = targetY + fromRect.height * .7f;
            fromRect.x += 350;
       
            return AddTransitionNode(new Vector2(fromRect.x, fromRect.y), transition, from);
        }

        public static TransitionNode AddTransitionNode(Vector2 pos, Transition transition, StateNode from)
        {
            var transitionNode = CreateInstance<TransitionNode>();

            transitionNode.Init(from, transition);
            
            from.AddTransitionNode(transitionNode);            
            transitionNode.windowRect = new Rect(pos.x, pos.y, 200, 80);
            transitionNode.windowTitle = "Condition Check";

            return transitionNode;
        }
        
        public static void DrawNodeCurve(Rect start, Rect end, bool left, Color curveColor)
        {
            var startPos = new Vector3
            ( (left) ? start.x + start.width : start.x,
                start.y + start.height * .5f,
                0
            );
            var endPos = new Vector3(end.x + end.width * .5f, end.y + end.height * .5f, 0);
            var startTan = startPos + Vector3.right * 50;
            Vector3 endTan = endPos + Vector3.left * 50;

            var shadow = new Color(0, 0, 0, 0.6f);

            for (int i = 0; i < 3; i++)
            {
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadow, null, (i + 1) * .5f);
            }
            Handles.DrawBezier(startPos, endPos, startTan, endTan, curveColor, null, 1);

        }

        public static void ClearWindowsFromList(List<BaseNode> l)
        {
            for (int i = 0; i < l.Count; i++)
            {
                if (windows.Contains(l[i])) windows.Remove(l[i]);
            }
        }
        #endregion
    }
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourGraph : ScriptableObject
{
    public List<Saved_StateNode> savedStateNodes = new List<Saved_StateNode>();
    Dictionary<StateNode, Saved_StateNode> stateNodesDict = new Dictionary<StateNode, Saved_StateNode>();
    Dictionary<State, StateNode> stateDict = new Dictionary<State, StateNode>();

    public void Init()
    {
        stateNodesDict.Clear();
        stateDict.Clear();
    }

#region State Nodes
    public void SetStateNode(StateNode node)
    {
        var s = GetSavedState(node); 
        if(s == null)
        {
            s = new Saved_StateNode();
            savedStateNodes.Add(s);
            stateNodesDict.Add(node, s);
        }

        s.state = node.currentState;
        s.position = new Vector2(node.windowRect.x, node.windowRect.y);
    }

    public void ClearStateNode(StateNode node)
    {
        var s = GetSavedState(node);
        if(s != null)
        {
            savedStateNodes.Remove(s);
            stateNodesDict.Remove(node);
        }
    }

    Saved_StateNode GetSavedState(StateNode node)
    {
        Saved_StateNode r;
        stateNodesDict.TryGetValue(node, out  r);
        return r;
    }

    public StateNode GetStateNode(State state)
    {
        StateNode r;
        stateDict.TryGetValue(state,  out r);
        return r;
    }
#endregion
}

[Serializable]
public class Saved_StateNode{
    public State state;
    public Vector2 position;

}
[Serializable]
public class Saved_Transition{

}
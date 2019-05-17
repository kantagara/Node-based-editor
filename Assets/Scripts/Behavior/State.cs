using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class State : ScriptableObject
{
    public StateActions[] onState;
    public StateActions[] onEnter;
    public StateActions[] onExit;
    
    public List<Transition> transitions = new List<Transition>();

    void OnEnter(StateManager states)
    {
        ExecuteActions(states, onEnter);
    }
    
    public void Tick(StateManager state)
    {
        ExecuteActions(state, onState);
        CheckTransitions(state);
    }

    void OnExit(StateManager stateManager)
    {
        ExecuteActions(stateManager, onExit);
    }

    public void ExecuteActions(StateManager state, StateActions[] l)
    {
        for (int i = 0; i < l.Length; i++)
        {
            if(l[i] != null) l[i].Execute(state);
        }
    }

    public void CheckTransitions(StateManager states)
    {
        for (int i = 0; i < transitions.Count; i++)
        {
            if(transitions[i].disable) continue;

            if (transitions[i].condition.CheckCondition(states))
            {
                states.currentState = transitions[i].targetState;
                OnExit(states);
                states.currentState.OnEnter(states);
            }

            return;
        }
    }

    public Transition AddTransition()
    {
        Transition retVal = new Transition();
        transitions.Add(retVal);
        return retVal;
    }
}

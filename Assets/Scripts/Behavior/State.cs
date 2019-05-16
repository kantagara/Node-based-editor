using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class State : ScriptableObject
{
    public List<Transition> transitions = new List<Transition>();
    void Tick()
    {
        
    }
}

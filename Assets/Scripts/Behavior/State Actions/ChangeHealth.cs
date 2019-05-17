using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Test/Add Health")]
public class ChangeHealth : StateActions
{
    public override void Execute(StateManager stateManager)
    {
        stateManager.health += 10;
    }
}

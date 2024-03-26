using UnityEngine;

public abstract class AIState
{
    protected AIController aiController;

    public AIState(AIController controller)
    {
        aiController = controller;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}

public class PatrolState : AIState
{
    public PatrolState(AIController controller) : base(controller) { }

    public override void EnterState()
    {
        Debug.Log("Entering Patrol state");
        // Additional setup code if needed
    }

    public override void UpdateState()
    {
        // Implement patrol behavior
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Patrol state");
        // Additional cleanup code if needed
    }
}

// Define other state classes similarly for Chase, Attack, Capture, Powerup states

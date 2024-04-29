using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    private InteractEvent interact = new InteractEvent();
    GameResources resources;

    public InteractEvent GetInteractEvent
    {
        get
        {
            if (interact == null) interact = new InteractEvent();
            return interact;
        }
    }

    public GameResources GetResources
    {
        get
        {
            return resources;
        }
    }

    public void CallInteract(GameResources interactedPlayer)
    {
        resources = interactedPlayer;
        interact.CallInteractEvent();
    }
}

public class InteractEvent
    {
        public delegate void InteractHandler();

        public event InteractHandler HasInteracted;

        public void CallInteractEvent() => HasInteracted?.Invoke();
}

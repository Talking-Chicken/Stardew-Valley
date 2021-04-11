using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public int state_current;
    public int state_changing;
    public int state_changingBack;
    public int state_normal;
    public int state_backpack;
    public int state_chest;
    // Start is called before the first frame update
    void Start()
    {
        state_current = state_normal;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getCurrentState()
    {
        return state_current;
    }
}

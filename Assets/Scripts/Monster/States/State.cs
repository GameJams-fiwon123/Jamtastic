using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected GameObject go;
    protected StateMachine sm;

    public State(GameObject go, StateMachine sm){
        this.go = go;
        this.sm = sm;
    }

    public virtual void Enter(){} // Called when state entered
    public virtual void Update(){} // GameObject.Update();
    public virtual void FixedUpdate(){} // GameObject.FixedUpdate();
    public virtual void Exit(){} // Called when state exited
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnityObject
{


    public virtual Vector3 Position()
    {
        return Vector3.zero;
    }


    public virtual void Init()
    {

    }

    public virtual void Release()
    {

    }

    protected virtual void OnRelease()
    {

    }

    protected virtual void OnInit()
    {

    }

}

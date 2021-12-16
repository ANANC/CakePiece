using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAction
{
    protected RoleController m_RoleController;

    public void SetRoleController(RoleController role)
    {
        m_RoleController = role;
    }

    public virtual void Init() { }

    public virtual void UnInit() { }
}

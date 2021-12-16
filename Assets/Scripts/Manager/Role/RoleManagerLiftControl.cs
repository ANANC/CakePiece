using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleManagerLiftControl : Stone_IManagerLifeControl
{
    public void InitAfter(Stone_Manager manager)
    {
        RoleManager roleManager = (RoleManager)manager;

        roleManager.AddRoleActionNameAndCreateFunc(RoleAction_ThreeDimensionalSpace_EffectArt.Name, RoleAction_ThreeDimensionalSpace_EffectArt.CreateAction);
    }
}

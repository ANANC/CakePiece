using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerManagerLifeControl : Stone_IManagerLifeControl
{
    public enum GamePlayType
    {
        ThreeDimensionalSpace = 1,  //三维空间
        MemoryLeft = 2,             //记忆通关
    }


    public void InitAfter(Stone_Manager manager)
    {
        GamePlayerManager gamePlayerManager = (GamePlayerManager)manager;

        gamePlayerManager.AddGamePlayTagAndType((int)GamePlayType.ThreeDimensionalSpace, typeof(GamePlay_ThreeDimensionalSpace));
        gamePlayerManager.AddGamePlayTagAndType((int)GamePlayType.MemoryLeft, typeof(GamePlay_MemoryLeft));
    }
}

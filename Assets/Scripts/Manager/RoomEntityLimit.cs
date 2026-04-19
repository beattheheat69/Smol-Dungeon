using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomEntityLimit : MonoBehaviour
{
    [Header("Integer Limits")]
    public int monsterLimit = 3;
    public int trapLimit = 3;
	public int totalLimit = 4;
    public int spikesLimit = 1;

    public int currentMonsterCount;
    public int currentTrapCount;
    public int currentTotalCount;
    public int currentSpikesCount;

	private void Start()
	{
        currentMonsterCount = monsterLimit;
        currentTrapCount = trapLimit;
        currentTotalCount = totalLimit;
        currentSpikesCount = spikesLimit;
	}

    public void ClearRoom()
    {
		currentMonsterCount = monsterLimit;
		currentTrapCount = trapLimit;
		currentTotalCount = totalLimit;
		currentSpikesCount = spikesLimit;
	}

    public void OccupyMonsterSpot()
    {
        currentMonsterCount--;
        currentTotalCount--;
    }

    public void OccupyTrapSpot()
    {
        currentTrapCount--;
		currentTotalCount--;
	}

    public void OccupySpikesSpot()
    {
        currentSpikesCount--;
        currentTrapCount--;
		currentTotalCount--;
	}
}

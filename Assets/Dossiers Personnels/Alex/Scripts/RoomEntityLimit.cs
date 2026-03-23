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

}

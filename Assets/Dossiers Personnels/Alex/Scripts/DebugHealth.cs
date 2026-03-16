using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugHealth : Hero
{
    [SerializeField] int hp;

	private void Update()
	{
		hp = HeroDataManager.Instance.GetHealt(index);
	}
}

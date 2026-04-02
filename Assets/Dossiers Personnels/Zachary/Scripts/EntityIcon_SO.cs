using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityIcon_SO", menuName = "Scriptable Objects/EntityIcon_SO")]
public class EntityIcon_SO : ScriptableObject
{
    [SerializeField] public List<EntityIcon> iconsList;
}

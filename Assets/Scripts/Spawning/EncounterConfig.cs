using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spawning/Encounter Config")]
public class EncounterConfig : ScriptableObject
{
    public Encounter[] encounters;
}

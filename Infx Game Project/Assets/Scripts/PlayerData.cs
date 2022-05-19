using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Player",menuName ="Entity/Player")]
public class PlayerData : ScriptableObject
{
    public string playername;
    public float health;
    public int str;
    public float crystalenergy;
    public int shards;
    public int atkrating;
    public float atkbonus;
    public Vector3 playerpos;

}

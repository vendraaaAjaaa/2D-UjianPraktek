using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy Data", order = 51)]
public class EnemyData : ScriptableObject
{
    [Header("Enemy Info")]
    public string enemyName;      // Nama enemy, misalnya "Kieru" atau "Kinze"

    [Header("Statistik")]
    public int maxHealth = 100;   // Health maksimal enemy
    public int damage = 10;       // Damage yang ditimbulkan enemy
    public float moveSpeed = 2f;  // Kecepatan gerak enemy
    public int defense = 5;

}

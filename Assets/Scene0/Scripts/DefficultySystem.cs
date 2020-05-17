using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefficultySystem : MonoBehaviour
{
    int enemyCount; 
    float DustDmg; 

    public void EasyOption()
    {
        enemyCount = 20;
        DustDmg = 0.2f;
        PlayerPrefs.SetInt("EnemyCount", enemyCount);
        PlayerPrefs.SetFloat("DustDmg", DustDmg);

        Debug.Log(PlayerPrefs.GetInt("EnemyCount") + " " + PlayerPrefs.GetFloat("DustDmg"));
    }
    public void MediumOption()
    {
        enemyCount = 40;
        DustDmg = 0.4f;
        PlayerPrefs.SetInt("EnemyCount", enemyCount);
        PlayerPrefs.SetFloat("DustDmg", DustDmg);

        Debug.Log(PlayerPrefs.GetInt("EnemyCount") + " " + PlayerPrefs.GetFloat("DustDmg"));
    }

    public void HardOption()
    {
        enemyCount = 60;
        DustDmg = 0.6f;
        PlayerPrefs.SetInt("EnemyCount", enemyCount);
        PlayerPrefs.SetFloat("DustDmg", DustDmg);

        Debug.Log(PlayerPrefs.GetInt("EnemyCount") + " " + PlayerPrefs.GetFloat("DustDmg"));
    }
}

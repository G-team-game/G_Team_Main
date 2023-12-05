using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaveEnemyIconView : MonoBehaviour
{
    [SerializeField] private Image icon;
    public void defeatEnemy()
    {
        icon.enabled = true;
    }
}
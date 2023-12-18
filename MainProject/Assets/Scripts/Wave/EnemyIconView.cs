using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyIconView : MonoBehaviour
{
    [SerializeField] private Image icon;
    public void defeatEnemy()
    {
        icon.enabled = true;
    }
}
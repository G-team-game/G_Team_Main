using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : BaseHealth
{
    private CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
    
    }

    protected override void Update()
    {
        
    }
    public override void InitHP()
    {
        base.InitHP();
       
        canvasGroup = GetComponent<CanvasGroup>();
    }
    /// <summary>
    /// ��ʾ���Ѫ��
    /// </summary>
    public void ShowPlayerHp()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
    }
    /// <summary>
    /// �������Ѫ��
    /// </summary>
    public void HidePlayerHp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
    }
}

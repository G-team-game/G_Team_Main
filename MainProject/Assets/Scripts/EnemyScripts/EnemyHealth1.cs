using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth1 : BaseHealth
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void InitHP()
    {
        base.InitHP();
        (transform as RectTransform).anchoredPosition = Vector2.zero;
        transform.localScale = Vector3.one * 0.01f;
        (transform as RectTransform).sizeDelta = new Vector2(100, 100);
        wait = new WaitForSeconds(delayTimer);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    protected override void Update()
    {
        base.Update();
    }

}

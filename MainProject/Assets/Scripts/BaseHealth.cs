using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour
{

    protected RectTransform frontFillImage;

    protected RectTransform backFillImage;

    [Header("血条的填充速度")]
    public float fillSpeed = 3;
    [Header("血条填充是否有开始延迟")]
    public bool isDelay = false;
    [Header("开始延迟的时间")]
    public float delayTimer;
    protected WaitForSeconds wait;
    protected  Coroutine coroutine;
    protected float targetValue = 1000;

    protected Transform player;

    protected float hpWidth;
    protected float hpHeight;


    private void Awake()
    {
        InitHP();
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }
    /// <summary>
    /// 初始化血条
    /// </summary>
    public virtual void InitHP()
    {
        frontFillImage = transform.GetChild(2).transform as RectTransform;
        backFillImage = transform.GetChild(1).transform as RectTransform;
        hpWidth = frontFillImage.sizeDelta.x;
        hpHeight = frontFillImage.sizeDelta.y;
        wait = new WaitForSeconds(delayTimer);






    }

    // Update is called once per frame
    protected virtual void   Update()
    {
        if(player)
        transform.LookAt(player);
    }
    /// <summary>
    /// 更新血条UI 增加血量形式
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="max"></param>
   public void UpdateAddHP(int hp,int max, float currentHp)
    {
        if (coroutine != null) StopCoroutine(coroutine); 
        coroutine= StartCoroutine(AddHPCoroutine(hp, max,currentHp));

    }
    /// <summary>
    /// 更新血条UI 减少血量形式
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="max"></param>
    public void UpdateReduceHP(int hp, int max,float currentHp)
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(ReduceHPCoroutine(hp, max,currentHp));

    }
    private IEnumerator AddHPCoroutine(int hp, int max, float currentHp)
    {


        backFillImage.sizeDelta = new Vector2((float)hp / max * hpWidth, hpHeight);

        float t = 0;
        float value = targetValue;
        if (isDelay)
            yield return wait;

        while (t < 1)
        {
            value = Mathf.Lerp(value, hp, t);


            t += Time.deltaTime / fillSpeed;


            frontFillImage.sizeDelta = new Vector2(value / max * hpWidth, hpHeight);
            targetValue = value;
            yield return 0;
        }
        frontFillImage.sizeDelta = new Vector2((float)hp / max * hpWidth, hpHeight);

    }
    private IEnumerator ReduceHPCoroutine(int hp, int max, float currentHp)
    {
        if(targetValue>currentHp)
        {
            targetValue = currentHp;
        }
  
        frontFillImage.sizeDelta = new Vector2((float)hp / max * hpWidth, hpHeight);

        float t = 0;
        float value= targetValue;
        if (isDelay)
            yield return wait;

        while (t < 1)
        {
            value = Mathf.Lerp(value, hp, t);
       

            t += Time.deltaTime / fillSpeed;

         
            backFillImage.sizeDelta = new Vector2(value/max*hpWidth , hpHeight);
            targetValue = value;
            yield return 0;
        }
        backFillImage.sizeDelta = new Vector2((float)hp / max * hpWidth, hpHeight);

    }
}

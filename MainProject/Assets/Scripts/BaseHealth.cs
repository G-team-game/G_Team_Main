using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour
{

    protected RectTransform frontFillImage;

    protected RectTransform backFillImage;

    [Header("ﾑｪﾌﾄﾌ鋧萢ﾙｶﾈ")]
    public float fillSpeed = 3;
    [Header("ﾑｪﾌ鋧萍ﾇｷﾐｿｪﾊｼﾑﾓｳﾙ")]
    public bool isDelay = false;
    [Header("ｿｪﾊｼﾑﾓｳﾙｵﾄﾊｱｼ・")]
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
    /// ｳｼｻｯﾑｪﾌ・
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
    /// ｸ・ﾂﾑｪﾌI ﾔﾓﾑｪﾁｿﾐﾎﾊｽ
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="max"></param>
   public void UpdateAddHP(int hp,int max, float currentHp)
    {
        if (coroutine != null) StopCoroutine(coroutine); 
        coroutine= StartCoroutine(AddHPCoroutine(hp, max,currentHp));

    }
    /// <summary>
    /// ｸ・ﾂﾑｪﾌI ｼﾙﾑｪﾁｿﾐﾎﾊｽ
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

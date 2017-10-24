using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {

    private float fillAmount;

    [SerializeField]
    private float lerpSpeed;

    [SerializeField]
    private Image content;
    [SerializeField]
    private Text valueText;
    [SerializeField]
    private Color fullColor;
    [SerializeField]
    private Color lowColor;

    [SerializeField]
    private bool lerpColors;
    public float MaxValue { get; set; }

    public float Value
    {
        set
        {
            string[] tmp = valueText.text.Split(':');
            valueText.text = tmp[0] + ": " + value;
            fillAmount = Map(value, 0, MaxValue);
        }
    }
	// Use this for initialization
	void Start () {

        if (lerpColors)
            content.color = fullColor;
	}
	
	// Update is called once per frame
	void Update () {
        HandelBar();
	}
    void HandelBar()
    {
        if (fillAmount != content.fillAmount)
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);

        if(lerpColors)
         content.color = Color.Lerp(lowColor, fullColor, fillAmount);
    }
    private float Map(float value,float inMin,float inMax)
    {
        return Mathf.InverseLerp(inMin, inMax, value);
    }
 
}

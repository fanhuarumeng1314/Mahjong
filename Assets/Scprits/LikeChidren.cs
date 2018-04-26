using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LikeChidren : MonoBehaviour {

    public Button nowButton;
    public int index;
    public int x;                        //当前图片的X
    public int y;                        //当前图片的Y
    public bool isLook = true;           //当前图片是否显示
    public GameObject lineUp;            //上方线条图片
    public GameObject lineDown;          //下方线条图片
    public GameObject lineLeft;          //左边线条图片
    public GameObject lineRight;         //右边线条图片
    public Image background;             //背景图片
    public GameObject mask;                   //遮罩的背景
    TestLinkStart nowLink;
	void Awake ()
    {
        lineUp = transform.Find("LineUp").gameObject;
        lineDown = transform.Find("LineDown").gameObject;
        lineLeft = transform.Find("LineLeft").gameObject;
        lineRight = transform.Find("LineRight").gameObject;
        background = transform.Find("ImgBackGround").gameObject.GetComponent<Image>();
        mask = transform.Find("Image_Mask").gameObject;
        lineUp.SetActive(false);
        lineDown.SetActive(false);
        lineLeft.SetActive(false);
        lineRight.SetActive(false);
        mask.SetActive(false);
        //index = Random.Range(0, 9);
        nowButton = GetComponent<Button>();
        nowButton.onClick.AddListener(OnClick);
       
    }

    private void Start()
    {
        nowLink = transform.parent.gameObject.GetComponent<TestLinkStart>();
        background.sprite = nowLink.sprits[index];
    }


    void Update () {
		
	}

    public void OnClick()
    {
        if (isLook)
        {
            mask.SetActive(true);
            nowLink.IsLink(x, y, index, mask);
        }

    }
}

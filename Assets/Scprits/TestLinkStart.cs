using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pos
{
    public int x;
    public int y;
}

public class TestLinkStart : MonoBehaviour {

    public GameObject nowIcon;
    [HideInInspector]
    public Vector3 pos2;
    public RectTransform nowtrs;
    public LikeChidren[,] nowChidrens;
    public List<int> nowPos;
    public List<int> types;
    [HideInInspector]
    public List<Sprite> sprits;
    [HideInInspector]
    public List<int> imgNumber;
    public List<GameObject> masks = new List<GameObject>();
    int imgNum;
	void Start ()
    {
        for (int i=0;i<10;i++)
        {
            imgNumber.Add(0);
        }
        nowIcon = Resources.Load("Button") as GameObject;
        sprits = new List<Sprite>();
        types = new List<int>();
        nowPos = new List<int>();
        nowChidrens = new LikeChidren[18, 10];

        pos2 = new Vector3(90, 90, 0);

        for (int i=1;i<11;i++)
        {
                string name = "老婆—" + i.ToString();
                Sprite nowSprite = Resources.Load(name, typeof(Sprite)) as Sprite;
                sprits.Add(nowSprite);

        }
        for (int i=0;i<10;i++)
        {
            for (int j=0;j<18;j++)
            {
                var icon = Instantiate(nowIcon, pos2, Quaternion.identity);
                var chidren = icon.GetComponent<LikeChidren>();
                chidren.x = j;
                chidren.y = i;
                chidren.index = Random.Range(0, 10);
                nowChidrens[j, i] = chidren;
                icon.transform.SetParent(transform);
                icon.transform.position = pos2;
                pos2 += new Vector3(100, 0, 0);
                if (j == 0 || j == 17 || i == 0 || i == 9)  //边界隐藏方便游戏
                {
                    chidren.isLook = false;
                    //icon.SetActive(false);
                    chidren.background.gameObject.SetActive(false);
                }
                else
                {
                    imgNumber[chidren.index]++;             //相同图片有多少
                    imgNum++;
                }
            }
            pos2 = new Vector3(90, 90, 0);
            pos2 += new Vector3(0, 100, 0) * (i+1);
        }
        ImgList();
        #region //测试用代码
        //imgNumber.Clear();
        //for (int i = 0; i < 10; i++)
        //{
        //    imgNumber.Add(0);
        //}
        //for (int i = 0; i < nowChidrens.GetLength(0); i++)
        //{
        //    for (int j = 0; j < nowChidrens.GetLength(1); j++)
        //    {
        //        if (j == 0 || j == 17 || i == 0 || i == 9)
        //        {
        //            continue;
        //        }
        //        imgNumber[nowChidrens[i, j].index]++;
        //    }
        //}
        //for (int m = 0; m < imgNumber.Count; m++)
        //{
        //    Debug.Log("图片类型数量:" + m + "个数：" + imgNumber[m].ToString());
        //}
        #endregion
    }
    /// <summary>
    /// 0折连线方法
    /// </summary>
    public List<Pos> LinkSearchZero(int x1,int y1,int x2,int y2)
    {
        List<Pos> nowPos = new List<Pos>();
        if (x1!=x2 && y1!=y2)
        {
            return null;
        }
        //X轴相等，Y轴进行延伸，判断能否一条直接相连
        if (x1==x2)
        {
            int yMax = y1 > y2 ? y1 : y2;
            int yMin = y1 < y2 ? y1 : y2;

            for (int i=++yMin;i<yMax;i++)
            {
                Pos tmpPos = new Pos();
                tmpPos.x = x1;
                tmpPos.y = i;
                nowPos.Add(tmpPos);
                bool tmpIsLook = nowChidrens[x1, i].isLook;
                if (tmpIsLook)
                {
                    return null;
                }
            }
            return nowPos;
        }
        //Y轴相等，X轴进行延伸，判断能否一条直接相连
        if (y1==y2)
        {
            int xMax = x1 > x2 ? x1 : x2;
            int xMin = x1 < x2 ? x1 : x2;
            for (int i=++xMin;i<xMax;i++)
            {
                Pos tmpPos = new Pos();
                tmpPos.x = i;
                tmpPos.y = y1;
                nowPos.Add(tmpPos);
                bool tmpIsLook = nowChidrens[i, y1].isLook;
                if (tmpIsLook)
                {
                    return null;
                }
            }
            return nowPos;
        }
        return null;
    }
    /// <summary>
    /// 一折连线方法
    /// </summary>
    public List<Pos> LinkSearchOne(int x1, int y1, int x2, int y2)
    {
        if (x1==x2 || y1==y2)
        {
            return null;
        }
        //判断对角点1现在的显示状态
        bool isLookOne = nowChidrens[x1, y2].isLook;
        if (!isLookOne)
        {
            List<Pos> linkOne = LinkSearchZero(x1,y2,x1,y1);
            List<Pos> linkTwo = LinkSearchZero(x1,y2,x2,y2);
            if (linkOne!=null && linkTwo!=null)
            {
                Pos tmpPos = new Pos();
                tmpPos.x = x1;
                tmpPos.y = y2;
                linkOne.Add(tmpPos);
                return MergeList(linkOne,linkTwo);
            }
        }
        //判断对角点2现在的显示状态
        bool isLookTwo = nowChidrens[x2, y1].isLook;
        if (!isLookTwo)
        {
            List<Pos> linkOne = LinkSearchZero(x2, y1, x1, y1);
            List<Pos> linkTwo = LinkSearchZero(x2, y1, x2, y2);
            if (linkOne != null && linkTwo != null)
            {

                Pos tmpPos = new Pos();
                tmpPos.x = x2;
                tmpPos.y = y1;
                linkOne.Add(tmpPos);
                return MergeList(linkOne, linkTwo);
            }
        }
        return null;
    }
    /// <summary>
    /// 2折连线方法
    /// </summary>
    public List<Pos> LinkSearchTwo(int x1, int y1, int x2, int y2)
    {
        List<Pos> nowPos = new List<Pos>();
        if(nowChidrens==null || nowChidrens.Length==0)
        {
            return null;
        }
        if (x1>nowChidrens.GetLongLength(0) || y1>nowChidrens.GetLength(1))
        {
            return null;
        }
        if (x2 > nowChidrens.GetLongLength(0) || y2 > nowChidrens.GetLength(1))
        {
            return null;
        }
        if (x1<0 || x2<0||y1<0||y2<0)
        {
            return null;
        }
        if (LinkSearchZero(x1,y1,x2,y2)!=null)      //查看是否满足0折连线条件
        {
            return LinkSearchZero(x1, y1, x2, y2);
        }
        if (LinkSearchOne(x1,y1,x2,y2)!=null)       //查看是否满足1折连线条件
        {
            return LinkSearchOne(x1, y1, x2, y2);
        }

        int xMax = nowChidrens.GetLength(0);
        int yMax = nowChidrens.GetLength(1);        //获取2维数组的各维长度
        for (int i =x1+1;i<xMax;i++)
        {
            Pos tmpPos = new Pos();
            tmpPos.x = i;
            tmpPos.y = y1;
            nowPos.Add(tmpPos);
            if (!nowChidrens[i, y1].isLook)
            {
                List<Pos> isLook = LinkSearchOne(i, y1, x2, y2);
                if (isLook!=null)
                {
                    return MergeList(nowPos,isLook);
                }
            }
            else
            {
                break;
            }

        }
        nowPos.Clear();
        for (int i=x1-1;i>-1;i--)
        {
            Pos tmpPos = new Pos();
            tmpPos.x = i;
            tmpPos.y = y1;
            nowPos.Add(tmpPos);
            if (!nowChidrens[i, y1].isLook)
            {
                List<Pos> isLook = LinkSearchOne(i, y1, x2, y2);
                if (isLook!=null)
                {
                    return MergeList(nowPos, isLook);
                }
            }
            else
            {

                break;
            }
        }
        nowPos.Clear();
        for (int i=y1+1;i<yMax;i++)
        {
            Pos tmpPos = new Pos();
            tmpPos.x = x1;
            tmpPos.y = i;
            nowPos.Add(tmpPos);
            if (!nowChidrens[x1, i].isLook)
            {
                List<Pos> isLook = LinkSearchOne(x1, i, x2, y2);
                if (isLook!=null)
                {
                    return MergeList(nowPos, isLook);
                }
            }
            else
            {

                break;
            }
        }
        nowPos.Clear();
        for (int i=y1-1;i>-1;i--)
        {
            Pos tmpPos = new Pos();
            tmpPos.x = x1;
            tmpPos.y = i;
            nowPos.Add(tmpPos);
            if (!nowChidrens[x1, i].isLook)
            {
                List<Pos> isLook = LinkSearchOne(x1, i, x2, y2);
                if (isLook!=null)
                {
                    return MergeList(nowPos, isLook);
                }
            }
            else
            {

                break;
            }
        }
        nowPos.Clear();
        return null;
    }

    public bool IsLink(int x,int y,int index,GameObject imgMask)
    {
        masks.Add(imgMask);
        types.Add(index);
        nowPos.Add(x);
        nowPos.Add(y);
        if (nowPos.Count>=4)
        {
            if (nowPos[0]==nowPos[2] && nowPos[1]==nowPos[3])
            {
                types.Clear();
                nowPos.Clear();
                masks[0].SetActive(false);
                masks[1].SetActive(false);
                masks.Clear();
                Debug.Log("点击相同的点");
                return false;
            }


            if (types[0]!=types[1])
            {
                types.Clear();
                nowPos.Clear();
                masks[0].SetActive(false);
                masks[1].SetActive(false);
                masks.Clear();
                return false;
            }
            List<Pos> isLinkTwo = LinkSearchTwo(nowPos[0], nowPos[1], nowPos[2], nowPos[3]);
            if (isLinkTwo==null)
            {
                types.Clear();
                nowPos.Clear();
                masks[0].SetActive(false);
                masks[1].SetActive(false);
                masks.Clear();
                return false;
            }
            Pos startPos = new Pos();
            startPos.x = nowPos[2];
            startPos.y = nowPos[3];
            isLinkTwo.Add(startPos);
            StartCoroutine(Sort(nowPos[0],nowPos[1],isLinkTwo));
            if (isLinkTwo!=null)
            {
                nowChidrens[nowPos[0], nowPos[1]].isLook = false;
                nowChidrens[nowPos[0], nowPos[1]].background.gameObject.SetActive(false);
                nowChidrens[nowPos[2], nowPos[3]].isLook = false;
                nowChidrens[nowPos[2], nowPos[3]].background.gameObject.SetActive(false);
                types.Clear();
                nowPos.Clear();
                masks[0].SetActive(false);
                masks[1].SetActive(false);
                EggWhite();
                masks.Clear();
                return true;
            }
            types.Clear();
            nowPos.Clear();
            masks[0].SetActive(false);
            masks[1].SetActive(false);
            masks.Clear();
        }

        return false;
    }

    public List<Pos> MergeList(List<Pos> pos1,List<Pos> pos2)
    {
        for (int i=0;i<pos2.Count;i++)
        {
            pos1.Add(pos2[i]);
        }
        return pos1;

    }

    IEnumerator Sort(int x, int y, List<Pos> pos)
    {
        List<GameObject> lines = new List<GameObject>();                //用于存储当前显示的连线物体，方便连线结束后关闭显示。
        while (pos.Count > 0)
        {
            for (int i = 0; i < pos.Count; i++)
            {
                if (x + 1 == pos[i].x && y == pos[i].y)
                {
                    nowChidrens[x, y].lineRight.SetActive(true);
                    nowChidrens[x + 1, y].lineLeft.SetActive(true);
                    lines.Add(nowChidrens[x, y].lineRight);
                    lines.Add(nowChidrens[x + 1, y].lineLeft);
                    x = x + 1;
                    pos.RemoveAt(i);
                    break;
                }

                if (x - 1 == pos[i].x && y == pos[i].y)
                {
                    nowChidrens[x, y].lineLeft.SetActive(true);
                    nowChidrens[x - 1, y].lineRight.SetActive(true);
                    lines.Add(nowChidrens[x, y].lineLeft);
                    lines.Add(nowChidrens[x - 1, y].lineRight);
                    x = x - 1;
                    pos.RemoveAt(i);
                    break;
                }

                if (y + 1 == pos[i].y && x == pos[i].x)
                {
                    nowChidrens[x, y].lineUp.SetActive(true);
                    nowChidrens[x, y + 1].lineDown.SetActive(true);
                    lines.Add(nowChidrens[x, y].lineUp);
                    lines.Add(nowChidrens[x, y + 1].lineDown);
                    y = y + 1;
                    pos.RemoveAt(i);
                    break;
                }

                if (y - 1 == pos[i].y && x == pos[i].x)
                {
                    nowChidrens[x, y].lineDown.SetActive(true);
                    nowChidrens[x, y - 1].lineUp.SetActive(true);
                    lines.Add(nowChidrens[x, y].lineDown);
                    lines.Add(nowChidrens[x, y - 1].lineUp);
                    y = y - 1;
                    pos.RemoveAt(i);
                    break;
                }
            }
            yield return new WaitForSecondsRealtime(0.1f);

        }
        for (int i=0;i<lines.Count;i++)
        {
            lines[i].SetActive(false);
        }


        yield return null;
    }
    /// <summary>
    /// 解决不能完全消除BUG方法
    /// </summary>
    public void ImgList()
    {
        List<int> lastImgList = new List<int>();
        for (int i=0;i<imgNumber.Count;i++)
        {
            if (imgNumber[i]%2==1)
            {
                lastImgList.Add(i);
            }
        }
        if (lastImgList.Count%2==1)
        {
            Debug.Log("出现异常");
        }
        int number = lastImgList.Count / 2;

        for (int i=0;i< nowChidrens.GetLength(0);i++)
        {
            for (int j=0;j<nowChidrens.GetLength(1);j++)
            {
                if (j == 0 || j == 17 || i == 0 || i == 9)
                {
                    continue;   //边界不进行处理
                }
                if (nowChidrens[i,j].index==lastImgList[0])
                {
                    nowChidrens[i, j].index = lastImgList[0 + number];
                    nowChidrens[i, j].background.sprite = sprits[nowChidrens[i, j].index];
                    lastImgList.RemoveAt(0+number);
                    lastImgList.RemoveAt(0);
                    number--;
                }
                if (number==0)
                {
                    Debug.Log("解决完毕");
                    return;
                }
            }
        }
    }
    #region
    public void EggWhite()
    {
        imgNum -= 2;
        if (imgNum<=0)
        {
            Sprite spriteImg = Resources.Load("彩蛋",typeof(Sprite))as Sprite;
            Image imgBack = GameObject.Find("Canvas/Panel").gameObject.GetComponent<Image>();
            imgBack.sprite = spriteImg;
        }

    }
    #endregion
}

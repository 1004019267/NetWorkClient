using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text1 : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        WebWork.Instance.Init();
        //StartCoroutine(WebWork.Instance.Get(EOPERATION.DownLoad, "chuanjianyinwei.mp4"));
        Dictionary<string, string> d = new Dictionary<string, string>();
        CoroutineManager.Instance.StartCor(WebWork.Instance.Post(EOPERATION.ADDSCORE, d));
        //string str = "the best hop";
        //Debug.Log(str);
        //string newStr = MD5.GetMD5Hash(str);
        //newStr = "aaaa";
        //Debug.Log(newStr);
        //Debug.Log(MD5.VerifyMD5Hash(str, newStr));
        IPConfig ip = new IPConfig();
        Debug.Log(ip.GetIP(EIP.OutSideIP));
    }

    // Update is called once per frame
    void Update()
    {
        if (WebWork.Instance.isStart)
        {
            Debug.Log(WebWork.Instance.GetProgress());
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using System.Reflection;
using System.Linq;
using Newtonsoft.Json;
public enum EOPERATION
{
    LOGIN = 0,//登录
    REGISTER,//注册
    COLLEGELIST, //学院
    MAJORLIST, //专业
    CLASSLIST,//班级
    EXISTMAIL,//邮箱重复验证
    EXISTNUMBER, //学号重复验证
    GETPASSWORD,//忘记密码
    ADDSCORE,//添加成绩
    DownLoad,
    GetOutSideIP,
}

/// <summary>
/// 分发器接口
/// </summary>
public interface IHander
{
    void RegisterMsg(Dictionary<EOPERATION, Action<TokenMsg>> handers);
}

public class UrlPath
{
    public EOPERATION operation;
    public string path;
}

public class WebWorkData
{
    public string ipAddress;
    public Dictionary<EOPERATION, string> urlPaths;
}

public struct TokenMsg
{
    public string name;
    public DownloadHandler hander;
}

public class WebWork :Singleton<WebWork>
{
    Dictionary<EOPERATION, Action<TokenMsg>> _handers = new Dictionary<EOPERATION, Action<TokenMsg>>();
    public bool isStart { get; private set; }
    UnityWebRequest request;
    //根据协议号获取地址后缀
    WebWorkData webWorkData;

    //在这里注册消息返回后分发处理
    public void Init()
    {
        //返回我的解决方案中的所有程序集
        Type[] types = AppDomain.CurrentDomain.GetAssemblies()
.SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IHander))))
.ToArray();

        //相当于
        //public static IEnumerable<Type> GetType(Type interfaceType)
        //{
        //    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        //    {
        //        foreach (var type in assembly.GetTypes())
        //        {
        //            foreach (var t in type.GetInterfaces())
        //            {
        //                if (t == interfaceType)
        //                {
        //                    yield return type;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}
        foreach (var item in types)
        {
            IHander obj = (IHander)Activator.CreateInstance(item);
            obj.RegisterMsg(_handers);
        }


        //按Json格式序列化
        //WebWorkData web = new WebWorkData() { ipAddress = ipAddress, urlPaths = mURLs };
        //var json = JsonConvert.SerializeObject(web, Formatting.Indented);
        //FileTool.CreateFile(Application.streamingAssetsPath + "/WebData.json", json);

        var str = FileTool.BytesToStr(FileTool.ReadFile(Application.streamingAssetsPath + "/WebData.json"));
        webWorkData = JsonConvert.DeserializeObject<WebWorkData>(str);

    }

    string GetUrl(EOPERATION op, string name = "")
    {
        return webWorkData.ipAddress + webWorkData.urlPaths[op] + name;
    }

    /// <summary>
    /// 获取下载进度
    /// </summary>
    /// <returns></returns>
    public float GetProgress()
    {
        if (request == null || !isStart)
            throw new Exception(GetType() + "GetProgress()/request==null or !isStart");
        return request.downloadProgress;
    }

    public IEnumerator Get(EOPERATION op, string name)
    {
        string url = GetUrl(op, name);
        if (!string.IsNullOrEmpty(url))
        {
            using (request = UnityWebRequest.Get(url))
            {
                isStart = true;
                //设置超时 链接超时返回 且isNetworkError为true
                request.timeout = 30;
                yield return request.SendWebRequest();
                isStart = false;
                //结果回传给具体实现
                if (request.isHttpError || request.isNetworkError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    _handers[op](new TokenMsg { name = name, hander = request.downloadHandler });
                }
            };
        }
    }

    public IEnumerator Post(EOPERATION op, Dictionary<string, string> dic)
    {
        string url = GetUrl(op);
        if (!string.IsNullOrEmpty(url))
        {
            WWWForm form = new WWWForm();

            foreach (var item in dic)
            {
                form.AddField(item.Key, item.Value);
            }
            using (request = UnityWebRequest.Post(url, form))
            {
                isStart = true;
                request.timeout = 30;
                yield return request.SendWebRequest();
                isStart = false;
                //结果回传给具体实现
                if (request.isHttpError || request.isNetworkError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    _handers[op](new TokenMsg {  hander = request.downloadHandler });
                }
            }
        }
    }
}
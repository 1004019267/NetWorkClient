using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AccountHander:IHander
{
    public void RegisterMsg(Dictionary<EOPERATION, Action<TokenMsg>> handers)
    {
        handers.Add(EOPERATION.LOGIN, OnRspLogin);
        handers.Add(EOPERATION.REGISTER, OnRspRegister);
        handers.Add(EOPERATION.DownLoad, OnRspDownLoad);
    }

    private void OnRspLogin(TokenMsg msg)
    {
        //用Json转化为类内部数据
        JsonConvert.DeserializeObject<CallBackUser>(msg.hander.text); 
        
    }
    private void OnRspRegister(TokenMsg msg)
    {

    }

    private void OnRspDownLoad(TokenMsg msg)
    {
        //data.data二进制的文件 视频 图片的信息
        //路径 byte[]
        FileTool.CreateFile(msg.name, msg.hander.data);
    }
 
}

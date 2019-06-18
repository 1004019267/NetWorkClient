/**
*Copyright(C) 2019 by #COMPANY#
*All rights reserved.
*FileName:     #SCRIPTFULLNAME#
*Author:       #AUTHOR#
*Version:      #VERSION#
*UnityVersion：#UNITYVERSION#
*Date:         #DATE#
*Description:   
*History:
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
public enum EIP
{
    IPv4,
    IPv6,
    OutSideIP,
}

public class IPConfig
{
    public string GetIP(EIP type = EIP.IPv4)
    {
        switch (type)
        {
            case EIP.IPv4:
               return GetInsideIP(AddressFamily.InterNetwork);             
            case EIP.IPv6:
                return GetInsideIP(AddressFamily.InterNetworkV6);             
            case EIP.OutSideIP:
                return GetOutSideIP();            
        }
        return null;
    }

    string GetInsideIP(AddressFamily addressType)
    {
        IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
        foreach (var item in ips)
        {
            if (item.AddressFamily == addressType)
            {
                return item.ToString();
            }
        }
        return null;
    }

    string GetOutSideIP()
    {
        using (WebClient wc = new WebClient())
        {
            return wc.DownloadString(@"http://icanhazip.com/");
        }
    }
}

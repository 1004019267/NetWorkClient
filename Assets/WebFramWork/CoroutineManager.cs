
using System;
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : SingletonM<CoroutineManager>
{
    public void StartCor(IEnumerator cor)
    {
        StartCoroutine(cor);
    }
}

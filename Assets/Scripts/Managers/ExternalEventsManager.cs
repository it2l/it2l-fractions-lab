﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using fractionslab.meshes;
using CodeTitans.JSon;
using taskDependentSupport;

public class ExternalEventsManager : MonoBehaviour
{
    #region Singleton instance
    protected static ExternalEventsManager instance = null;

    public static ExternalEventsManager Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    #region Protected Fields
    protected GameObject workspace;
    protected GameObject interfaces;
    #endregion

    #region External Events
    public void SendBrowserMessage(params object[] args)
    {
#if !UNITY_IPHONE
        Application.ExternalCall("newEvent", args);
#endif
    }

    public void SendMessageToSupport(params object[] args)
    {
        //Debug.Log(args[0].ToString() + " ---> " + args[1].ToString());
        TDSWrapper.SendMessageToSupport(args);
    }
    #endregion

    #region External Callbacks
    public void SendEvent(string json)
    {
        JSonReader reader = new JSonReader();
        IJSonObject jsonObj = reader.ReadAsJSonObject(json);

        if (jsonObj.Contains("method") && jsonObj.Contains("parameters"))
        {
            switch (jsonObj["method"].ToString())
            {
                case ("LowFeedback"):
                    if (jsonObj["parameters"].Contains("message"))
                        if (null != interfaces)
                            interfaces.SendMessage("ShowLowFeedback", jsonObj["parameters"]["message"].ToString(), SendMessageOptions.DontRequireReceiver);
                    break;
                case ("HighFeedback"):
                    if (jsonObj["parameters"].Contains("message"))
                        if (null != interfaces)
                            interfaces.SendMessage("ShowHighFeedback", jsonObj["parameters"]["message"].ToString(), SendMessageOptions.DontRequireReceiver);
                    break;
                case ("Highlight"):
                    if (jsonObj["parameters"].Contains("target"))
                        if (null != workspace)
                            workspace.SendMessage("Highlight", jsonObj["parameters"]["target"].ToString(), SendMessageOptions.DontRequireReceiver);
                    break;
                case ("RemoveHighlight"):
                    if (null != workspace)
                        workspace.SendMessage("DestroyHighlight", SendMessageOptions.DontRequireReceiver);
                    break;
            }
        }
    }
    #endregion

    #region Unity callbacks
    void Awake()
    {
        if (instance != null)
            Debug.LogError("ExternalEventsManager component must be unique!");

        instance = this;
    }

    void Start()
    {
        interfaces = GameObject.FindGameObjectWithTag("Interface");
        workspace = GameObject.FindGameObjectWithTag("Workspace");
        TDSWrapper.eventManager = gameObject;
    }
    #endregion
}

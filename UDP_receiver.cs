using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.UI;
using System;
using System.Text;
using UnityEngine.UIElements;

public class UDP_receiver : MonoBehaviour
{
    UdpClient Client;
    string data;
    public GameObject Button_begn;
    private TMPro.TMP_Text _textChat;
    void Start()
    {
        Client = new UdpClient(1111);
        data = "";
        _textChat = GameObject.Find("TextChat").GetComponent<TMPro.TMP_Text>();
        StartReceive();
    }

    public void StartReceive()
    {
        try
        {
            Client.BeginReceive(new AsyncCallback(recv), null);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message.ToString());
        }
    }

    public void recv(IAsyncResult res)
    {
        IPEndPoint RemoteIp = new IPEndPoint(IPAddress.Any, 60240);
        byte[] received = Client.EndReceive(res, ref RemoteIp);
        data = Encoding.UTF8.GetString(received);
        Debug.Log(data);
        Client.BeginReceive(new AsyncCallback(recv), null);
        
    }
    public void PrintPackages()
    {
        _textChat.text = data;
    }
}

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
    private const int bufSize = 8 * 1024;
    private TMPro.TMP_Text _textChat;
    private GameObject inputFieldSender;
    void Start()
    {
        Client = new UdpClient(12345);
        data = "";
        _textChat = GameObject.Find("TextChat").GetComponent<TMPro.TMP_Text>();
        inputFieldSender = GameObject.Find("/Canvas/PanelMain/PanelMiddle/InputField_ToServer/Text");

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
        IPEndPoint RemoteIp = new IPEndPoint(IPAddress.Any, 12345);
        byte[] received = Client.EndReceive(res, ref RemoteIp);
        data = "Recived From "+RemoteIp.ToString()+" : "+Encoding.UTF8.GetString(received)+"\n";
        Debug.Log(data);
        Client.BeginReceive(new AsyncCallback(recv), null);
        
    }
    public void RefreshChat()
    {
        _textChat.text += data;
    }
    public void SendToServer()
    {
        string tosend = inputFieldSender.GetComponent<Text>().text;
        byte[] dataToSend = Encoding.ASCII.GetBytes(tosend);
        _textChat.text += "The message <"+tosend+"> have not been sent. Sender in construction. \n";
        //Client.Send(dataToSend, tosend.Length);
    }
}

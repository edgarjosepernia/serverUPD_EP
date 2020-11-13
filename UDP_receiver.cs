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
    public const int bufSize = 8 * 1024;
    private const int port = 12345;
    //private const string ipAddress = "192.168.43.31"; // house
    private const string ipAddress = "10.167.8.205"; // Institut


    private Socket _socket;
    private State state = new State();

    private TMPro.TMP_Text _textChat;
    private GameObject inputFieldSender;
    void Start()
    {
        Client = new UdpClient(port);
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        data = "";
        _textChat = GameObject.Find("TextChat").GetComponent<TMPro.TMP_Text>();
        inputFieldSender = GameObject.Find("/Canvas/PanelMain/PanelMiddle/InputField_ToServer/Text");

        StartReceive();
    }

    public void StartReceive()
    {
        try
        {
            Client.BeginReceive(new AsyncCallback(recv), state);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message.ToString());
        }
    }

    public void recv(IAsyncResult res)
    {
        IPEndPoint RemoteIp = new IPEndPoint(IPAddress.Any, port);
        byte[] received = Client.EndReceive(res, ref RemoteIp);
        data = "Recived From "+RemoteIp.ToString()+" : "+Encoding.UTF8.GetString(received)+"\n";
        Debug.Log(data);
        Client.BeginReceive(new AsyncCallback(recv), state);
        
    }
    public void RefreshChat()
    {
        _textChat.text += data;
    }
    public void SendToServer()
    {
        
        try
        {
            string txtToSend = inputFieldSender.GetComponent<Text>().text;
            byte[] dataToSend = Encoding.ASCII.GetBytes(txtToSend);

            _socket.Connect(IPAddress.Parse(ipAddress), port);
            _socket.BeginSend(dataToSend, 0, dataToSend.Length, SocketFlags.None, (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndSend(ar);
                Console.WriteLine("SEND: {0}, {1}", bytes, txtToSend);
            }, state);
            
            _textChat.text += txtToSend + "\n";
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message.ToString());
            _textChat.text += "The message have not been sent. Sender in construction. \n";
        }

    }
}

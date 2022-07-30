using UnityEngine;
using System.Collections;
 
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
public class UDPconnection : MonoBehaviour {

    private Thread receiveThread;
    private UdpClient socketConnection; 
    private IPEndPoint remoteEndPoint;
    // infos
    public string lastReceivedUDPPacket="";
    public string allReceivedUDPPackets=""; // clean up this from time to time!

    private string IP;  
    public int port_send; 
    public int port_receive;
   
   
    
    public void Start()
    {
        
        //client = new UdpClient();
        //print("Sending to "+IP+" : "+port);
        //print("Testing: nc -lu "+IP+" : "+port);
        //print("UDPSend.init()");
        //print("Sending to 127.0.0.1 : "+port);
        //print("Test-Sending to this Port: nc -u 127.0.0.1  "+port+"");
        receiveThread = new Thread(
        new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start(); 
        
    }
   void Update()
   {
       if (socketConnection == null)
		{
			Debug.Log("socket sample scene null");
			return;
		}
        try{
            sendString();
        }catch (Exception socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
       
   }
    // receive thread
    private void ReceiveData()
    {
 
        //SOS
        try{
            IP= "127.0.0.1";
            port_send = 8052;
            port_receive = 8052;
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port_send);
            socketConnection = new UdpClient(port_receive);
            Debug.Log("GG");
            
            
            IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
            
             Debug.Log("GG1");
            
        
            while (true)
            {
                try
                {
                    //Creates an IPEndPoint to record the IP Address and port number of the sender.
                    // The IPEndPoint will allow you to read datagrams sent from any source.
                    
                    Debug.Log("IP1");
                    byte[] data = socketConnection.Receive(ref anyIP);
                    Debug.Log("IP2");
                    string serverMessage = Encoding.ASCII.GetString(data);
                    Debug.Log("IP3");
                    Debug.Log("<-- message from server -->" + serverMessage);                   
                    // lastReceivedUDPPacket=text;
                    // allReceivedUDPPackets=allReceivedUDPPackets+text;

                }
                catch (Exception err)
                {
                    Debug.Log("ListenFail1");
                }
            }
        }catch (Exception err)
        {
            Debug.Log("ListenFail2");
        }
    }
    // public string getLatestUDPPacket()
    // {
    //     allReceivedUDPPackets="";
    //     return lastReceivedUDPPacket;
    // }
   
    // getLatestUDPPacket
    // cleans up the rest
    private void sendString()
    {
        if(socketConnection != null){
            try
            {              
                string clientMessage = "HI data";
                byte[] data_sent = Encoding.ASCII.GetBytes(clientMessage);
                Debug.Log("Socket Found2");  
                socketConnection.Send(data_sent, data_sent.Length, remoteEndPoint);
                Debug.Log("Socket Found3");  
                
            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
            }
        }else{
            Debug.Log("socketConnection Not Found");
            return;
        }
    }
    //  private void sendEndless(string testStr)
    // {
    //     do
    //     {
    //         sendString(testStr);
    //     }
    //     while(true);
       
    // }
}



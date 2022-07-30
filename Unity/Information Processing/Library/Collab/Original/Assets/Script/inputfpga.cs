using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using System.Windows;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class inputfpga : MonoBehaviour
{
    static inputfpga instance;
    static Scene m_Scene;
    Thread mThread;
    // public string connectionIP = "127.0.0.1";
    private int connectionPort = 8052;
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;
    Vector3 receivedPos = Vector3.zero;
    public int moving_direction;
    public int speed;
    public int eat;
    public int bomb;
    public string username;
    public int Button0_Pressed;
    public int Button1_Pressed;
    bool running;
    public int prev_kill_num = 0;
    public int cur_kill_num = 0;
    public string pre_health;
    public string current_health;
    public Text kill_num_display;
    public string scene_name;
    GameObject healthBar;
    GameObject opponent_car; 
    HealthBar healthBarScript;
    MyNetworkManger opponent_carscript;



void Awake()
    {
        
        m_Scene = SceneManager.GetActiveScene();
        if (m_Scene.name == "SampleScene")
        {
            healthBar = GameObject.Find("Health bar");
            //  prev_kill_num = 0;
            //cur_kill_num = 0;
        }
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }
    private void Update()
    {
        //transform.position = receivedPos; //assigning receivedPos in SendAndReceiveData()
        scene_name = m_Scene.name;
        Debug.Log("healthBar is null? " + (healthBar == null), gameObject);
        if (m_Scene.name == "SampleScene")
		{
			try 
			{
			    SendData();		
			}
			catch
			{
				Debug.Log("Sending Fail");
			}
        }

    }

    private void Start()
    {
        
        
        connectionPort = 8052;
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();
        
         
        //healthBarScript = healthBar.GetComponent<HealthBar>();
        opponent_car = GameObject.Find("MyNetworkManger");
        opponent_carscript = opponent_car.GetComponent<MyNetworkManger>();

    }

    void GetInfo()
    {
        localAdd = IPAddress.Parse("127.0.0.1");
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listener.Start();
        Debug.Log("socket built");
        client = listener.AcceptTcpClient();
        Debug.Log("connection built");
        running = true;
        while (running)
        {
            SendAndReceiveData();
        }
        listener.Stop();
    }

    void SendAndReceiveData()
    {
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];

        //---receiving Data from the Host----
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string


        if (dataReceived != null)
        {
            Debug.Log(dataReceived);
            string[] lines = dataReceived.Split(',');
            for (int i = 0; i < lines.Length - 1; i++)
            {
                string[] result = lines[i].Split('/');
                if(result[0] == "Initial")
                {
                    username = result[1];
                }
                else
                {
                    Debug.Log(result.Length);
                    moving_direction = int.Parse(result[0]);
                    speed = int.Parse(result[1]);
                    eat = int.Parse(result[2]);
                    bomb = int.Parse(result[3]);
                    Button1_Pressed =  int.Parse(result[3]);
                    Button0_Pressed = int.Parse(result[4]);
                }               
            }
        }
    }
    

    private void SendData()
	{

		if (client == null)
		{
			return;
		}
		try
		{
			NetworkStream stream = client.GetStream();
            if (stream.CanWrite)
            {
                if (scene_name == "SampleScene" )
                {
                    current_health = healthBar.GetComponent<Slider>().value.ToString(); 
                    if(current_health != pre_health || prev_kill_num != cur_kill_num){
                        pre_health = current_health;
                        string clientMessage = 
                            "" + current_health + "/" +
                            prev_kill_num.ToString() + ",";             
                        byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
                        // Write byte array to socketConnection stream.                 
                        stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                        Debug.Log("Client sent his message - should be received by server" + clientMessage);
                    }
                }
            }		
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
    }

}
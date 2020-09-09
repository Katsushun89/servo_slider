using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

public class ServoSliderServer : MonoBehaviour
{
    WebSocketServer server;
    Queue<string> servoAngle = new Queue<string>();

    //Slider slider; 

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        server = new WebSocketServer(8080);

        server.AddWebSocketService<ServoStatus>("/", () => new ServoStatus(){
            ServoAngle = servoAngle 
        });
        server.Start();

    }

    public void ReadSlider(float value)
    {
        Debug.Log(value);
        servoAngle.Enqueue(value.ToString("####0.00"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDestroy()
    {
        server.Stop();
        server = null;
    }

}

public class ServoStatus : WebSocketBehavior
{
    public Queue<string> ServoAngle;

    public void SendMessage(string msg)
    {
        Sessions.Broadcast(msg);
    }

    protected override void OnMessage (MessageEventArgs e)
    {
        //Debug.Log(e.Data);
        string angle = "{\"angle\":\"";
        if(ServoAngle.Count > 0)
        {
            angle += ServoAngle.Dequeue();
            angle += "\"}";
            Debug.Log(angle);
            Sessions.Broadcast(angle);
        }
    }
}
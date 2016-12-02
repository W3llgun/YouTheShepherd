using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.Net.Sockets;
using System.Collections;

public class IRC : MonoBehaviour {
    private static byte[] data;
    NetworkStream stream;
    TcpClient client;
    string channel ;
    string loginstring;
	bool listen = true;
    public int port = 6667;
    public string hostname = "irc.twitch.tv";
    public string username = "youtheshepherd";
    public string pass = "oauth:0x2sdluao24dmajxwjgmopoi6fh5t3";
    

    public void Start() {
        channel = "#" + username;
        loginstring = "PASS "+pass+"\r\nNICK "+username+"\r\n";
        client = new TcpClient(hostname, port);
        stream = client.GetStream();

        Byte[] login = System.Text.Encoding.ASCII.GetBytes(loginstring);
        stream.Write(login, 0, login.Length);
        //Debug.Log("Sent login.\r\n");
        //Debug.Log(loginstring);

        // Receive the TcpServer.response.
        // Buffer to store the response bytes.
        data = new Byte[512];
        string responseData = String.Empty;
        Int32 bytes = stream.Read(data, 0, data.Length);
        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
        //Debug.Log("Received WELCOME: \r\n\r\n{0}"+ responseData);

        // send message to join channel
        string joinstring = "JOIN " + channel + "\r\n";
        Byte[] join = System.Text.Encoding.ASCII.GetBytes(joinstring);
        //Debug.Log("can write: "+ stream.CanWrite);
        stream.Write(join, 0, join.Length);
        stream.Flush();
        //Debug.Log("Sent channel join.\r\n");
        //Debug.Log(joinstring);

        // PMs the channel to announce that it's joined and listening
        // These three lines are the example for how to send something to the channel

        string announcestring = ":" + username + "!" + username + "@" + username + ".tmi.twitch.tv PRIVMSG " + channel + " : GAME STARTED !\r\n";
        Byte[] announce = System.Text.Encoding.ASCII.GetBytes(announcestring);
        stream.Write(announce, 0, announce.Length);
        stream.Flush();
        // Lets you know its working

        //Debug.Log("TWITCH CHAT HAS BEGUN.\r\n\r\nr.");
        //Debug.Log("\r\nBE CAREFUL.");
        StartCoroutine(messages());
    }
    int a = 0;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            a++;
            say(a.ToString());
            
        }
    }

    public void say(string str)
    {
        try
        {
            string format = ":" + username + "!" + username + "@" + username + ".tmi.twitch.tv PRIVMSG " + channel + " :" + str + "\r\n";

            Byte[] say = System.Text.Encoding.ASCII.GetBytes(format);
            stream.Write(say, 0, say.Length);
            stream.Flush();
            Debug.Log(str);
        }
        catch (Exception e)
        {
            Debug.Log("SOMETHING WENT WRONG\r\n" + e);
        }
    }

    
   IEnumerator messages () {

        yield return new WaitForSeconds(1f);
        while (listen)
        {
            byte[] myReadBuffer = new byte[1024];
            StringBuilder myCompleteMessage = new StringBuilder();
            int numberOfBytesRead = 0;

            while (stream.DataAvailable == false)
            {
                yield return 0;
            }

            // Incoming message may be larger than the buffer size.
            do
            {
                try
                {
                    
                    numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);
                }
                catch (Exception e)
                {
                    Debug.Log("OH SHIT SOMETHING WENT WRONG\r\n"+ e);
                }

                myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));
            }while (stream.DataAvailable);
            
            try
            {
                string messageParser = myCompleteMessage.ToString();
                string[] message = messageParser.Split(':');
                string[] preamble = message[1].Split(' ');
                string tochat;

                // This means it's a message to the channel.  Yes, PRIVMSG is IRC for messaging a channel too
                if (preamble[1] == "PRIVMSG")
                {
                    string[] sendingUser = preamble[0].Split('!');
                    tochat = sendingUser[0] + ": " + message[2];

                    // sometimes the carriage returns get lost (??)
                    if (tochat.Contains("\n") == false)
                    {
                        tochat = tochat + "\n";
                    }

                    // user and message data extracted
                    //Debug.Log(sendingUser[0]+": "+message[2]);
                    InputManager.Instance.command(sendingUser[0], message[2]);
                }
                /*else if (preamble[1] == "JOIN")       // do something when user join
                {
                    string[] sendingUser = preamble[0].Split('!');
                    tochat = "JOINED: " + sendingUser[0];
                    //    Debug.Log(tochat);
                    //SendKeys.SendWait(tochat.TrimEnd('\n'));
                    Debug.Log(tochat);
                }*/
            }
            catch (Exception e)
            {
                Debug.Log("SOMETHING WENT WRONG\r\n"+ e);
            }
            // Debug.Log("Raw output: " + message[0] + "::" + message[1] + "::" + message[2]);
            // Debug.Log("You received the following message : " + myCompleteMessage);
        }
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class Networking {
    const Int32 DEFAULT_PORT_TCP = 1337;
    const Int32 DEFAULT_PORT_UDP = 1338;
    const Int32 DEFAULT_PORT_TCP_LISTEN = 1339;
    const Int32 DEFAULT_PORT_UDP_LISTEN = 1340;
    String SERVER_HOSTNAME = "localhost";

    public static Byte[] recv_udp(String hostname, Int32 port)
    {
        try
        {
            UdpClient udp = new UdpClient(DEFAULT_PORT_TCP_LISTEN);

            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            Byte[] responseData = udp.Receive(ref RemoteIpEndPoint);
            String responseDataStr = Encoding.UTF8.GetString(responseData);

            Debug.Log("[INFO] Recieved data (UDP): <" + responseDataStr.ToString() + "> from " + RemoteIpEndPoint.Address.ToString() + ":" + RemoteIpEndPoint.Port.ToString());

            udp.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        return new Byte[0];
    }

    public static void send_udp(String hostname, Int32 port, Byte[] data)
    {
        try
        {
            String strData = Encoding.UTF8.GetString(data);
            UdpClient udp = new UdpClient();
            udp.Send(data, data.Length, hostname, port);
            Debug.Log("[INFO] Sent data (UDP): <" + strData.ToString() + "> to " + hostname + ":" + port);
            udp.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public static void recv_tcp(String hostname, Int32 port, Byte[] data)
    {
        try
        {
            TcpClient client = new TcpClient(hostname, port);

            NetworkStream stream = client.GetStream();

            data = new Byte[256];

            String responseData = String.Empty;

            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Debug.Log("[INFO] Received data (TCP): " + responseData);

            stream.Close();
            client.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public static void send_tcp(String hostname, Int32 port, Byte[] data)
    {
        try
        {
            TcpClient client = new TcpClient(hostname, port);

            NetworkStream stream = client.GetStream();

            stream.Write(data, 0, data.Length);
            String dataStr = Encoding.UTF8.GetString(data);
            Debug.Log("[INFO] Sent data (TCP): " + dataStr);

            stream.Close();
            client.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}

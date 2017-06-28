using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class Networking
{
    public const Int32 DEFAULT_PORT_TCP = 1337;
    public const Int32 DEFAULT_PORT_UDP = 1338;
    public const Int32 DEFAULT_PORT_TCP_LISTEN = 1339;
    public const Int32 DEFAULT_PORT_UDP_LISTEN = 1340;
    public const String DEFAULT_SERVER_HOSTNAME = "localhost";
    public const Int32 DEFAULT_NODE_LISTENER = 0;

    public static Byte[] recv_udp()
    {
        try
        {
            UdpClient udp = new UdpClient(DEFAULT_PORT_TCP_LISTEN);

            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            Byte[] responseData = udp.Receive(ref RemoteIpEndPoint);
            String responseDataStr = Encoding.UTF8.GetString(responseData);

            Debug.Log("[INFO] Recieved data (UDP): <" + responseDataStr.ToString() + "> from " + RemoteIpEndPoint.Address.ToString() + ":" + RemoteIpEndPoint.Port.ToString());

            udp.Close();

            Int32 listenerId = get_listener_id(responseData);
            return responseData;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        return new Byte[0];
    }

    public static void send_udp(String hostname, Int32 port, Byte[] data, Int32 listenerId)
    {
        data = mark_listener_id(data, listenerId);
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

    public static Byte[] recv_tcp(String hostname, Int32 port)
    {
        try
        {
            TcpClient client = new TcpClient(hostname, port);

            NetworkStream stream = client.GetStream();

            Byte[] data = new Byte[1024];

            String responseData = String.Empty;

            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Debug.Log("[INFO] Received data (TCP): " + responseData);

            stream.Close();
            client.Close();

            Int32 listenerId = get_listener_id(data);
            return data;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        return new Byte[0];
    }

    public static void send_tcp(String hostname, Int32 port, Byte[] data, Int32 listenerId)
    {
        data = mark_listener_id(data, listenerId);
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

    private static Byte[] mark_listener_id(Byte[] data, Int32 listenerId)
    {
        byte[] marked_data = new byte[data.Length + 1];
        data.CopyTo(marked_data, 1);
        marked_data[0] = Convert.ToByte(listenerId);
        // data = marked_data;
        return marked_data;
    }

    private static Int32 get_listener_id(Byte[] data)
    {
        return Convert.ToInt32(data[0]);
    }
}

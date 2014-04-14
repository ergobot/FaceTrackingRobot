using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;


// I think the original was at:
// http://msdn.microsoft.com/en-us/library/tst0kwb1.aspx

// Created by Sean OBryan
// 12/8/2012
// Email: globalw2865@gmail.com

namespace Calculon.Networking.UdpAlerts
{
    public class UdpBroadcast
    {
        public string Message { get; set; }
        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
        ProtocolType.Udp);
        public IPAddress broadcast { get; set; }

        public UdpBroadcast()
        {
            Message = string.Empty;
            broadcast = IPAddress.Parse("192.168.0.255");
        }
        public UdpBroadcast(string msg)
        {
            Message = msg;
            broadcast = IPAddress.Parse("192.168.0.255");
        }
        public UdpBroadcast(IPAddress ipTarget)
        {
            Message = string.Empty;
            broadcast = ipTarget;
        }
        public UdpBroadcast(string msg, IPAddress ipTarget)
        {
            Message = msg;
            broadcast = ipTarget;
        }

        public void Send()
        {
            byte[] sendbuf = Encoding.ASCII.GetBytes(Message);
            IPEndPoint ep = new IPEndPoint(broadcast, 11000);
            s.SendTo(sendbuf, ep);
        }
    }
}

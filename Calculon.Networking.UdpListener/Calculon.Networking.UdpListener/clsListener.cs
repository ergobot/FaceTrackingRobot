using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace Calculon.Networking.UdpListener
{
    public class clsListener
    {

        private const int listenPort = 11000;
        public clsListener()
        {
            //StartListener();
        }

        public void StartListener()
        {
            bool done = false;

            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

            try
            {
                while (!done)
                {
                    Console.WriteLine("Waiting for broadcast");
                    byte[] bytes = listener.Receive(ref groupEP);

                    Console.WriteLine("Received broadcast from {0} :\n {1}\n",
                        groupEP.ToString(),
                        Encoding.ASCII.GetString(bytes, 0, bytes.Length));

                    Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, bytes.Length));
                    Console.WriteLine(groupEP.Address.ToString() + ":8080");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                listener.Close();
            }
        }



    }
}

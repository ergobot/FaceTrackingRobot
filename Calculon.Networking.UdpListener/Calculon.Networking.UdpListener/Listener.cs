using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;


namespace Calculon.Networking.UdpListener
{
    public partial class Listener : Form
    {
        private const int listenPort = 11000;
        clsListener listener;
        Thread thread;
        ThreadStart threadStart;
        public Listener()
        {
            InitializeComponent();
            listener = new clsListener();
            threadStart = new ThreadStart(StartListener);
            thread = new Thread(threadStart);
            thread.Start();
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
                    //lblServerLink.Text = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    //Console.WriteLine(groupEP.Address.ToString() + ":8080");
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

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            StartListener();
        }


    }
}

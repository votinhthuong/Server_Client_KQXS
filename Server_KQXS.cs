using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server_KQXS
{
    class Program
    {
        private static int SendVarData(Socket s, byte[] data)
        {
            int total = 0;
            int size = data.Length;
            int dataleft = size;
            int sent;

            byte[] datasize = new byte[4];
            datasize = BitConverter.GetBytes(size);
            sent = s.Send(datasize);

            while (total < size)
            {
                sent = s.Send(data, total, dataleft, SocketFlags.None);
                total += sent;
                dataleft -= sent;
            }
            return total;
        }

        private static byte[] ReceiveVarData(Socket s)
        {
            int total = 0;
            int recv;
            byte[] datasize = new byte[4];

            recv = s.Receive(datasize, 0, 4, 0);
            int size = BitConverter.ToInt32(datasize, 0);
            int dataleft = size;
            byte[] data = new byte[size];


            while (total < size)
            {
                recv = s.Receive(data, total, dataleft, 0);
                if (recv == 0)
                {
                    data = Encoding.ASCII.GetBytes("exit");
                    break;
                }
                total += recv;
                dataleft -= recv;
            }
            return data;
        }

        public static void Main()
        {
            byte[] data = new byte[1024];
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);

            Socket newsock = new Socket(AddressFamily.InterNetwork,
                            SocketType.Stream, ProtocolType.Tcp);

            newsock.Bind(ipep);
            newsock.Listen(10);
            Console.WriteLine("Waiting for a client...");

            Socket client = newsock.Accept();
            IPEndPoint newclient = (IPEndPoint)client.RemoteEndPoint;
            Console.WriteLine("Connected with {0} at port {1}",
                            newclient.Address, newclient.Port);

            string welcome = "Welcome to my test server";
            data = Encoding.ASCII.GetBytes(welcome);
            int sent = SendVarData(client, data);
            string input;

            while (true)
            {
                data = ReceiveVarData(client);

                string ss = Encoding.ASCII.GetString(data);

                Console.WriteLine("Client: " + Encoding.ASCII.GetString(data));

                string[] lines = System.IO.File.ReadAllLines(@"D:\WriteText.txt");
                string kq="";
                foreach (string l in lines)
                {
                    if (ss != l)
                    {
                        kq+="Hen ban may man lan sau!";
                        break;
                    }

                    else
                    {
                        kq += "Chuc mung ban da trung so xo dac biet!!!";
                        break;
                    }
                        
                    

                }
                sent = SendVarData(client, Encoding.ASCII.GetBytes(kq));

            }
            Console.WriteLine("Disconnected from {0}", newclient.Address);
            client.Close();
            newsock.Close();
            Console.ReadLine();
        }
    }
}

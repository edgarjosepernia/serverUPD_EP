using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace UDP_EP
{
    public class Program
    {
        
        private string path;
        private string ip;
        private string data;
        private int port;
        
        static void Main(string[] args)
        {
            Program p = new Program();
            UDPSocket s = new UDPSocket();
            s.Server(p.ip, p.port);

            UDPSocket c = new UDPSocket();
            c.Client(p.ip, p.port);

            c.Send(p.ReadFile(p.path));

            Console.ReadKey();
        }

        public Program()
        {
            path = @"C:\Users\edgar\Documents\edgar\ENSAM\Ingenierie_RV_RA\ProjetSenior\Serveur_v1\UDP_EP\Coordenates.txt";
            ip = "127.0.0.1";
            data = "";
            port = 12345;
        }
        public string ReadFile(string path) 
        {
            string datafile = File.ReadAllText(path);
            return datafile;
        } 


    }
}

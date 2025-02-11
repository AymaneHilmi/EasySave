using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using EasySave;
using View;
using View.MVVM.View;

namespace EasySaveApp
{
    public class Server
    {
        public TcpListener _listener;
        public TcpClient _client;

        public Server(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            try
            {
                // Créer une instance de la classe TcpListener qui écoute sur l'adresse IP et le port spécifiés
                _listener = new TcpListener(IPAddress.Any, 12345);

                // Démarrer le serveur en commençant à écouter les connexions entrantes
                _listener.Start();

                // Lancer un thread pour gérer les connexions entrantes
                Thread acceptThread = new Thread(new ThreadStart(ListenForClients));
                acceptThread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Une erreur est survenue lors du démarrage du serveur : " + e.Message);
            }
        }


        public void Stop()
        {
            _listener.Stop();
        }
        
        public void ListenForClients()
        {
            string data;
            double lastprog = 0;
            // Attendre les connexions de clients
            while (true)
            {
                TcpClient client = _listener.AcceptTcpClient();

                _client = client;
                
                // Envoyer une réponse au client
                Task.Run(async () =>
                {
                    while (Save.globalProgression != 100)
                    {

                        if (lastprog != Save.globalProgression)
                        {
                            SendProgress(Save.globalProgression);
                        }
                        
                        lastprog = Save.globalProgression;
                        await Task.Delay(100); // Attendre 100 ms pour éviter une boucle infinie
                    }
                });

                Task.Run(async () =>
                {
                    while (true)
                    {
                        string data = await ReceiveDataAsync();
                        if (data != null)
                        {
                            if (data == "stop")
                            {
                                Save._stopped = true;
                            }
                            else if (data == "pause")
                            {
                                Save._paused = true;
                            }
                            else if (data == "play")
                            {
                                Save._paused = false;
                            }
                        }
                    }
                });


           
            }
        }
        public async Task<string> ReceiveDataAsync()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await _client.GetStream().ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead == 0)
            {
                // Le client a fermé la connexion
                return null;
            }
            else
            {
                string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                return data;
            }
        }

        public void SendProgress(double progress)
        {
            var stream = _client.GetStream();
            var buffer = BitConverter.GetBytes(progress);
            stream.Write(buffer, 0, buffer.Length);
        }

    }
}

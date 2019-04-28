using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace KeePassPipe
{
    class KeePassPipe
    {
        private static readonly string[] Switches = { "-t", "-u", "-p" };
        private static string ServerPipeName;

        private static void Main(string[] args) 
        {
            const int NumberOfDataLines = 3;
            string[] Lines=new string[NumberOfDataLines];
            
            try
            {
                if ((args.Length != 2) || (! Array.Exists(Switches, element => element.Equals(args[0]))))
                {
                    
                    Console.Error.WriteLine("Usage: KeePassPipe [-t|-u|-p] Title");
                    Environment.Exit(20010);
                    return;
                }
                                          
                
                ServerPipeName = "KeePassPipe." + Process.GetCurrentProcess().SessionId;
                NamedPipeClientStream Connection = new NamedPipeClientStream(
                    ".", ServerPipeName, PipeDirection.InOut, PipeOptions.None);
                Connection.Connect(2000);

                StreamReader Reader = new StreamReader(Connection, Encoding.UTF8);
                StreamWriter Writer = new StreamWriter(Connection, Encoding.UTF8);

                String SearchTitle = args[1];
                Writer.WriteLine(SearchTitle);
                Writer.Flush();
                Connection.Flush();

                for (int i = 0; i < NumberOfDataLines; i++)
                {
                    if (Reader.EndOfStream) break;
                    string Response = Reader.ReadLine();
                    Response = '"' + Response + '"';
                    Lines[i] = Response;
                }
                Connection.Dispose();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error: " + e.Message);
                Environment.Exit(20012);
                return;
            }
            int LineNumber = Array.IndexOf(Switches, args[0]);
            Console.Out.WriteLine(Lines[LineNumber]);
            Environment.Exit(0);
        }
        
    }
   
}

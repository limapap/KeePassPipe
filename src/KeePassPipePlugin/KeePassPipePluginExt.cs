using KeePass.Plugins;
using KeePassLib;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;


namespace KeePassPipePlugin 
{
    public sealed class KeePassPipePluginExt : Plugin
    {
        private IPluginHost KeePassHost = null;
        private string PipeName;
        private readonly object ThreadLock = new object();
        private Boolean Terminating = false;
                       
        public override bool Initialize(IPluginHost host)
        {
            if (host == null) return false; 
            KeePassHost = host;
            PipeName = "KeePassPipe." + Process.GetCurrentProcess().SessionId;

            Thread ServerThread = new Thread(ThreadNamedPipeServer);
            Terminating = false;
            ServerThread.Start(this);
            return true;
        }

        public override void Terminate()
        {
            Terminating = true;
            lock (ThreadLock)
            {
                try
                {
                    using (NamedPipeClientStream cs = new NamedPipeClientStream(
                        ".", PipeName, PipeDirection.InOut, PipeOptions.None))
                    {
                        cs.Connect(200);
                        Thread.Sleep(200);
                    }
                }
                catch { }
            }
            KeePassHost = null;
        }
        
        private void ThreadNamedPipeServer(object data)
        {
            NamedPipeServerStream PipeStream = null;
            try
            {
                while (!Terminating)
                {
                    lock (ThreadLock)
                    {
                        if (Terminating) return;

                        var sec = new PipeSecurity();
                        sec.AddAccessRule(
                            new PipeAccessRule(WindowsIdentity.GetCurrent().Name, PipeAccessRights.FullControl, 
                            AccessControlType.Allow));
                        PipeStream = new NamedPipeServerStream(
                            PipeName, PipeDirection.InOut,1,PipeTransmissionMode.Byte,PipeOptions.None, 0, 0, sec);
                    }

                    try
                    {
                        PipeStream.WaitForConnection();
                        if (Terminating)
                        {
                            PipeStream.Dispose();
                            return;
                        }
                        else
                        {
                            StreamReader Reader = new StreamReader(PipeStream, Encoding.UTF8);
                            StreamWriter Writer = new StreamWriter(PipeStream, Encoding.UTF8);

                            string SearchTitle = Reader.ReadLine();
                            string[] Lines = GetDataLines(SearchTitle);
                            foreach (string s in Lines)
                                Writer.WriteLine(s);
                            Writer.Flush();

                            PipeStream.Flush();
                            PipeStream.Close();
                            PipeStream.Dispose();
                        }
                    }
                    catch { }
                } 
            }
            catch { }
        }

        private string[] GetDataLines(string ASearchTitle)
        {
            const int NumberOfDataLines = 3;
            string[] Lines = new string[NumberOfDataLines] {"","",""};
            foreach (var doc in KeePassHost.MainWindow.DocumentManager.Documents)
            {
                PwDatabase db = doc.Database;
                if (db.IsOpen)
                {
                    var ItemList = db.RootGroup.GetObjects(true, true);
                    foreach (var Item in ItemList)
                    {
                        if (Item is PwEntry)
                        {
                            PwEntry Entry = Item as PwEntry;
                            string Title = Entry.Strings.ReadSafe("Title");
                            if (ASearchTitle.Equals(Title))
                            {
                                Lines[0] = Title;
                                Lines[1] = Entry.Strings.ReadSafe("UserName");
                                Lines[2] = Entry.Strings.ReadSafe("Password");
                                goto EntryFound;
                            }
                        }
                    }
                }
            }
            EntryFound:
            return Lines;
        }
    }
}

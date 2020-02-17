using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;

namespace GTM_Parse
{
    internal class GtmParse
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var text = string.Empty;
            if (Clipboard.ContainsText())
            {
                text = Clipboard.GetText();
            }
            else
            {
                var thing = Clipboard.GetDataObject();
                if (thing != null)
                    text = thing.ToString();
            }
            
            var index = text.IndexOf("/join/");
            if (index != -1)
            {
                index += 6;
                var gtmMeetingId = text.Substring(index, 9);
                var settings = UserSettings.GetSettings();
                if (!settings.GTMDirectory.EndsWith("\\"))
                    settings.GTMDirectory += "\\";

                var directoryInfo = new DirectoryInfo(settings.GTMDirectory);
                var mostRecent = directoryInfo.GetDirectories().OrderByDescending(x => x.CreationTime).FirstOrDefault();
                
                var builder = new StringBuilder();
                builder.AppendLine("@ECHO off");
                builder.AppendLine($"start /d \"{settings.GTMDirectory}{mostRecent.Name}\" g2mstart.exe \"/Action Join\" \"/MeetingID {gtmMeetingId}\"");
                if(File.Exists("gtm.bat"))
                    File.Delete("gtm.bat");
                File.WriteAllText("gtm.bat", builder.ToString());
                try
                {
                    Process proc = new Process
                    {
                        StartInfo =
                        {
                            FileName = "gtm.bat",
                            CreateNoWindow = false
                        }
                    };
                    proc.Start();
                    proc.WaitForExit();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                
                if(File.Exists("gtm.bat"))
                    File.Delete("gtm.bat");
            }
            else
            {
                MessageBox.Show("No meeting id found.");
            }
        }

    }
}
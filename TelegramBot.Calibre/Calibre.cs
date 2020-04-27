using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Calibre
{
    public static class Calibre
    {
        public static async void ConvertToMobi(string FilePath, string Email)
        {
            await Task.Run(() => {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = $@"/C ebook-convert ""{FilePath}"" ""{FilePath}.mobi""";
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            });
            Console.WriteLine("Converted");
            TelegramBot.Email.SendEMail.Send(Email, FilePath + ".mobi");
        }
    }
}

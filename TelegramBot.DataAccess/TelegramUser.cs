using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.DataAccess
{
    public class TelegramUser
    {
        public int TelegramUserId { get; set; }
        public long Chat { get; set; }
        public string Username { get; set; }
        public string EMail { get; set; }
        public int NrDownload { get; set; }
        public string LastCommand { get; set; }
    }
}

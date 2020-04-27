using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.DataAccess
{
    public class TelegramBotContext : DbContext
    {
        public virtual DbSet<TelegramUser> TelegramUsers { get; set; }
    }
}

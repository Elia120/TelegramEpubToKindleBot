using System;
using System.IO;
using System.Linq;
using System.Net;
using Telegram.Bot;
using TelegramBot.DataAccess;

namespace TelegramBot
{
    class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("1212561602:AAGmsaRNo7210JLMVgx8F8VwANyvyMZMUhI");
        static void Main(string[] args)
        {
            Bot.OnMessage += Bot_OnMessage;
            Bot.OnMessageEdited += Bot_OnMessage;


            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            TelegramUser TempUser;
            using (var context = new TelegramBotContext())
            {
                TempUser = context.TelegramUsers.Where(x => x.Chat == e.Message.Chat.Id).FirstOrDefault();
                if (TempUser==null)
                {
                    context.TelegramUsers.Add(new TelegramUser { Chat = e.Message.Chat.Id, LastCommand= String.Empty, Username = e.Message.Chat.Username });
                    context.SaveChanges();
                    TempUser = context.TelegramUsers.Where(x => x.Chat == e.Message.Chat.Id).FirstOrDefault();
                }

                if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
                {
                    if (TempUser.EMail != String.Empty)
                    {
                        DownloadFileConvertSend(e.Message.Document.FileId, @$"c:\temp\TelegramBot\{e.Message.Chat.Id}", $@"\{ e.Message.Document.FileName}", TempUser.EMail);
                        TempUser.NrDownload++;
                        context.SaveChanges();
                    }
                    else
                    {
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "Please add your Email first: /email");
                    }
                }
                else if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                {
                    if (e.Message.Text.Trim().ToLower()=="/email")
                    {
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "Please enter your Kindle E-Mail adress now:");
                        TempUser.LastCommand = e.Message.Text.Trim().ToLower();
                        context.SaveChanges();
                    }
                    else
                    {
                        if (TempUser.LastCommand=="/email")
                        {
                            TempUser.EMail = e.Message.Text.Trim().ToLower();
                            TempUser.LastCommand = String.Empty;
                            context.SaveChanges();
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Email adress saved: "+TempUser.EMail);
                        }
                        else
                        {
                            if (e.Message.Text.Trim().ToLower() == "/status")
                            {
                                Bot.SendTextMessageAsync(e.Message.Chat.Id,$"{TempUser.EMail},{TempUser.LastCommand},{TempUser.NrDownload},{TempUser.TelegramUserId},{TempUser.Username},{TempUser.Chat}" );
                            }
                            else
                            {
                                Bot.SendTextMessageAsync(e.Message.Chat.Id, @"Welcome to EpubToKindleBot. You can send me Ebooks and I will convert and send them to your Kindle. Please just remember to add epubtokindletelegrambot@gmail.com to your trusted divices on your Amazon Acoount. Use /email to add or change your kindle email adress");
                            }
                           
                        }
                    }
                    
                }
                else
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "Unknown Type");
                }
            }
        }
        private static async void DownloadFileConvertSend(string fileId, string FilePath,string FileName, string Email)
        {
            try
            {
                var file = await Bot.GetFileAsync(fileId);
                var download_url = @"https://api.telegram.org/file/bot1212561602:AAGmsaRNo7210JLMVgx8F8VwANyvyMZMUhI/" + file.FilePath;
                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }
                using (WebClient client = new WebClient())
                {
                   await client.DownloadFileTaskAsync(new Uri(download_url),FilePath+FileName);
                }
                Calibre.Calibre.ConvertToMobi(FilePath + FileName, Email);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error downloading: " + ex.Message);
            }

            
            Console.WriteLine("This should run first");
        }
        public static async void SendMessage(int ChatId, string Message)
        {
           await Bot.SendTextMessageAsync(ChatId, Message);
        }
    }
}

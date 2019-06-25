using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NiceToMeetYou.Models;
using NiceToMeetYou.Services;

namespace NiceToMeetYou.Modules
{
    
    public class ClassificationModule : ModuleBase<SocketCommandContext>
    {
        private static IEnumerable<IMessage> _messages = new List<IMessage>();
        public static ClassificationService ClassificationService { set; private get; }
      
        [Command("load")]
        public async Task Load(int amount)
        {
            var typingState = Context.Channel.EnterTypingState();
            
            _messages = _messages.Union(await Context.Channel.GetMessagesAsync(amount).FlattenAsync());

            var eb = new BotEmbedBuilder();
            eb.WithDescription("Messages fetched.");
            typingState.Dispose();
            await ReplyAsync("", false, eb.Build());
        }

        [Command("train")]
        public async Task Train()
        {
            ClassificationService.LoadMessages(_messages);
        }

        [Command("predict")]
        public async Task Predict(ulong messageID)
        {
            IMessage message = await Context.Channel.GetMessageAsync(messageID);
            SimpleTextMessage simpleMessage = new SimpleTextMessage
            {
                Content = message.Content
            };
            await ReplyAsync(Context.Guild.GetUser(ClassificationService.PredictUser(simpleMessage)).Username);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
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
    }
}
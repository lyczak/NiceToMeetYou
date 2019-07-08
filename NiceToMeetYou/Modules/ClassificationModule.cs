using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Net;
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
        public async Task Load(string channelMention)
        {
            var eb = new BotEmbedBuilder();
            string channelId = Regex.Match(channelMention, @"\d+").Value;

            SocketTextChannel textChannel = null;
            if (!string.IsNullOrEmpty(channelId))
            {
                textChannel = Context.Guild.GetTextChannel(ulong.Parse(channelId));
            }
            
            var typingState = Context.Channel.EnterTypingState();
            
            if (textChannel != null)
            {
                try
                {
                    _messages = _messages.Union(await textChannel.GetMessagesAsync(Int32.MaxValue).FlattenAsync());
                    eb.WithDescription("Messages fetched.");
                }
                catch (HttpException)
                {
                    eb.WithDescription("I can't access that text channel. Do I have permission to read messages there?");
                }
            }
            else
            {
                eb.WithDescription("I can't find that text channel. Try mentioning a valid text channel like this:" +
                                   "```#{Context.Channel.Name}```");
            }
            
            typingState.Dispose();
            await ReplyAsync("", false, eb.Build());
        }

        [Command("train")]
        public async Task Train()
        {
            var typingState = Context.Channel.EnterTypingState();
            
            ClassificationService.LoadMessages(_messages);
            
            typingState.Dispose();
            var eb = new BotEmbedBuilder();
            eb.WithDescription("Training complete.");
            await ReplyAsync("", false, eb.Build());
        }

        [Command("predict")]
        public async Task Predict([Remainder]String content)
        {
            SimpleTextMessage simpleMessage = new SimpleTextMessage
            {
                Content = content
            };
            var userId = ClassificationService.PredictUser(simpleMessage);
            var username = Context.Guild.GetUser(userId).Username;
            var eb = new BotEmbedBuilder();
            eb.WithDescription($"\"{content}\" - {username} (probably)");
            await ReplyAsync("", false, eb.Build());
        }
    }
}
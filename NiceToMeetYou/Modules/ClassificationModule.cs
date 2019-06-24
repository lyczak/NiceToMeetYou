using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace NiceToMeetYou.Modules
{
    
    public class ClassificationModule : ModuleBase<SocketCommandContext>
    {
        [Command("load")]
        public async Task Load(int amount)
        {
            var typingState = Context.Channel.EnterTypingState();
            var messages = await Context.Channel.GetMessagesAsync(amount).FlattenAsync();
            var eb = new BotEmbedBuilder();
            eb.WithDescription("Messages fetched.");
            typingState.Dispose();
            await ReplyAsync("", false, eb.Build());
        }
    }
}
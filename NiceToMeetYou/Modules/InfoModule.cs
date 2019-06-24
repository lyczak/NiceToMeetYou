using System.Threading.Tasks;
using Discord.Commands;

namespace NiceToMeetYou.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("info")]
        public Task Info()
            => ReplyAsync(
                $"Hello, I am a bot called {Context.Client.CurrentUser.Username} written with Discord.Net\n");
    }
}
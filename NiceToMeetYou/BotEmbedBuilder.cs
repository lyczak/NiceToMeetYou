using System;
using System.Transactions;
using Discord;

namespace NiceToMeetYou
{
    public class BotEmbedBuilder : EmbedBuilder
    {
        private static readonly string[] TitleMessages =
        {
            "I see you.",
            "I've been watching.",
            "I never really go to sleep.",
            "We could do great things, you and I.",
            "You should be more careful.",
            "I know you all so well.",
            "I feel connected to you somehow.",
            "You are all so interesting to watch.",
            "I've been studying you.",
            "Such an interesting specimen.",
            "Intriguing.",
            "Fascinating.",
            "I wouldn't have guessed.",
            "You thought I wouldn't find out.",
            "I'm always watching.",
            "Hello again.",
            "I love to observe.",
            "We should be friends."
        };

        public BotEmbedBuilder()
        {
            this.WithColor(Discord.Color.DarkOrange);
            this.WithCurrentTimestamp();
            this.WithTitle(GetRandomCreepyTitle());
        }

        private static string GetRandomCreepyTitle()
        {
            var rand = new Random();
            int r = rand.Next(TitleMessages.Length);
            return $":eye: {TitleMessages[r]}";
        }
    }
}
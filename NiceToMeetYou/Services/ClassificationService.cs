using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;
using Microsoft.ML;
using NiceToMeetYou.Models;

namespace NiceToMeetYou.Services
{
    public class ClassificationService
    {
        private static MLContext _context;
        private static PredictionEngine<SimpleTextMessage, SimpleMessagePrediction> _predEngine;

        public ClassificationService()
        {
            _context = new MLContext();
        }

        public void LoadMessages(IEnumerable<SocketMessage> messages)
        {
            IEnumerable<SimpleTextMessage> simpleMessages = messages.Select(m =>
                new SimpleTextMessage
                {
                    Id = m.Author.Id,
                    Content = m.Content
                });
            
            _context.Data.LoadFromEnumerable(simpleMessages);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using Microsoft.ML;
using Microsoft.ML.Data;
using NiceToMeetYou.Models;

namespace NiceToMeetYou.Services
{
    public class ClassificationService
    {
        private static MLContext _context;
        private static PredictionEngine<SimpleTextMessage, SimpleMessagePrediction> _predEngine;
        private static IDataView _trainingData;
        private static ITransformer _trainedModel;

        public ClassificationService()
        {
            _context = new MLContext();
        }

        public void LoadMessages(IEnumerable<IMessage> messages)
        {
            IEnumerable<SimpleTextMessage> simpleMessages = messages.Select(m =>
                new SimpleTextMessage
                {
                    Id = m.Author.Id,
                    Content = m.Content
                });
            
            _trainingData = _context.Data.LoadFromEnumerable(simpleMessages);
            BuildAndTrain(FeaturizeAndTransform());
        }

        private IEstimator<ITransformer> FeaturizeAndTransform()
        {
            var pipeline = _context.Transforms.Conversion
                .MapValueToKey("Label", nameof(SimpleTextMessage.Id))
                .Append(_context.Transforms.Text.FeaturizeText(
                    "Features", nameof(SimpleTextMessage.Content)));
            return pipeline;
        }

        private void BuildAndTrain(IEstimator<ITransformer> pipeline)
        {
            var trainingPipeline = pipeline.Append(_context.MulticlassClassification.Trainers
                    .SdcaMaximumEntropy("Label", "Features"))
                .Append(_context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            _trainedModel = trainingPipeline.Fit(_trainingData);
        }

        public ulong PredictUser(SimpleTextMessage message)
        {
            PredictionEngine<SimpleTextMessage, SimpleMessagePrediction> predictionEngine =
                _context.Model.CreatePredictionEngine<SimpleTextMessage, SimpleMessagePrediction>(_trainedModel);
            SimpleMessagePrediction prediction = predictionEngine.Predict(message);
            return prediction.Id;
        }
    }
}
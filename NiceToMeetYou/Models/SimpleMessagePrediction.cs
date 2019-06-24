using Microsoft.ML.Data;

namespace NiceToMeetYou.Models
{
    public class SimpleMessagePrediction
    {
        [ColumnName("PredictedLabel")]
        public ulong Id { get; set; }
    }
}
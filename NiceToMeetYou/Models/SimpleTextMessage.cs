using Microsoft.ML.Data;

namespace NiceToMeetYou.Models
{
    public class SimpleTextMessage
    {
        [ColumnName("Id")]
        public ulong Id { get; set; }
        [ColumnName("Content")]
        public string Content { get; set; }
    }
}
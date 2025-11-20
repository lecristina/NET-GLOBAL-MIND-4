using Microsoft.ML.Data;

namespace nexus.Services.ML.Models
{
    /// <summary>
    /// Classe de dados para entrada do modelo ML.NET
    /// </summary>
    public class SentimentInput
    {
        [LoadColumn(0)]
        public string Text { get; set; } = string.Empty;

        [LoadColumn(1)]
        public bool Label { get; set; }
    }

    /// <summary>
    /// Classe de dados para predição do modelo ML.NET
    /// </summary>
    public class SentimentPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        [ColumnName("Probability")]
        public float Probability { get; set; }

        [ColumnName("Score")]
        public float Score { get; set; }
    }

    /// <summary>
    /// Classe para dados de sentimento processados
    /// </summary>
    public class ProcessedSentimentData
    {
        public string OriginalText { get; set; } = string.Empty;
        public string ProcessedText { get; set; } = string.Empty;
        public List<string> Tokens { get; set; } = new List<string>();
        public Dictionary<string, int> WordFrequency { get; set; } = new Dictionary<string, int>();
    }
}


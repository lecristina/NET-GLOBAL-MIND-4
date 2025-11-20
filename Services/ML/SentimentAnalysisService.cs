using Microsoft.ML;
using Microsoft.ML.Data;
using nexus.Models.DTOs;

namespace nexus.Services.ML
{
    /// <summary>
    /// Serviço de análise de sentimento usando ML.NET
    /// Implementa IA Generativa para análise de texto e geração de recomendações
    /// </summary>
    public class SentimentAnalysisService : ISentimentAnalysisService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;
        private readonly ILogger<SentimentAnalysisService> _logger;

        public SentimentAnalysisService(ILogger<SentimentAnalysisService> logger)
        {
            _logger = logger;
            _mlContext = new MLContext(seed: 0);
            
            // Criar modelo simples de análise de sentimento baseado em palavras-chave
            // Em produção, isso seria treinado com um dataset real
            _model = CriarModeloBasico();
        }

        /// <summary>
        /// Analisa o sentimento de um texto e gera recomendações personalizadas
        /// </summary>
        public async Task<AnaliseSentimentoResponseDto> AnalisarSentimentoAsync(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return new AnaliseSentimentoResponseDto
                {
                    Sentimento = "Neutro",
                    Score = 0.5,
                    NivelRisco = 3,
                    Mensagem = "Texto vazio ou inválido",
                    Recomendacoes = new List<string> { "Forneça mais informações para uma análise precisa" }
                };
            }

            try
            {
                // Análise de sentimento baseada em palavras-chave e padrões
                var sentimento = AnalisarSentimentoBasico(texto);
                var score = CalcularScoreSentimento(texto, sentimento);
                var nivelRisco = CalcularNivelRisco(sentimento, score);
                var recomendacoes = GerarRecomendacoes(texto, sentimento, nivelRisco);
                var mensagem = GerarMensagemPersonalizada(sentimento, nivelRisco);

                _logger.LogInformation("Análise de sentimento concluída: {Sentimento}, Score: {Score}, Risco: {Risco}", 
                    sentimento, score, nivelRisco);

                return await Task.FromResult(new AnaliseSentimentoResponseDto
                {
                    Sentimento = sentimento,
                    Score = score,
                    NivelRisco = nivelRisco,
                    Mensagem = mensagem,
                    Recomendacoes = recomendacoes
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao analisar sentimento do texto");
                throw;
            }
        }

        /// <summary>
        /// Analisa múltiplos textos e retorna análise agregada
        /// </summary>
        public async Task<AnaliseSentimentoResponseDto> AnalisarSentimentosAsync(IEnumerable<string> textos)
        {
            var textosList = textos.Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
            
            if (!textosList.Any())
            {
                return new AnaliseSentimentoResponseDto
                {
                    Sentimento = "Neutro",
                    Score = 0.5,
                    NivelRisco = 3,
                    Mensagem = "Nenhum texto válido fornecido",
                    Recomendacoes = new List<string>()
                };
            }

            var analises = new List<AnaliseSentimentoResponseDto>();
            foreach (var texto in textosList)
            {
                analises.Add(await AnalisarSentimentoAsync(texto));
            }

            // Agregar resultados
            var sentimentoMedio = CalcularSentimentoMedio(analises);
            var scoreMedio = analises.Average(a => a.Score);
            var nivelRiscoMaximo = analises.Max(a => a.NivelRisco);
            var todasRecomendacoes = analises.SelectMany(a => a.Recomendacoes).Distinct().ToList();

            return new AnaliseSentimentoResponseDto
            {
                Sentimento = sentimentoMedio,
                Score = scoreMedio,
                NivelRisco = nivelRiscoMaximo,
                Mensagem = $"Análise agregada de {textosList.Count} textos. Sentimento geral: {sentimentoMedio}",
                Recomendacoes = todasRecomendacoes
            };
        }

        #region Métodos Privados

        private ITransformer CriarModeloBasico()
        {
            // Modelo básico para demonstração
            // Em produção, seria carregado de um arquivo .zip treinado
            var data = _mlContext.Data.LoadFromEnumerable(new List<SentimentData>());
            var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", "Text");
            return pipeline.Fit(data);
        }

        private string AnalisarSentimentoBasico(string texto)
        {
            var textoLower = texto.ToLowerInvariant();
            
            // Palavras-chave positivas
            var palavrasPositivas = new[] { "bom", "ótimo", "excelente", "feliz", "satisfeito", "energizado", 
                "motivado", "produtivo", "bem", "ótima", "melhor", "ótimo dia", "bom dia", "satisfação" };
            
            // Palavras-chave negativas
            var palavrasNegativas = new[] { "ruim", "péssimo", "cansado", "estressado", "sobrecarregado", 
                "exausto", "frustrado", "ansioso", "preocupado", "mal", "difícil", "problema", "burnout", 
                "esgotado", "desanimado", "deprimido" };

            var countPositivo = palavrasPositivas.Count(p => textoLower.Contains(p));
            var countNegativo = palavrasNegativas.Count(p => textoLower.Contains(p));

            if (countPositivo > countNegativo && countPositivo > 0)
                return "Positivo";
            else if (countNegativo > countPositivo && countNegativo > 0)
                return "Negativo";
            else
                return "Neutro";
        }

        private double CalcularScoreSentimento(string texto, string sentimento)
        {
            var textoLower = texto.ToLowerInvariant();
            double score = 0.5; // Neutro por padrão

            if (sentimento == "Positivo")
            {
                score = 0.6 + (texto.Length > 50 ? 0.2 : 0.1);
                // Aumentar score baseado em palavras muito positivas
                if (textoLower.Contains("excelente") || textoLower.Contains("ótimo") || textoLower.Contains("perfeito"))
                    score = Math.Min(0.95, score + 0.15);
            }
            else if (sentimento == "Negativo")
            {
                score = 0.4 - (texto.Length > 50 ? 0.2 : 0.1);
                // Diminuir score baseado em palavras muito negativas
                if (textoLower.Contains("péssimo") || textoLower.Contains("esgotado") || textoLower.Contains("burnout"))
                    score = Math.Max(0.05, score - 0.15);
            }

            return Math.Clamp(score, 0.0, 1.0);
        }

        private int CalcularNivelRisco(string sentimento, double score)
        {
            if (sentimento == "Negativo" && score < 0.3)
                return 5; // Risco muito alto
            else if (sentimento == "Negativo" && score < 0.4)
                return 4; // Risco alto
            else if (sentimento == "Negativo")
                return 3; // Risco médio
            else if (sentimento == "Neutro")
                return 2; // Risco baixo
            else
                return 1; // Risco muito baixo (positivo)
        }

        private List<string> GerarRecomendacoes(string texto, string sentimento, int nivelRisco)
        {
            var recomendacoes = new List<string>();
            var textoLower = texto.ToLowerInvariant();

            if (nivelRisco >= 4)
            {
                recomendacoes.Add("⚠️ Risco elevado detectado. Considere fazer uma pausa imediata.");
                recomendacoes.Add("💬 Recomendamos conversar com seu gestor ou equipe de RH sobre seu bem-estar.");
                recomendacoes.Add("🧘 Pratique técnicas de relaxamento e respiração.");
                recomendacoes.Add("⏰ Revise sua carga de trabalho e priorize tarefas essenciais.");
            }
            else if (nivelRisco == 3)
            {
                recomendacoes.Add("📊 Monitore seu bem-estar regularmente.");
                recomendacoes.Add("💧 Mantenha-se hidratado e faça pausas regulares.");
                recomendacoes.Add("🏃 Pratique atividades físicas leves para reduzir o estresse.");
            }
            else if (sentimento == "Positivo")
            {
                recomendacoes.Add("✅ Continue mantendo esse equilíbrio!");
                recomendacoes.Add("📝 Registre o que está funcionando bem para você.");
                recomendacoes.Add("🤝 Compartilhe suas práticas saudáveis com a equipe.");
            }
            else
            {
                recomendacoes.Add("📈 Mantenha o monitoramento regular do seu bem-estar.");
                recomendacoes.Add("🎯 Foque em manter um equilíbrio entre trabalho e descanso.");
            }

            // Recomendações específicas baseadas em palavras-chave
            if (textoLower.Contains("cansado") || textoLower.Contains("exausto"))
            {
                recomendacoes.Add("😴 Priorize uma boa noite de sono (7-9 horas).");
            }

            if (textoLower.Contains("estressado") || textoLower.Contains("ansioso"))
            {
                recomendacoes.Add("🧘 Experimente meditação ou mindfulness por 10 minutos diários.");
            }

            if (textoLower.Contains("sobrecarregado") || textoLower.Contains("muitas tarefas"))
            {
                recomendacoes.Add("📋 Use técnicas de priorização (Matriz de Eisenhower).");
                recomendacoes.Add("🗣️ Comunique-se com seu gestor sobre a carga de trabalho.");
            }

            return recomendacoes.Distinct().ToList();
        }

        private string GerarMensagemPersonalizada(string sentimento, int nivelRisco)
        {
            return sentimento switch
            {
                "Positivo" => nivelRisco == 1 
                    ? "Ótimo! Você está se sentindo bem e equilibrado. Continue assim! 😊"
                    : "Você está se sentindo bem. Mantenha esse ritmo positivo! 👍",
                
                "Negativo" => nivelRisco >= 4
                    ? "Detectamos sinais de preocupação no seu bem-estar. É importante cuidar de si mesmo. Considere fazer uma pausa e buscar apoio. 💙"
                    : nivelRisco == 3
                    ? "Notamos alguns sinais de desconforto. Fique atento ao seu bem-estar e não hesite em buscar ajuda se necessário. 🤗"
                    : "Você mencionou alguns desafios. Lembre-se de cuidar de si mesmo e manter o equilíbrio. 💪",
                
                _ => "Seu estado emocional parece neutro. Continue monitorando seu bem-estar regularmente. 📊"
            };
        }

        private string CalcularSentimentoMedio(List<AnaliseSentimentoResponseDto> analises)
        {
            var positivos = analises.Count(a => a.Sentimento == "Positivo");
            var negativos = analises.Count(a => a.Sentimento == "Negativo");
            var neutros = analises.Count(a => a.Sentimento == "Neutro");

            if (positivos > negativos && positivos > neutros)
                return "Positivo";
            else if (negativos > positivos && negativos > neutros)
                return "Negativo";
            else
                return "Neutro";
        }

        #endregion

        #region Classes Auxiliares

        private class SentimentData
        {
            public string Text { get; set; } = string.Empty;
        }

        #endregion
    }
}


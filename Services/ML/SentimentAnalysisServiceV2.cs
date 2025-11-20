using Microsoft.ML;
using Microsoft.ML.Data;
using nexus.Models.DTOs;
using nexus.Services.ML.Models;

namespace nexus.Services.ML
{
    /// <summary>
    /// Serviço melhorado de análise de sentimento usando ML.NET treinado e NLP
    /// Versão 2 com modelo treinado e técnicas avançadas de processamento de texto
    /// </summary>
    public class SentimentAnalysisServiceV2 : ISentimentAnalysisService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer? _modeloTreinado;
        private readonly NLPService _nlpService;
        private readonly MLModelTrainer _modelTrainer;
        private readonly ILogger<SentimentAnalysisServiceV2> _logger;
        private readonly bool _usaModeloTreinado;

        public SentimentAnalysisServiceV2(
            NLPService nlpService,
            MLModelTrainer modelTrainer,
            ILogger<SentimentAnalysisServiceV2> logger)
        {
            _mlContext = new MLContext(seed: 0);
            _nlpService = nlpService;
            _modelTrainer = modelTrainer;
            _logger = logger;

            // Tentar carregar modelo treinado
            _modeloTreinado = _modelTrainer.CarregarModelo();
            _usaModeloTreinado = _modeloTreinado != null;

            if (_usaModeloTreinado)
            {
                _logger.LogInformation("✅ Modelo ML.NET treinado carregado com sucesso");
            }
            else
            {
                _logger.LogWarning("⚠️ Modelo treinado não encontrado. Usando análise baseada em NLP melhorada.");
            }
        }

        /// <summary>
        /// Analisa o sentimento de um texto usando modelo treinado ou NLP melhorado
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
                string sentimento;
                double score;
                bool? predicaoModelo = null;
                float probabilidadeModelo = 0.5f;

                // Tentar usar modelo treinado primeiro
                if (_usaModeloTreinado && _modeloTreinado != null)
                {
                    var resultadoModelo = UsarModeloTreinado(texto);
                    predicaoModelo = resultadoModelo.Prediction;
                    probabilidadeModelo = resultadoModelo.Probability;
                    
                    sentimento = predicaoModelo.Value ? "Positivo" : "Negativo";
                    score = probabilidadeModelo;
                    
                    _logger.LogInformation("Análise usando modelo ML.NET treinado. Probabilidade: {Prob:P2}", probabilidadeModelo);
                }
                else
                {
                    // Usar NLP melhorado como fallback
                    var (sentimentoNLP, scoreNLP) = _nlpService.DetectarSentimentoBasico(texto);
                    sentimento = sentimentoNLP;
                    score = scoreNLP;
                    
                    _logger.LogInformation("Análise usando NLP melhorado. Score: {Score:F2}", score);
                }

                // Ajustar score baseado em características adicionais
                var caracteristicas = _nlpService.ExtrairCaracteristicas(texto);
                score = AjustarScoreComCaracteristicas(score, caracteristicas, sentimento);

                var nivelRisco = CalcularNivelRisco(sentimento, score);
                var recomendacoes = GerarRecomendacoes(texto, sentimento, nivelRisco, caracteristicas);
                var mensagem = GerarMensagemPersonalizada(sentimento, nivelRisco, score);

                _logger.LogInformation("Análise concluída: {Sentimento}, Score: {Score:F2}, Risco: {Risco}", 
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

            // Agregar resultados com média ponderada
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

        private SentimentPrediction UsarModeloTreinado(string texto)
        {
            if (_modeloTreinado == null)
                throw new InvalidOperationException("Modelo não está carregado");

            var input = new SentimentInput { Text = texto, Label = false };
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<SentimentInput, SentimentPrediction>(_modeloTreinado);
            return predictionEngine.Predict(input);
        }

        private double AjustarScoreComCaracteristicas(double scoreBase, Dictionary<string, object> caracteristicas, string sentimento)
        {
            var scoreAjustado = scoreBase;

            // Ajustar baseado no comprimento do texto (textos mais longos são mais confiáveis)
            var comprimento = (int)caracteristicas["ComprimentoTexto"];
            if (comprimento > 100)
                scoreAjustado += 0.05;
            else if (comprimento < 20)
                scoreAjustado -= 0.05;

            // Ajustar baseado no número de palavras significativas
            var palavrasSignificativas = (int)caracteristicas["NumeroPalavrasSignificativas"];
            if (palavrasSignificativas > 10)
                scoreAjustado += 0.03;
            else if (palavrasSignificativas < 3)
                scoreAjustado -= 0.03;

            return Math.Clamp(scoreAjustado, 0.0, 1.0);
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

        private List<string> GerarRecomendacoes(string texto, string sentimento, int nivelRisco, Dictionary<string, object> caracteristicas)
        {
            var recomendacoes = new List<string>();
            var textoProcessado = _nlpService.ProcessarTexto(texto);
            var tokens = _nlpService.Tokenizar(textoProcessado);

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

            // Recomendações específicas baseadas em análise NLP
            if (tokens.Any(t => t.Contains("cans") || t.Contains("exaust")))
            {
                recomendacoes.Add("😴 Priorize uma boa noite de sono (7-9 horas).");
            }

            if (tokens.Any(t => t.Contains("estress") || t.Contains("ansios")))
            {
                recomendacoes.Add("🧘 Experimente meditação ou mindfulness por 10 minutos diários.");
            }

            if (tokens.Any(t => t.Contains("sobrecarg") || t.Contains("muitas")))
            {
                recomendacoes.Add("📋 Use técnicas de priorização (Matriz de Eisenhower).");
                recomendacoes.Add("🗣️ Comunique-se com seu gestor sobre a carga de trabalho.");
            }

            return recomendacoes.Distinct().ToList();
        }

        private string GerarMensagemPersonalizada(string sentimento, int nivelRisco, double score)
        {
            var confianca = score > 0.7 || score < 0.3 ? "alta" : "média";
            
            return sentimento switch
            {
                "Positivo" => nivelRisco == 1 
                    ? $"Ótimo! Você está se sentindo bem e equilibrado (confiança: {confianca}). Continue assim! 😊"
                    : $"Você está se sentindo bem. Mantenha esse ritmo positivo! 👍",
                
                "Negativo" => nivelRisco >= 4
                    ? $"Detectamos sinais de preocupação no seu bem-estar (confiança: {confianca}). É importante cuidar de si mesmo. Considere fazer uma pausa e buscar apoio. 💙"
                    : nivelRisco == 3
                    ? $"Notamos alguns sinais de desconforto. Fique atento ao seu bem-estar e não hesite em buscar ajuda se necessário. 🤗"
                    : $"Você mencionou alguns desafios. Lembre-se de cuidar de si mesmo e manter o equilíbrio. 💪",
                
                _ => $"Seu estado emocional parece neutro. Continue monitorando seu bem-estar regularmente. 📊"
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
    }
}


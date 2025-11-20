using nexus.Data;
using nexus.Models.DTOs;
using nexus.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace nexus.Services.ML
{
    /// <summary>
    /// Serviço de análise completa de bem-estar
    /// Integra análise de sentimento, produtividade e gera alertas inteligentes
    /// </summary>
    public class WellnessAnalysisService : IWellnessAnalysisService
    {
        private readonly ISentimentAnalysisService _sentimentService;
        private readonly ILogger<WellnessAnalysisService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IAlertaIARepository _alertaIARepository;

        public WellnessAnalysisService(
            ISentimentAnalysisService sentimentService,
            ILogger<WellnessAnalysisService> logger,
            ApplicationDbContext context,
            IAlertaIARepository alertaIARepository)
        {
            _sentimentService = sentimentService;
            _logger = logger;
            _context = context;
            _alertaIARepository = alertaIARepository;
        }

        /// <summary>
        /// Realiza análise completa de bem-estar do usuário
        /// </summary>
        public async Task<AnaliseBemEstarCompletaDto> AnalisarBemEstarCompletoAsync(int idUsuario)
        {
            try
            {
                // Buscar dados do usuário
                var humores = await _context.Humores
                    .Where(h => h.IdUsuario == idUsuario)
                    .OrderByDescending(h => h.DataRegistro)
                    .Take(10)
                    .ToListAsync();

                var sprints = await _context.Sprints
                    .Where(s => s.IdUsuario == idUsuario)
                    .OrderByDescending(s => s.DataInicio)
                    .Take(5)
                    .ToListAsync();

                // Análise de sentimento dos comentários de humor
                AnaliseSentimentoResponseDto? analiseSentimento = null;
                var comentarios = humores
                    .Where(h => !string.IsNullOrWhiteSpace(h.Comentario))
                    .Select(h => h.Comentario!)
                    .ToList();

                if (comentarios.Any())
                {
                    analiseSentimento = await _sentimentService.AnalisarSentimentosAsync(comentarios);
                }

                // Análise de produtividade
                AnaliseProdutividadeDto? analiseProdutividade = null;
                if (sprints.Any())
                {
                    var produtividades = sprints.Where(s => s.Produtividade.HasValue).Select(s => (double)s.Produtividade!.Value).ToList();
                    var mediaProdutividade = produtividades.Any() ? produtividades.Average() : 0.0;
                    var tendencia = CalcularTendenciaProdutividade(sprints);
                    var analisePadroes = AnalisarPadroesProdutividade(sprints, humores);

                    analiseProdutividade = new AnaliseProdutividadeDto
                    {
                        MediaProdutividade = mediaProdutividade,
                        Tendencia = tendencia,
                        AnalisePadroes = analisePadroes
                    };
                }

                // Gerar alertas inteligentes
                var alertas = await GerarAlertasInteligentesAsync(idUsuario);

                // Calcular score geral de bem-estar
                var scoreBemEstar = CalcularScoreBemEstar(humores, sprints, analiseSentimento);

                // Gerar recomendações gerais
                var recomendacoesGerais = GerarRecomendacoesGerais(analiseSentimento, analiseProdutividade, scoreBemEstar);

                _logger.LogInformation("Análise completa de bem-estar concluída para usuário {IdUsuario}. Score: {Score}", 
                    idUsuario, scoreBemEstar);

                return new AnaliseBemEstarCompletaDto
                {
                    IdUsuario = idUsuario,
                    AnaliseSentimento = analiseSentimento,
                    AnaliseProdutividade = analiseProdutividade,
                    Alertas = alertas,
                    ScoreBemEstar = scoreBemEstar,
                    RecomendacoesGerais = recomendacoesGerais,
                    DataAnalise = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao analisar bem-estar completo do usuário {IdUsuario}", idUsuario);
                throw;
            }
        }

        /// <summary>
        /// Gera alertas inteligentes baseados nos dados do usuário
        /// </summary>
        public async Task<List<AlertaIAGeradoDto>> GerarAlertasInteligentesAsync(int idUsuario)
        {
            var alertas = new List<AlertaIAGeradoDto>();

            try
            {
                // Buscar dados recentes
                var humoresRecentes = await _context.Humores
                    .Where(h => h.IdUsuario == idUsuario)
                    .OrderByDescending(h => h.DataRegistro)
                    .Take(7)
                    .ToListAsync();

                var sprintsRecentes = await _context.Sprints
                    .Where(s => s.IdUsuario == idUsuario)
                    .OrderByDescending(s => s.DataInicio)
                    .Take(3)
                    .ToListAsync();

                if (!humoresRecentes.Any())
                {
                    alertas.Add(new AlertaIAGeradoDto
                    {
                        TipoAlerta = "Informação",
                        Mensagem = "Comece a registrar seu humor regularmente para receber análises personalizadas.",
                        NivelRisco = 1,
                        Prioridade = "Baixa"
                    });
                    return alertas;
                }

                // Análise de padrões de humor
                var mediaHumor = humoresRecentes.Average(h => h.NivelHumor);
                var mediaEnergia = humoresRecentes.Average(h => h.NivelEnergia);
                var tendenciaHumor = CalcularTendenciaHumor(humoresRecentes);

                // Alerta de burnout (baixo humor + baixa energia + alta produtividade)
                var produtividadesRecentes = sprintsRecentes.Where(s => s.Produtividade.HasValue).Select(s => (double)s.Produtividade!.Value).ToList();
                var mediaProdutividadeRecente = produtividadesRecentes.Any() ? produtividadesRecentes.Average() : 0.0;
                
                if (mediaHumor <= 2 && mediaEnergia <= 2 && mediaProdutividadeRecente > 80)
                {
                    alertas.Add(new AlertaIAGeradoDto
                    {
                        TipoAlerta = "Burnout",
                        Mensagem = "⚠️ Sinais de possível burnout detectados: baixo humor e energia com alta produtividade. Considere fazer uma pausa e buscar apoio.",
                        NivelRisco = 5,
                        Prioridade = "Alta"
                    });
                }

                // Alerta de sobrecarga
                var tarefasRecentes = sprintsRecentes.Where(s => s.TarefasConcluidas.HasValue).Select(s => s.TarefasConcluidas!.Value).ToList();
                var mediaTarefasRecentes = tarefasRecentes.Any() ? tarefasRecentes.Average() : 0.0;
                
                if (mediaTarefasRecentes > 15 && mediaHumor <= 3)
                {
                    alertas.Add(new AlertaIAGeradoDto
                    {
                        TipoAlerta = "Sobrecarga",
                        Mensagem = "📊 Muitas tarefas concluídas com humor baixo. Considere revisar sua carga de trabalho.",
                        NivelRisco = 4,
                        Prioridade = "Média"
                    });
                }

                // Alerta de tendência negativa
                if (tendenciaHumor == "Diminuindo" && mediaHumor <= 3)
                {
                    alertas.Add(new AlertaIAGeradoDto
                    {
                        TipoAlerta = "Tendência Negativa",
                        Mensagem = "📉 Tendência de declínio no bem-estar detectada. Fique atento e cuide de si mesmo.",
                        NivelRisco = 3,
                        Prioridade = "Média"
                    });
                }

                // Análise de sentimento dos comentários
                var comentarios = humoresRecentes
                    .Where(h => !string.IsNullOrWhiteSpace(h.Comentario))
                    .Select(h => h.Comentario!)
                    .ToList();

                if (comentarios.Any())
                {
                    var analiseSentimento = await _sentimentService.AnalisarSentimentosAsync(comentarios);
                    
                    if (analiseSentimento.NivelRisco >= 4)
                    {
                        alertas.Add(new AlertaIAGeradoDto
                        {
                            TipoAlerta = "Sentimento Negativo",
                            Mensagem = $"💬 Análise de sentimento: {analiseSentimento.Mensagem}",
                            NivelRisco = analiseSentimento.NivelRisco,
                            Prioridade = analiseSentimento.NivelRisco >= 5 ? "Alta" : "Média"
                        });
                    }
                }

                // Alerta positivo (quando está bem)
                if (mediaHumor >= 4 && mediaEnergia >= 4 && tendenciaHumor == "Aumentando")
                {
                    alertas.Add(new AlertaIAGeradoDto
                    {
                        TipoAlerta = "Equilíbrio",
                        Mensagem = "✅ Excelente! Você está mantendo um bom equilíbrio entre trabalho e bem-estar. Continue assim!",
                        NivelRisco = 1,
                        Prioridade = "Baixa"
                    });
                }

                return alertas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar alertas inteligentes para usuário {IdUsuario}", idUsuario);
                return alertas;
            }
        }

        #region Métodos Privados

        private string CalcularTendenciaProdutividade(List<nexus.Models.Sprint> sprints)
        {
            if (sprints.Count < 2)
                return "Estável";

            var produtividades = sprints
                .Where(s => s.Produtividade.HasValue)
                .OrderBy(s => s.DataInicio)
                .Select(s => (double)s.Produtividade!.Value)
                .ToList();

            if (produtividades.Count < 2)
                return "Estável";

            var primeira = produtividades.First();
            var ultima = produtividades.Last();

            if (ultima > primeira + 5)
                return "Aumentando";
            else if (ultima < primeira - 5)
                return "Diminuindo";
            else
                return "Estável";
        }

        private string CalcularTendenciaHumor(List<nexus.Models.Humor> humores)
        {
            if (humores.Count < 2)
                return "Estável";

            var humoresOrdenados = humores.OrderBy(h => h.DataRegistro).ToList();
            var primeiro = humoresOrdenados.First().NivelHumor;
            var ultimo = humoresOrdenados.Last().NivelHumor;

            if (ultimo > primeiro)
                return "Aumentando";
            else if (ultimo < primeiro)
                return "Diminuindo";
            else
                return "Estável";
        }

        private string AnalisarPadroesProdutividade(List<nexus.Models.Sprint> sprints, List<nexus.Models.Humor> humores)
        {
            var produtividades = sprints.Where(s => s.Produtividade.HasValue).Select(s => (double)s.Produtividade!.Value).ToList();
            var mediaProdutividade = produtividades.Any() ? produtividades.Average() : 0.0;
            var mediaHumor = humores.Any() ? humores.Average(h => h.NivelHumor) : 3;

            if (mediaProdutividade > 85 && mediaHumor >= 4)
                return "Alta produtividade com bom bem-estar. Padrão saudável mantido.";
            else if (mediaProdutividade > 85 && mediaHumor < 3)
                return "Alta produtividade, mas bem-estar comprometido. Risco de burnout.";
            else if (mediaProdutividade < 60 && mediaHumor >= 4)
                return "Produtividade baixa, mas bem-estar preservado. Pode indicar necessidade de desafios ou ajustes.";
            else
                return "Produtividade e bem-estar em níveis moderados. Continue monitorando.";
        }

        private int CalcularScoreBemEstar(
            List<nexus.Models.Humor> humores, 
            List<nexus.Models.Sprint> sprints, 
            AnaliseSentimentoResponseDto? analiseSentimento)
        {
            int score = 50; // Base

            if (humores.Any())
            {
                var mediaHumor = humores.Average(h => h.NivelHumor);
                var mediaEnergia = humores.Average(h => h.NivelEnergia);
                score += (int)((mediaHumor + mediaEnergia) * 5); // +10 a +50
            }

            if (sprints.Any())
            {
                var produtividades = sprints.Where(s => s.Produtividade.HasValue).Select(s => (double)s.Produtividade!.Value).ToList();
                if (produtividades.Any())
                {
                    var mediaProdutividade = produtividades.Average();
                    score += (int)(mediaProdutividade * 0.2); // +0 a +20
                }
            }

            if (analiseSentimento != null)
            {
                if (analiseSentimento.Sentimento == "Positivo")
                    score += 10;
                else if (analiseSentimento.Sentimento == "Negativo")
                    score -= 15;

                score -= analiseSentimento.NivelRisco * 3; // -3 a -15
            }

            return Math.Clamp(score, 0, 100);
        }

        private List<string> GerarRecomendacoesGerais(
            AnaliseSentimentoResponseDto? analiseSentimento,
            AnaliseProdutividadeDto? analiseProdutividade,
            int scoreBemEstar)
        {
            var recomendacoes = new List<string>();

            if (scoreBemEstar >= 80)
            {
                recomendacoes.Add("🌟 Excelente! Você está mantendo um ótimo equilíbrio entre trabalho e bem-estar.");
                recomendacoes.Add("📝 Continue registrando seus dados para manter esse padrão saudável.");
            }
            else if (scoreBemEstar >= 60)
            {
                recomendacoes.Add("👍 Você está em um bom caminho. Continue monitorando seu bem-estar.");
                recomendacoes.Add("💪 Mantenha hábitos saudáveis e pausas regulares.");
            }
            else if (scoreBemEstar >= 40)
            {
                recomendacoes.Add("⚠️ Seu bem-estar precisa de atenção. Considere fazer ajustes na rotina.");
                recomendacoes.Add("🧘 Pratique técnicas de relaxamento e gerencie melhor o estresse.");
            }
            else
            {
                recomendacoes.Add("🚨 Seu bem-estar está comprometido. É importante buscar apoio e fazer mudanças.");
                recomendacoes.Add("💬 Converse com seu gestor ou equipe de RH sobre seu bem-estar.");
            }

            if (analiseSentimento?.Recomendacoes != null && analiseSentimento.Recomendacoes.Any())
            {
                recomendacoes.AddRange(analiseSentimento.Recomendacoes.Take(3));
            }

            if (analiseProdutividade != null && analiseProdutividade.Tendencia == "Diminuindo" && analiseProdutividade.MediaProdutividade > 0)
            {
                recomendacoes.Add("📉 Produtividade em declínio detectada. Revise sua carga de trabalho e prioridades.");
            }

            return recomendacoes.Distinct().ToList();
        }

        #endregion
    }
}


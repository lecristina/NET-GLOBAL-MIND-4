using System.ComponentModel.DataAnnotations;

namespace nexus.Models.DTOs
{
    /// <summary>
    /// DTO para análise de sentimento de texto
    /// </summary>
    public class AnaliseSentimentoDto
    {
        /// <summary>
        /// Texto a ser analisado
        /// </summary>
        [Required(ErrorMessage = "Texto é obrigatório")]
        [StringLength(1000, ErrorMessage = "Texto deve ter no máximo 1000 caracteres")]
        public string Texto { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para resposta de análise de sentimento
    /// </summary>
    public class AnaliseSentimentoResponseDto
    {
        /// <summary>
        /// Sentimento detectado (Positivo, Negativo, Neutro)
        /// </summary>
        public string Sentimento { get; set; } = string.Empty;

        /// <summary>
        /// Score de confiança (0.0 a 1.0)
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Recomendações geradas pela IA
        /// </summary>
        public List<string> Recomendacoes { get; set; } = new List<string>();

        /// <summary>
        /// Nível de risco detectado (1-5)
        /// </summary>
        public int NivelRisco { get; set; }

        /// <summary>
        /// Mensagem personalizada gerada
        /// </summary>
        public string Mensagem { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para classificação de imagem
    /// </summary>
    public class ClassificacaoImagemDto
    {
        /// <summary>
        /// Imagem em base64 ou URL
        /// </summary>
        [Required(ErrorMessage = "Imagem é obrigatória")]
        public string ImagemBase64 { get; set; } = string.Empty;

        /// <summary>
        /// Descrição opcional da imagem
        /// </summary>
        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string? Descricao { get; set; }
    }

    /// <summary>
    /// DTO para resposta de classificação de imagem
    /// </summary>
    public class ClassificacaoImagemResponseDto
    {
        /// <summary>
        /// Categoria detectada (Organizado, Desorganizado, Confortável, Estressante, etc.)
        /// </summary>
        public string Categoria { get; set; } = string.Empty;

        /// <summary>
        /// Score de confiança (0.0 a 1.0)
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Análise de bem-estar do ambiente
        /// </summary>
        public string AnaliseBemEstar { get; set; } = string.Empty;

        /// <summary>
        /// Recomendações para melhorar o ambiente
        /// </summary>
        public List<string> Recomendacoes { get; set; } = new List<string>();

        /// <summary>
        /// Nível de bem-estar do ambiente (1-5)
        /// </summary>
        public int NivelBemEstar { get; set; }
    }

    /// <summary>
    /// DTO para análise completa de bem-estar (combina dados de humor, sprints e IA)
    /// </summary>
    public class AnaliseBemEstarCompletaDto
    {
        /// <summary>
        /// ID do usuário
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Análise de sentimento dos comentários de humor
        /// </summary>
        public AnaliseSentimentoResponseDto? AnaliseSentimento { get; set; }

        /// <summary>
        /// Análise de produtividade das sprints
        /// </summary>
        public AnaliseProdutividadeDto? AnaliseProdutividade { get; set; }

        /// <summary>
        /// Alertas gerados pela IA
        /// </summary>
        public List<AlertaIAGeradoDto> Alertas { get; set; } = new List<AlertaIAGeradoDto>();

        /// <summary>
        /// Score geral de bem-estar (0-100)
        /// </summary>
        public int ScoreBemEstar { get; set; }

        /// <summary>
        /// Recomendações gerais
        /// </summary>
        public List<string> RecomendacoesGerais { get; set; } = new List<string>();

        /// <summary>
        /// Data da análise
        /// </summary>
        public DateTime DataAnalise { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// DTO para análise de produtividade
    /// </summary>
    public class AnaliseProdutividadeDto
    {
        /// <summary>
        /// Média de produtividade
        /// </summary>
        public double MediaProdutividade { get; set; }

        /// <summary>
        /// Tendência (Aumentando, Diminuindo, Estável)
        /// </summary>
        public string Tendencia { get; set; } = string.Empty;

        /// <summary>
        /// Análise de padrões
        /// </summary>
        public string AnalisePadroes { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para alerta gerado pela IA
    /// </summary>
    public class AlertaIAGeradoDto
    {
        /// <summary>
        /// Tipo de alerta
        /// </summary>
        public string TipoAlerta { get; set; } = string.Empty;

        /// <summary>
        /// Mensagem do alerta
        /// </summary>
        public string Mensagem { get; set; } = string.Empty;

        /// <summary>
        /// Nível de risco (1-5)
        /// </summary>
        public int NivelRisco { get; set; }

        /// <summary>
        /// Prioridade (Baixa, Média, Alta)
        /// </summary>
        public string Prioridade { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para exemplo de treinamento customizado
    /// </summary>
    public class ExemploTreinamentoDto
    {
        /// <summary>
        /// Texto para treinamento
        /// </summary>
        [Required(ErrorMessage = "Texto é obrigatório")]
        [StringLength(1000, ErrorMessage = "Texto deve ter no máximo 1000 caracteres")]
        public string Texto { get; set; } = string.Empty;

        /// <summary>
        /// Label do sentimento (true = Positivo, false = Negativo)
        /// </summary>
        [Required(ErrorMessage = "Label é obrigatório")]
        public bool Label { get; set; }
    }

    /// <summary>
    /// DTO para adicionar múltiplos exemplos de treinamento
    /// </summary>
    public class AdicionarExemplosTreinamentoDto
    {
        /// <summary>
        /// Lista de exemplos de treinamento
        /// </summary>
        [Required(ErrorMessage = "Exemplos são obrigatórios")]
        [MinLength(1, ErrorMessage = "Pelo menos um exemplo deve ser fornecido")]
        public List<ExemploTreinamentoDto> Exemplos { get; set; } = new List<ExemploTreinamentoDto>();
    }

    /// <summary>
    /// DTO para resposta de treinamento
    /// </summary>
    public class TreinamentoResponseDto
    {
        /// <summary>
        /// Indica se o treinamento foi bem-sucedido
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Mensagem de resultado
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Tamanho do dataset usado
        /// </summary>
        public int DatasetSize { get; set; }

        /// <summary>
        /// Métricas do modelo (se disponível)
        /// </summary>
        public ModelMetricsDto? Metrics { get; set; }

        /// <summary>
        /// Caminho do modelo salvo
        /// </summary>
        public string? ModelPath { get; set; }

        /// <summary>
        /// Caminho do dataset salvo
        /// </summary>
        public string? DatasetPath { get; set; }
    }

    /// <summary>
    /// DTO para métricas do modelo
    /// </summary>
    public class ModelMetricsDto
    {
        /// <summary>
        /// Acurácia do modelo (0.0 a 1.0)
        /// </summary>
        public double Accuracy { get; set; }

        /// <summary>
        /// AUC (Area Under Curve)
        /// </summary>
        public double Auc { get; set; }

        /// <summary>
        /// F1 Score
        /// </summary>
        public double F1Score { get; set; }
    }
}


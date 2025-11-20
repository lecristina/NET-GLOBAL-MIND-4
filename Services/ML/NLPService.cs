using System.Text;
using System.Text.RegularExpressions;

namespace nexus.Services.ML
{
    /// <summary>
    /// Serviço de processamento de linguagem natural (NLP)
    /// Implementa técnicas de pré-processamento de texto em português
    /// </summary>
    public class NLPService
    {
        private readonly ILogger<NLPService> _logger;
        
        // Stop words em português (palavras comuns que não agregam significado)
        private readonly HashSet<string> _stopWords = new HashSet<string>
        {
            "a", "o", "e", "de", "do", "da", "em", "um", "uma", "para", "com", "não", "que",
            "é", "se", "na", "por", "mais", "as", "os", "me", "meu", "minha", "te", "seu", "sua",
            "nos", "nossos", "vocês", "eles", "elas", "isso", "isto", "aquilo", "onde", "quando",
            "como", "porque", "mas", "ou", "então", "também", "já", "ainda", "só", "até", "sobre"
        };

        // Sufixos comuns em português para stemming básico
        private readonly Dictionary<string, string> _sufixos = new Dictionary<string, string>
        {
            { "ção", "" }, { "ções", "" },
            { "mente", "" },
            { "ado", "" }, { "ada", "" }, { "ados", "" }, { "adas", "" },
            { "ido", "" }, { "ida", "" }, { "idos", "" }, { "idas", "" },
            { "ando", "" },
            { "endo", "" }, { "indo", "" },
            { "s", "" }, { "es", "" }
        };

        public NLPService(ILogger<NLPService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Tokeniza um texto (divide em palavras)
        /// </summary>
        public List<string> Tokenizar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return new List<string>();

            // Remover pontuação e converter para minúsculas
            var textoLimpo = Regex.Replace(texto.ToLowerInvariant(), @"[^\w\s]", " ");
            
            // Dividir em palavras
            var tokens = textoLimpo
                .Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(t => t.Length > 1) // Remover palavras de 1 caractere
                .ToList();

            return tokens;
        }

        /// <summary>
        /// Remove stop words de uma lista de tokens
        /// </summary>
        public List<string> RemoverStopWords(List<string> tokens)
        {
            return tokens
                .Where(token => !_stopWords.Contains(token.ToLowerInvariant()))
                .ToList();
        }

        /// <summary>
        /// Aplica stemming básico (reduz palavras à raiz)
        /// </summary>
        public string AplicarStemming(string palavra)
        {
            var palavraLower = palavra.ToLowerInvariant();

            foreach (var sufixo in _sufixos.Keys.OrderByDescending(s => s.Length))
            {
                if (palavraLower.EndsWith(sufixo))
                {
                    return palavraLower.Substring(0, palavraLower.Length - sufixo.Length);
                }
            }

            return palavraLower;
        }

        /// <summary>
        /// Processa texto completo: tokenização, remoção de stop words e stemming
        /// </summary>
        public string ProcessarTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            var tokens = Tokenizar(texto);
            var tokensSemStopWords = RemoverStopWords(tokens);
            var tokensStemmed = tokensSemStopWords.Select(AplicarStemming).ToList();

            return string.Join(" ", tokensStemmed);
        }

        /// <summary>
        /// Extrai características do texto (frequência de palavras, comprimento, etc.)
        /// </summary>
        public Dictionary<string, object> ExtrairCaracteristicas(string texto)
        {
            var tokens = Tokenizar(texto);
            var tokensSemStopWords = RemoverStopWords(tokens);

            var caracteristicas = new Dictionary<string, object>
            {
                { "ComprimentoTexto", texto.Length },
                { "NumeroPalavras", tokens.Count },
                { "NumeroPalavrasSignificativas", tokensSemStopWords.Count },
                { "PalavrasUnicas", tokensSemStopWords.Distinct().Count() },
                { "MediaComprimentoPalavras", tokens.Any() ? tokens.Average(t => t.Length) : 0.0 }
            };

            // Frequência de palavras
            var frequencia = tokensSemStopWords
                .GroupBy(t => t.ToLowerInvariant())
                .ToDictionary(g => g.Key, g => g.Count());

            caracteristicas["FrequenciaPalavras"] = frequencia;

            return caracteristicas;
        }

        /// <summary>
        /// Detecta sentimento básico usando análise de palavras-chave melhorada
        /// </summary>
        public (string Sentimento, double Score) DetectarSentimentoBasico(string texto)
        {
            var textoProcessado = ProcessarTexto(texto);
            var tokens = Tokenizar(textoProcessado);

            // Palavras-chave positivas (com stemming aplicado)
            var palavrasPositivas = new[] { 
                "bom", "otim", "excelent", "feliz", "satisfeit", "energiz", 
                "motiv", "produtiv", "bem", "melhor", "perfeit", "realiz",
                "anim", "content", "maravilh", "satisfeit"
            };

            // Palavras-chave negativas (com stemming aplicado)
            var palavrasNegativas = new[] { 
                "ruim", "pessim", "cans", "estress", "sobrecarg", 
                "exaust", "frustr", "ansios", "preocup", "mal", 
                "dificil", "problem", "burnout", "esgot", "desanim", 
                "deprim", "terrivel", "pessim"
            };

            var countPositivo = tokens.Count(t => palavrasPositivas.Any(p => t.Contains(p)));
            var countNegativo = tokens.Count(t => palavrasNegativas.Any(p => t.Contains(p)));

            // Calcular score
            var total = countPositivo + countNegativo;
            double score = 0.5; // Neutro

            if (total > 0)
            {
                score = countPositivo / (double)total;
            }

            // Determinar sentimento
            string sentimento;
            if (countPositivo > countNegativo && countPositivo > 0)
                sentimento = "Positivo";
            else if (countNegativo > countPositivo && countNegativo > 0)
                sentimento = "Negativo";
            else
                sentimento = "Neutro";

            return (sentimento, score);
        }

        /// <summary>
        /// Normaliza texto (remove acentos, converte para minúsculas, etc.)
        /// </summary>
        public string NormalizarTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            // Converter para minúsculas
            var normalizado = texto.ToLowerInvariant();

            // Remover acentos básicos (simplificado)
            normalizado = normalizado
                .Replace("á", "a").Replace("à", "a").Replace("ã", "a").Replace("â", "a")
                .Replace("é", "e").Replace("ê", "e")
                .Replace("í", "i")
                .Replace("ó", "o").Replace("ô", "o").Replace("õ", "o")
                .Replace("ú", "u").Replace("ü", "u")
                .Replace("ç", "c");

            // Remover caracteres especiais
            normalizado = Regex.Replace(normalizado, @"[^\w\s]", " ");

            // Remover espaços múltiplos
            normalizado = Regex.Replace(normalizado, @"\s+", " ").Trim();

            return normalizado;
        }
    }
}


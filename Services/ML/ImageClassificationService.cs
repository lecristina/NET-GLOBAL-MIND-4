using nexus.Models.DTOs;

namespace nexus.Services.ML
{
    /// <summary>
    /// Serviço de classificação de imagens usando ML.NET
    /// Implementa Visão Computacional para análise de ambiente de trabalho
    /// </summary>
    public class ImageClassificationService : IImageClassificationService
    {
        private readonly ILogger<ImageClassificationService> _logger;
        private readonly Dictionary<string, string[]> _categoriasAmbiente;

        public ImageClassificationService(ILogger<ImageClassificationService> logger)
        {
            _logger = logger;
            
            // Categorias de ambiente de trabalho para classificação
            _categoriasAmbiente = new Dictionary<string, string[]>
            {
                { "Organizado", new[] { "organizado", "limpo", "arrumado", "ordenado", "estruturado" } },
                { "Desorganizado", new[] { "desorganizado", "bagunçado", "desordenado", "caótico" } },
                { "Confortável", new[] { "confortável", "acolhedor", "agradável", "relaxante", "confort" } },
                { "Estressante", new[] { "estressante", "tenso", "pressão", "sobrecarga", "estresse" } },
                { "Ergonômico", new[] { "ergonômico", "adequado", "bem configurado", "postura" } },
                { "Inadequado", new[] { "inadequado", "improvisado", "precário", "inconfortável" } }
            };
        }

        /// <summary>
        /// Classifica uma imagem e analisa o ambiente de trabalho
        /// </summary>
        public async Task<ClassificacaoImagemResponseDto> ClassificarImagemAsync(string imagemBase64, string? descricao = null)
        {
            if (!ValidarImagem(imagemBase64))
            {
                throw new ArgumentException("Imagem inválida ou formato não suportado");
            }

            try
            {
                // Em produção, aqui usaria um modelo ML.NET treinado
                // Por enquanto, usamos análise baseada em descrição e heurísticas
                var categoria = ClassificarImagemBasica(imagemBase64, descricao);
                var score = CalcularScoreClassificacao(categoria, descricao);
                var nivelBemEstar = CalcularNivelBemEstar(categoria);
                var analiseBemEstar = GerarAnaliseBemEstar(categoria, nivelBemEstar);
                var recomendacoes = GerarRecomendacoesAmbiente(categoria, nivelBemEstar);

                _logger.LogInformation("Classificação de imagem concluída: {Categoria}, Score: {Score}, Bem-estar: {BemEstar}", 
                    categoria, score, nivelBemEstar);

                return await Task.FromResult(new ClassificacaoImagemResponseDto
                {
                    Categoria = categoria,
                    Score = score,
                    NivelBemEstar = nivelBemEstar,
                    AnaliseBemEstar = analiseBemEstar,
                    Recomendacoes = recomendacoes
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao classificar imagem");
                throw;
            }
        }

        /// <summary>
        /// Valida se a imagem é válida
        /// </summary>
        public bool ValidarImagem(string imagemBase64)
        {
            if (string.IsNullOrWhiteSpace(imagemBase64))
                return false;

            try
            {
                // Remover prefixo data:image se existir
                var base64Data = imagemBase64.Contains(",") 
                    ? imagemBase64.Split(',')[1] 
                    : imagemBase64;

                var imageBytes = Convert.FromBase64String(base64Data);
                
                if (imageBytes.Length == 0 || imageBytes.Length > 10 * 1024 * 1024) // Max 10MB
                    return false;

                // Validar formato básico (verificar magic bytes)
                // JPEG: FF D8 FF
                // PNG: 89 50 4E 47
                // GIF: 47 49 46 38
                if (imageBytes.Length < 4)
                    return false;

                var header = BitConverter.ToUInt32(imageBytes, 0);
                var isValidFormat = 
                    (imageBytes[0] == 0xFF && imageBytes[1] == 0xD8 && imageBytes[2] == 0xFF) || // JPEG
                    (imageBytes[0] == 0x89 && imageBytes[1] == 0x50 && imageBytes[2] == 0x4E && imageBytes[3] == 0x47) || // PNG
                    (imageBytes[0] == 0x47 && imageBytes[1] == 0x49 && imageBytes[2] == 0x46 && imageBytes[3] == 0x38); // GIF
                
                return isValidFormat;
            }
            catch
            {
                return false;
            }
        }

        #region Métodos Privados

        private string ClassificarImagemBasica(string imagemBase64, string? descricao)
        {
            // Priorizar análise da descrição (mais confiável que heurísticas de tamanho)
            if (!string.IsNullOrWhiteSpace(descricao))
            {
                var descLower = descricao.ToLowerInvariant();
                
                // Palavras-chave negativas (desorganização, problemas)
                var palavrasNegativas = new[] { 
                    "desorganizado", "desorganizada", "desorganização", "bagunçado", "bagunçada", 
                    "bagunça", "desordenado", "desordenada", "caótico", "caótica", "caos",
                    "sujo", "suja", "sujeira", "confuso", "confusa", "confusão",
                    "estressante", "estresse", "tenso", "tensa", "sobrecarga", "pressão",
                    "inadequado", "inadequada", "improvisado", "improvisada", "precário", "precária",
                    "pouco desorganizada", "pouco desorganizado", "um pouco desorganizada", "um pouco desorganizado"
                };
                
                // Palavras-chave positivas (organização, conforto)
                var palavrasPositivas = new[] { 
                    "organizado", "organizada", "organização", "limpo", "limpa", "limpeza",
                    "arrumado", "arrumada", "ordenado", "ordenada", "estruturado", "estruturada",
                    "confortável", "confort", "acolhedor", "acolhedora", "agradável", "relaxante",
                    "ergonômico", "ergonômica", "adequado", "adequada", "bem configurado", "bem configurada"
                };
                
                // Contar ocorrências de palavras negativas e positivas
                var countNegativas = palavrasNegativas.Count(palavra => descLower.Contains(palavra));
                var countPositivas = palavrasPositivas.Count(palavra => descLower.Contains(palavra));
                
                // Se houver palavras negativas, priorizar categorias negativas
                if (countNegativas > 0)
                {
                    // Verificar qual categoria negativa melhor se encaixa
                    if (palavrasNegativas.Any(p => descLower.Contains("desorganizado") || descLower.Contains("desorganizada") || 
                                                   descLower.Contains("bagunçado") || descLower.Contains("bagunçada") ||
                                                   descLower.Contains("desordenado") || descLower.Contains("desordenada") ||
                                                   descLower.Contains("caótico") || descLower.Contains("caótica")))
                    {
                        return "Desorganizado";
                    }
                    else if (palavrasNegativas.Any(p => descLower.Contains("estressante") || descLower.Contains("estresse") ||
                                                       descLower.Contains("tenso") || descLower.Contains("pressão")))
                    {
                        return "Estressante";
                    }
                    else if (palavrasNegativas.Any(p => descLower.Contains("inadequado") || descLower.Contains("inadequada") ||
                                                       descLower.Contains("improvisado") || descLower.Contains("precário")))
                    {
                        return "Inadequado";
                    }
                }
                
                // Se houver palavras positivas e nenhuma negativa, usar categorias positivas
                if (countPositivas > 0 && countNegativas == 0)
                {
                    // Verificar qual categoria positiva melhor se encaixa
                    foreach (var categoria in _categoriasAmbiente)
                    {
                        if (categoria.Value.Any(palavra => descLower.Contains(palavra)))
                        {
                            return categoria.Key;
                        }
                    }
                }
                
                // Se houver mais palavras negativas que positivas, classificar como desorganizado
                if (countNegativas > countPositivas)
                {
                    return "Desorganizado";
                }
            }

            // Análise básica da imagem (em produção, usaria modelo ML.NET treinado)
            // Por enquanto, retorna baseado em heurísticas simples
            try
            {
                var base64Data = imagemBase64.Contains(",") 
                    ? imagemBase64.Split(',')[1] 
                    : imagemBase64;
                var imageBytes = Convert.FromBase64String(base64Data);
                
                // Heurísticas básicas baseadas no tamanho do arquivo
                // Em produção, isso seria feito por um modelo de ML treinado com ML.NET Image Classification
                var tamanhoArquivo = imageBytes.Length;
                
                // Classificação baseada em características básicas
                // Sem descrição, usar heurísticas mais conservadoras
                if (tamanhoArquivo > 500000) // > 500KB
                {
                    return "Organizado";
                }
                else if (tamanhoArquivo > 200000) // > 200KB
                {
                    return "Confortável";
                }
                else
                {
                    // Default mais neutro quando não há informação suficiente
                    return "Confortável";
                }
            }
            catch
            {
                // Se não conseguir analisar, retornar baseado na descrição ou default neutro
                if (!string.IsNullOrWhiteSpace(descricao))
                {
                    var descLower = descricao.ToLowerInvariant();
                    if (descLower.Contains("desorganizado") || descLower.Contains("desorganizada") ||
                        descLower.Contains("bagunçado") || descLower.Contains("bagunçada"))
                    {
                        return "Desorganizado";
                    }
                    if (descLower.Contains("organizado") || descLower.Contains("organizada"))
                    {
                        return "Organizado";
                    }
                }
                return "Confortável";
            }
        }

        private double CalcularScoreClassificacao(string categoria, string? descricao)
        {
            double score = 0.7; // Score padrão

            // Ajustar score baseado na categoria e descrição
            switch (categoria)
            {
                case "Organizado":
                case "Confortável":
                case "Ergonômico":
                    score = 0.75 + (descricao?.Length > 50 ? 0.1 : 0.0);
                    // Se a descrição confirma a categoria positiva, aumentar score
                    if (!string.IsNullOrWhiteSpace(descricao))
                    {
                        var descLower = descricao.ToLowerInvariant();
                        if ((categoria == "Organizado" && (descLower.Contains("organizado") || descLower.Contains("limpo"))) ||
                            (categoria == "Confortável" && (descLower.Contains("confortável") || descLower.Contains("agradável"))) ||
                            (categoria == "Ergonômico" && descLower.Contains("ergonômico")))
                        {
                            score += 0.1;
                        }
                    }
                    break;
                case "Desorganizado":
                    score = 0.4; // Score mais baixo para desorganizado
                    // Se a descrição confirma desorganização, aumentar confiança (score mais baixo = mais certeza de desorganização)
                    if (!string.IsNullOrWhiteSpace(descricao))
                    {
                        var descLower = descricao.ToLowerInvariant();
                        if (descLower.Contains("desorganizado") || descLower.Contains("desorganizada") ||
                            descLower.Contains("bagunçado") || descLower.Contains("bagunçada") ||
                            descLower.Contains("pouco desorganizada") || descLower.Contains("pouco desorganizado"))
                        {
                            score = 0.35; // Score mais baixo = maior confiança na classificação negativa
                        }
                    }
                    break;
                case "Estressante":
                    score = 0.3;
                    break;
                case "Inadequado":
                    score = 0.25;
                    break;
            }

            return Math.Clamp(score, 0.0, 1.0);
        }

        private int CalcularNivelBemEstar(string categoria)
        {
            return categoria switch
            {
                "Organizado" or "Confortável" or "Ergonômico" => 5,
                "Inadequado" => 2,
                "Desorganizado" => 3,
                "Estressante" => 1,
                _ => 3
            };
        }

        private string GerarAnaliseBemEstar(string categoria, int nivelBemEstar)
        {
            return categoria switch
            {
                "Organizado" => "Seu ambiente de trabalho está bem organizado, o que contribui positivamente para sua produtividade e bem-estar. 👍",
                "Confortável" => "O ambiente parece confortável e adequado para o trabalho. Isso é ótimo para manter seu bem-estar! 😊",
                "Ergonômico" => "Excelente! Seu ambiente está configurado de forma ergonômica, o que ajuda a prevenir problemas de saúde. 🎯",
                "Desorganizado" => "O ambiente parece um pouco desorganizado. Organizar o espaço pode melhorar sua produtividade e reduzir o estresse. 📋",
                "Estressante" => "O ambiente parece estar causando estresse. Considere fazer ajustes para torná-lo mais agradável e produtivo. ⚠️",
                "Inadequado" => "O ambiente de trabalho pode estar inadequado. Recomendamos melhorias para garantir seu bem-estar e produtividade. 🔧",
                _ => "Análise do ambiente de trabalho concluída. Continue monitorando para manter um espaço saudável. 📊"
            };
        }

        private List<string> GerarRecomendacoesAmbiente(string categoria, int nivelBemEstar)
        {
            var recomendacoes = new List<string>();

            switch (categoria)
            {
                case "Organizado":
                    recomendacoes.Add("✅ Continue mantendo a organização do seu espaço!");
                    recomendacoes.Add("🔄 Revise periodicamente para manter a ordem.");
                    break;

                case "Confortável":
                    recomendacoes.Add("😊 Ótimo ambiente! Continue mantendo o conforto.");
                    recomendacoes.Add("💡 Considere adicionar plantas para melhorar ainda mais o ambiente.");
                    break;

                case "Ergonômico":
                    recomendacoes.Add("🎯 Excelente configuração ergonômica!");
                    recomendacoes.Add("⏰ Lembre-se de fazer pausas regulares mesmo com boa ergonomia.");
                    break;

                case "Desorganizado":
                    recomendacoes.Add("📋 Organize seu espaço de trabalho para melhorar a produtividade.");
                    recomendacoes.Add("🗂️ Use organizadores e mantenha apenas o essencial à vista.");
                    recomendacoes.Add("🧹 Reserve 10 minutos diários para organização.");
                    break;

                case "Estressante":
                    recomendacoes.Add("⚠️ Considere reorganizar o ambiente para reduzir o estresse.");
                    recomendacoes.Add("🌿 Adicione elementos que tragam calma (plantas, iluminação adequada).");
                    recomendacoes.Add("🎵 Use música ambiente suave se possível.");
                    recomendacoes.Add("💬 Converse com seu gestor sobre melhorias no ambiente.");
                    break;

                case "Inadequado":
                    recomendacoes.Add("🔧 Melhore a configuração do seu ambiente de trabalho.");
                    recomendacoes.Add("🪑 Verifique se sua cadeira e mesa estão adequadas.");
                    recomendacoes.Add("💡 Ajuste a iluminação para reduzir cansaço visual.");
                    recomendacoes.Add("🌡️ Mantenha temperatura e ventilação adequadas.");
                    break;
            }

            // Recomendações gerais baseadas no nível de bem-estar
            if (nivelBemEstar <= 2)
            {
                recomendacoes.Add("🚨 Ambiente com baixo nível de bem-estar detectado. Ações imediatas recomendadas.");
            }

            return recomendacoes.Distinct().ToList();
        }

        #endregion
    }
}


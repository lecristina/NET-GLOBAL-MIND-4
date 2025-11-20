using Microsoft.ML;
using Microsoft.ML.Data;
using nexus.Services.ML.Models;
using System.Text;

namespace nexus.Services.ML
{
    /// <summary>
    /// Serviço para treinar modelo de análise de sentimento usando ML.NET
    /// </summary>
    public class MLModelTrainer
    {
        private readonly MLContext _mlContext;
        private readonly ILogger<MLModelTrainer> _logger;

        public MLModelTrainer(ILogger<MLModelTrainer> logger)
        {
            _mlContext = new MLContext(seed: 0);
            _logger = logger;
        }

        /// <summary>
        /// Treina um modelo de análise de sentimento com dados fornecidos
        /// </summary>
        public ITransformer TreinarModelo(List<SentimentInput> dadosTreinamento, string caminhoModelo = "sentiment_model.zip")
        {
            try
            {
                _logger.LogInformation("Iniciando treinamento do modelo com {Count} exemplos", dadosTreinamento.Count);

                // Carregar dados
                var dataView = _mlContext.Data.LoadFromEnumerable(dadosTreinamento);

                // Dividir em treino e teste (80/20)
                var split = _mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

                // Pipeline de transformação e treinamento
                var pipeline = _mlContext.Transforms.Text
                    .FeaturizeText(
                        outputColumnName: "Features",
                        inputColumnName: nameof(SentimentInput.Text))
                    .Append(_mlContext.BinaryClassification.Trainers
                        .SdcaLogisticRegression(
                            labelColumnName: nameof(SentimentInput.Label),
                            featureColumnName: "Features"));

                // Treinar modelo
                _logger.LogInformation("Treinando modelo...");
                var model = pipeline.Fit(split.TrainSet);

                // Avaliar modelo
                var predictions = model.Transform(split.TestSet);
                var metrics = _mlContext.BinaryClassification.Evaluate(predictions, labelColumnName: nameof(SentimentInput.Label));

                _logger.LogInformation("Acurácia do modelo: {Accuracy:P2}", metrics.Accuracy);
                _logger.LogInformation("AUC: {Auc:P2}", metrics.AreaUnderRocCurve);
                _logger.LogInformation("F1 Score: {F1Score:P2}", metrics.F1Score);

                // Salvar modelo
                var modelPath = Path.Combine(AppContext.BaseDirectory, caminhoModelo);
                _mlContext.Model.Save(model, split.TrainSet.Schema, modelPath);
                _logger.LogInformation("Modelo salvo em: {Path}", modelPath);

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao treinar modelo");
                throw;
            }
        }

        /// <summary>
        /// Treina modelo e retorna métricas
        /// </summary>
        public (ITransformer Model, ModelMetrics Metrics) TreinarModeloComMetricas(List<SentimentInput> dadosTreinamento, string caminhoModelo = "sentiment_model.zip")
        {
            try
            {
                _logger.LogInformation("Iniciando treinamento do modelo com {Count} exemplos", dadosTreinamento.Count);

                if (dadosTreinamento.Count < 10)
                {
                    throw new InvalidOperationException("É necessário pelo menos 10 exemplos para treinar o modelo");
                }

                // Carregar dados
                var dataView = _mlContext.Data.LoadFromEnumerable(dadosTreinamento);

                // Dividir em treino e teste (80/20)
                var split = _mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

                // Pipeline de transformação e treinamento
                var pipeline = _mlContext.Transforms.Text
                    .FeaturizeText(
                        outputColumnName: "Features",
                        inputColumnName: nameof(SentimentInput.Text))
                    .Append(_mlContext.BinaryClassification.Trainers
                        .SdcaLogisticRegression(
                            labelColumnName: nameof(SentimentInput.Label),
                            featureColumnName: "Features"));

                // Treinar modelo
                _logger.LogInformation("Treinando modelo...");
                var model = pipeline.Fit(split.TrainSet);

                // Avaliar modelo
                var predictions = model.Transform(split.TestSet);
                var metrics = _mlContext.BinaryClassification.Evaluate(predictions, labelColumnName: nameof(SentimentInput.Label));

                _logger.LogInformation("Acurácia do modelo: {Accuracy:P2}", metrics.Accuracy);
                _logger.LogInformation("AUC: {Auc:P2}", metrics.AreaUnderRocCurve);
                _logger.LogInformation("F1 Score: {F1Score:P2}", metrics.F1Score);

                // Salvar modelo
                var modelPath = Path.Combine(AppContext.BaseDirectory, caminhoModelo);
                _mlContext.Model.Save(model, split.TrainSet.Schema, modelPath);
                _logger.LogInformation("Modelo salvo em: {Path}", modelPath);

                var modelMetrics = new ModelMetrics
                {
                    Accuracy = metrics.Accuracy,
                    Auc = metrics.AreaUnderRocCurve,
                    F1Score = metrics.F1Score
                };

                return (model, modelMetrics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao treinar modelo");
                throw;
            }
        }

        /// <summary>
        /// Classe para métricas do modelo
        /// </summary>
        public class ModelMetrics
        {
            public double Accuracy { get; set; }
            public double Auc { get; set; }
            public double F1Score { get; set; }
        }

        /// <summary>
        /// Carrega um modelo treinado de um arquivo
        /// </summary>
        public ITransformer? CarregarModelo(string caminhoModelo = "sentiment_model.zip")
        {
            try
            {
                var modelPath = Path.Combine(AppContext.BaseDirectory, caminhoModelo);
                
                if (!File.Exists(modelPath))
                {
                    _logger.LogWarning("Modelo não encontrado em: {Path}", modelPath);
                    return null;
                }

                var model = _mlContext.Model.Load(modelPath, out var schema);
                _logger.LogInformation("Modelo carregado com sucesso de: {Path}", modelPath);
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar modelo");
                return null;
            }
        }

        /// <summary>
        /// Gera dataset de exemplo em português para treinamento
        /// </summary>
        public List<SentimentInput> GerarDatasetExemplo()
        {
            return new List<SentimentInput>
            {
                // Textos Positivos (Label = true)
                new SentimentInput { Text = "Estou me sentindo muito bem hoje, produtivo e energizado!", Label = true },
                new SentimentInput { Text = "Excelente dia de trabalho, consegui finalizar todas as tarefas com sucesso.", Label = true },
                new SentimentInput { Text = "Me sinto satisfeito e motivado com o progresso do projeto.", Label = true },
                new SentimentInput { Text = "Ótimo ambiente de trabalho, me sinto confortável e feliz.", Label = true },
                new SentimentInput { Text = "Perfeito! Estou muito contente com os resultados alcançados.", Label = true },
                new SentimentInput { Text = "Dia maravilhoso, me sinto realizado e animado.", Label = true },
                new SentimentInput { Text = "Estou muito bem, energizado e pronto para novos desafios.", Label = true },
                new SentimentInput { Text = "Satisfação total com o trabalho realizado hoje.", Label = true },
                new SentimentInput { Text = "Me sinto ótimo, produtivo e com muita energia positiva.", Label = true },
                new SentimentInput { Text = "Excelente progresso, estou muito satisfeito e motivado.", Label = true },

                // Textos Negativos (Label = false)
                new SentimentInput { Text = "Estou muito cansado e sobrecarregado com muitas tarefas.", Label = false },
                new SentimentInput { Text = "Me sinto estressado e esgotado, não consigo descansar direito.", Label = false },
                new SentimentInput { Text = "Péssimo dia, muito frustrado e desanimado com o trabalho.", Label = false },
                new SentimentInput { Text = "Estou exausto, sobrecarregado e sem energia para continuar.", Label = false },
                new SentimentInput { Text = "Muito ansioso e preocupado com os prazos e responsabilidades.", Label = false },
                new SentimentInput { Text = "Me sinto deprimido e sem motivação para trabalhar.", Label = false },
                new SentimentInput { Text = "Burnout total, não aguento mais essa pressão constante.", Label = false },
                new SentimentInput { Text = "Estou esgotado, muito estressado e sem conseguir me concentrar.", Label = false },
                new SentimentInput { Text = "Dia terrível, me sinto mal e sem energia alguma.", Label = false },
                new SentimentInput { Text = "Muito difícil lidar com tanta sobrecarga e pressão.", Label = false },

                // Textos Neutros (Label = false, mas com menor certeza)
                new SentimentInput { Text = "Dia normal de trabalho, sem grandes eventos.", Label = false },
                new SentimentInput { Text = "Rotineiro, consegui fazer algumas tarefas básicas.", Label = false },
                new SentimentInput { Text = "Sem grandes novidades, trabalho padrão do dia a dia.", Label = false }
            };
        }

        /// <summary>
        /// Carrega dataset de um arquivo CSV
        /// </summary>
        public List<SentimentInput> CarregarDatasetDeArquivo(string caminhoArquivo)
        {
            var dataset = new List<SentimentInput>();

            if (!File.Exists(caminhoArquivo))
            {
                _logger.LogWarning("Arquivo de dataset não encontrado: {Path}", caminhoArquivo);
                return dataset;
            }

            try
            {
                var linhas = File.ReadAllLines(caminhoArquivo, Encoding.UTF8);
                
                foreach (var linha in linhas.Skip(1)) // Pular cabeçalho
                {
                    var partes = linha.Split(',');
                    if (partes.Length >= 2)
                    {
                        var texto = partes[0].Trim('"');
                        var label = bool.Parse(partes[1].Trim());
                        dataset.Add(new SentimentInput { Text = texto, Label = label });
                    }
                }

                _logger.LogInformation("Dataset carregado: {Count} exemplos", dataset.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar dataset do arquivo");
            }

            return dataset;
        }

        /// <summary>
        /// Salva dataset em arquivo CSV
        /// </summary>
        public void SalvarDatasetEmArquivo(List<SentimentInput> dataset, string caminhoArquivo = "sentiment_dataset.csv")
        {
            try
            {
                var linhas = new List<string> { "Text,Label" };
                
                foreach (var item in dataset)
                {
                    linhas.Add($"\"{item.Text}\",{item.Label}");
                }

                File.WriteAllLines(caminhoArquivo, linhas, Encoding.UTF8);
                _logger.LogInformation("Dataset salvo em: {Path} com {Count} exemplos", caminhoArquivo, dataset.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar dataset");
            }
        }

        /// <summary>
        /// Combina dataset padrão com exemplos customizados
        /// </summary>
        public List<SentimentInput> CombinarDatasets(List<SentimentInput> exemplosCustomizados, bool incluirPadrao = true)
        {
            var datasetCombinado = new List<SentimentInput>();

            // Adicionar dataset padrão se solicitado
            if (incluirPadrao)
            {
                var datasetPadrao = GerarDatasetExemplo();
                datasetCombinado.AddRange(datasetPadrao);
            }

            // Adicionar exemplos customizados
            datasetCombinado.AddRange(exemplosCustomizados);

            _logger.LogInformation("Datasets combinados: {Total} exemplos ({Padrao} padrão + {Custom} customizados)", 
                datasetCombinado.Count, 
                incluirPadrao ? GerarDatasetExemplo().Count : 0, 
                exemplosCustomizados.Count);

            return datasetCombinado;
        }

        /// <summary>
        /// Carrega exemplos customizados salvos anteriormente
        /// </summary>
        public List<SentimentInput> CarregarExemplosCustomizados(string caminhoArquivo = "custom_training_data.csv")
        {
            return CarregarDatasetDeArquivo(caminhoArquivo);
        }

        /// <summary>
        /// Salva exemplos customizados em arquivo separado
        /// </summary>
        public void SalvarExemplosCustomizados(List<SentimentInput> exemplos, string caminhoArquivo = "custom_training_data.csv")
        {
            // Carregar exemplos existentes se o arquivo existir
            var exemplosExistentes = new List<SentimentInput>();
            if (File.Exists(caminhoArquivo))
            {
                exemplosExistentes = CarregarDatasetDeArquivo(caminhoArquivo);
            }

            // Combinar com novos exemplos (evitar duplicatas)
            var todosExemplos = exemplosExistentes.ToList();
            foreach (var exemplo in exemplos)
            {
                // Verificar se já existe (mesmo texto e label)
                if (!todosExemplos.Any(e => e.Text.Equals(exemplo.Text, StringComparison.OrdinalIgnoreCase) && e.Label == exemplo.Label))
                {
                    todosExemplos.Add(exemplo);
                }
            }

            // Salvar todos os exemplos
            SalvarDatasetEmArquivo(todosExemplos, caminhoArquivo);
            _logger.LogInformation("Exemplos customizados salvos: {Count} total (adicionados {Novos} novos)", 
                todosExemplos.Count, exemplos.Count);
        }
    }
}


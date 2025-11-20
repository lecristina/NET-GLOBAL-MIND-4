# 🧪 Guia Completo de Testes - Funcionalidades de IA

Este guia mostra como testar todas as funcionalidades de IA implementadas no MindTrack API.

## 📋 Pré-requisitos

1. **Aplicação rodando**: Execute `dotnet run` na pasta do projeto
2. **Token JWT**: Você precisará de um token de autenticação para testar os endpoints
3. **Ferramenta de testes**: Escolha uma das opções abaixo

---

## 🚀 Método 1: Swagger UI (Mais Fácil)

### Passo 1: Iniciar a aplicação

```bash
dotnet run
```

A aplicação estará disponível em:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

### Passo 2: Acessar o Swagger

Abra no navegador:
- `http://localhost:5000/swagger` ou
- `https://localhost:5001/swagger`

### Passo 3: Obter Token JWT

1. **Criar usuário** (se ainda não tiver):
   - Vá em `POST /api/v1.0/Usuarios`
   - Clique em "Try it out"
   - Use este JSON:
   ```json
   {
     "nome": "Teste IA",
     "email": "teste.ia@example.com",
     "senha": "senha123",
     "perfil": "PROFISSIONAL",
     "empresa": "Tech Solutions"
   }
   ```
   - Clique em "Execute"

2. **Fazer Login**:
   - Vá em `POST /api/v1.0/Auth/login`
   - Clique em "Try it out"
   - Use este JSON:
   ```json
   {
     "email": "teste.ia@example.com",
     "senha": "senha123"
   }
   ```
   - Clique em "Execute"
   - **Copie o token** da resposta (campo `token`)

3. **Autorizar no Swagger**:
   - Clique no botão **"Authorize"** (cadeado) no topo da página
   - Cole o token no campo "Value"
   - Clique em "Authorize" e depois "Close"

### Passo 4: Testar Endpoints de IA

Agora você pode testar todos os endpoints de IA:

#### ✅ 1. Verificar Status
- `GET /api/v1.0/ML/status`
- Clique em "Try it out" → "Execute"
- Deve retornar status das funcionalidades

#### ✅ 2. Análise de Sentimento (IA Generativa)
- `POST /api/v1.0/ML/sentimento/analisar`
- Clique em "Try it out"
- Use este JSON (texto negativo):
```json
{
  "texto": "Estou me sentindo muito cansado e sobrecarregado com muitas tarefas. Não consigo descansar direito."
}
```
- Clique em "Execute"
- Veja as recomendações geradas pela IA!

**Teste com texto positivo:**
```json
{
  "texto": "Hoje me senti muito bem! Produtivo e energizado. Consegui finalizar todas as tarefas."
}
```

#### ✅ 3. Classificação de Imagem (Visão Computacional)
- `POST /api/v1.0/ML/imagem/classificar`
- Clique em "Try it out"
- Use este JSON (imagem em base64):
```json
{
  "imagemBase64": "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQEAYABgAAD...",
  "descricao": "Minha mesa de trabalho, um pouco desorganizada"
}
```

**💡 Dica**: Para converter uma imagem para base64:
- Use um conversor online: https://www.base64-image.de/
- Ou use PowerShell:
```powershell
[Convert]::ToBase64String([IO.File]::ReadAllBytes("caminho/para/imagem.jpg"))
```

#### ✅ 4. Análise Completa de Bem-estar
- `GET /api/v1.0/ML/bem-estar/analise-completa`
- Clique em "Try it out" → "Execute"
- **Nota**: Funciona melhor se você tiver criado registros de humor e sprints antes

#### ✅ 5. Gerar Alertas Inteligentes
- `GET /api/v1.0/ML/alertas/gerar`
- Clique em "Try it out" → "Execute"
- **Nota**: Funciona melhor se você tiver criado registros de humor e sprints antes

---

## 🚀 Método 2: Arquivo HTTP (REST Client)

### Passo 1: Instalar extensão REST Client

No VS Code, instale a extensão **REST Client** (humao.rest-client)

### Passo 2: Usar o arquivo de testes

1. Abra o arquivo `test-ia.http` na raiz do projeto
2. Execute os comandos na ordem:

#### 2.1 - Criar usuário e fazer login
- Execute: `POST /api/v1.0/Usuarios` (linha 15)
- Execute: `POST /api/v1.0/Auth/login` (linha 24)
- **Copie o token** da resposta
- Cole na variável `@token` na linha 10 do arquivo

#### 2.2 - Testar endpoints de IA
Agora você pode executar qualquer endpoint clicando em "Send Request" acima de cada comando.

---

## 🚀 Método 3: Postman

### Passo 1: Importar Collection

1. Abra o Postman
2. Crie uma nova Collection chamada "MindTrack IA"
3. Configure a variável `baseUrl` = `http://localhost:5000`

### Passo 2: Criar Requests

#### Request 1: Login
- **Method**: POST
- **URL**: `{{baseUrl}}/api/v1.0/Auth/login`
- **Body** (raw JSON):
```json
{
  "email": "teste.ia@example.com",
  "senha": "senha123"
}
```
- **Tests** (para salvar o token automaticamente):
```javascript
var jsonData = pm.response.json();
pm.environment.set("token", jsonData.token);
```

#### Request 2: Análise de Sentimento
- **Method**: POST
- **URL**: `{{baseUrl}}/api/v1.0/ML/sentimento/analisar`
- **Headers**: 
  - `Authorization`: `Bearer {{token}}`
  - `Content-Type`: `application/json`
- **Body** (raw JSON):
```json
{
  "texto": "Estou me sentindo muito cansado e sobrecarregado."
}
```

#### Request 3: Classificação de Imagem
- **Method**: POST
- **URL**: `{{baseUrl}}/api/v1.0/ML/imagem/classificar`
- **Headers**: 
  - `Authorization`: `Bearer {{token}}`
  - `Content-Type`: `application/json`
- **Body** (raw JSON):
```json
{
  "imagemBase64": "data:image/jpeg;base64,...",
  "descricao": "Mesa de trabalho"
}
```

---

## 🚀 Método 4: cURL (Terminal)

### Passo 1: Obter Token

```bash
curl -X POST "http://localhost:5000/api/v1.0/Auth/login" \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"teste.ia@example.com\",\"senha\":\"senha123\"}"
```

**Copie o token** da resposta.

### Passo 2: Testar Endpoints

#### Análise de Sentimento
```bash
curl -X POST "http://localhost:5000/api/v1.0/ML/sentimento/analisar" \
  -H "Authorization: Bearer SEU_TOKEN_AQUI" \
  -H "Content-Type: application/json" \
  -d "{\"texto\":\"Estou me sentindo muito cansado e sobrecarregado.\"}"
```

#### Classificação de Imagem
```bash
curl -X POST "http://localhost:5000/api/v1.0/ML/imagem/classificar" \
  -H "Authorization: Bearer SEU_TOKEN_AQUI" \
  -H "Content-Type: application/json" \
  -d "{\"imagemBase64\":\"data:image/jpeg;base64,...\",\"descricao\":\"Mesa de trabalho\"}"
```

#### Análise Completa
```bash
curl -X GET "http://localhost:5000/api/v1.0/ML/bem-estar/analise-completa" \
  -H "Authorization: Bearer SEU_TOKEN_AQUI"
```

#### Gerar Alertas
```bash
curl -X GET "http://localhost:5000/api/v1.0/ML/alertas/gerar" \
  -H "Authorization: Bearer SEU_TOKEN_AQUI"
```

---

## 📝 Exemplos de Testes

### Teste 1: Análise de Sentimento Negativo

**Request:**
```json
{
  "texto": "Estou muito estressado, sobrecarregado com tarefas, cansado e sem energia. Não consigo descansar."
}
```

**Resultado Esperado:**
- Sentimento: "Negativo"
- Score: < 0.4
- Nível de Risco: 4 ou 5
- Recomendações: Lista com sugestões de cuidado

### Teste 2: Análise de Sentimento Positivo

**Request:**
```json
{
  "texto": "Excelente dia! Me senti muito produtivo, energizado e satisfeito com o trabalho realizado."
}
```

**Resultado Esperado:**
- Sentimento: "Positivo"
- Score: > 0.6
- Nível de Risco: 1
- Recomendações: Mensagens de encorajamento

### Teste 3: Classificação de Ambiente Desorganizado

**Request:**
```json
{
  "imagemBase64": "...",
  "descricao": "Mesa de trabalho bagunçada com muitos papéis e objetos espalhados"
}
```

**Resultado Esperado:**
- Categoria: "Desorganizado"
- Nível de Bem-estar: 3 ou menos
- Recomendações: Sugestões de organização

### Teste 4: Análise Completa (requer dados)

**Pré-requisito**: Criar pelo menos:
- 2 registros de humor com comentários
- 1 sprint com produtividade

**Resultado Esperado:**
- Score de bem-estar (0-100)
- Análise de sentimento agregada
- Análise de produtividade
- Lista de alertas gerados
- Recomendações gerais

---

## 🔍 Verificando Resultados

### O que observar nas respostas:

1. **Análise de Sentimento**:
   - ✅ Campo `sentimento` (Positivo/Negativo/Neutro)
   - ✅ Campo `score` (0.0 a 1.0)
   - ✅ Campo `nivelRisco` (1 a 5)
   - ✅ Campo `recomendacoes` (lista de strings)
   - ✅ Campo `mensagem` (mensagem personalizada)

2. **Classificação de Imagem**:
   - ✅ Campo `categoria` (Organizado, Desorganizado, etc.)
   - ✅ Campo `score` (0.0 a 1.0)
   - ✅ Campo `nivelBemEstar` (1 a 5)
   - ✅ Campo `analiseBemEstar` (texto descritivo)
   - ✅ Campo `recomendacoes` (lista de sugestões)

3. **Análise Completa**:
   - ✅ Campo `scoreBemEstar` (0 a 100)
   - ✅ Campo `analiseSentimento` (objeto completo)
   - ✅ Campo `analiseProdutividade` (objeto completo)
   - ✅ Campo `alertas` (lista de alertas)
   - ✅ Campo `recomendacoesGerais` (lista de recomendações)

---

## ⚠️ Troubleshooting

### Erro 401 Unauthorized
- **Causa**: Token JWT inválido ou expirado
- **Solução**: Faça login novamente e obtenha um novo token

### Erro 400 Bad Request
- **Causa**: Dados inválidos no request
- **Solução**: Verifique o formato JSON e os campos obrigatórios

### Análise Completa retorna dados vazios
- **Causa**: Usuário não tem dados de humor ou sprints
- **Solução**: Crie alguns registros de humor e sprints primeiro

### Imagem inválida
- **Causa**: Base64 mal formatado ou imagem muito grande (>10MB)
- **Solução**: Verifique o formato base64 e o tamanho da imagem

---

## 🎯 Dicas de Teste

1. **Teste diferentes cenários**:
   - Textos muito positivos
   - Textos muito negativos
   - Textos neutros
   - Textos vazios (deve retornar erro)

2. **Teste a integração**:
   - Crie dados de humor e sprints
   - Execute análise completa
   - Veja como a IA integra todos os dados

3. **Teste os alertas**:
   - Crie humor com baixo nível (1-2)
   - Crie sprint com alta produtividade (>85)
   - Execute geração de alertas
   - Deve detectar risco de burnout

4. **Teste múltiplos textos**:
   - Envie uma lista de textos
   - Veja a análise agregada
   - Compare com análises individuais

---

## 📚 Recursos Adicionais

- **Swagger UI**: `http://localhost:5000/swagger`
- **Health Check**: `http://localhost:5000/health`
- **Documentação completa**: Veja o README.md

---

## ✅ Checklist de Testes

- [ ] Status das funcionalidades de ML
- [ ] Análise de sentimento (texto negativo)
- [ ] Análise de sentimento (texto positivo)
- [ ] Análise de sentimento (texto neutro)
- [ ] Análise de múltiplos textos
- [ ] Classificação de imagem (com descrição)
- [ ] Classificação de imagem (sem descrição)
- [ ] Análise completa de bem-estar
- [ ] Geração de alertas inteligentes
- [ ] Integração com dados existentes

---

**Bons testes! 🚀**


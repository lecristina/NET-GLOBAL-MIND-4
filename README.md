# 🧠 MindTrack API - Plataforma de Bem-Estar para Profissionais de TI

## 🚀 ADVANCED BUSINESS DEVELOPMENT WITH .NET

## 👥 Integrantes
- **André Rogério Vieira Pavanela Altobelli Antunes**, RM: 554764
- **Enrico Figueiredo Del Guerra**, RM: 558604
- **Leticia Cristina Dos Santos Passos**, RM: 555241

---

## 📋 Visão Geral do Projeto

**MindTrack** é uma plataforma integrada (web + mobile) voltada para profissionais e equipes de tecnologia da informação.

Ela combina inteligência artificial, análise de dados e gamificação para monitorar o bem-estar emocional, promover equilíbrio entre produtividade e descanso, e ajudar profissionais de TI e gestores a prevenir burnout e melhorar a performance sustentável.

O sistema é voltado tanto para profissionais individuais (devs, analistas, testers, etc.) quanto para empresas e squads, promovendo um ambiente mais saudável, empático e humano.

### 🎯 Objetivos do Projeto

- **Monitoramento de Bem-Estar**: Acompanhamento contínuo do estado emocional e energético dos profissionais
- **Prevenção de Burnout**: Identificação precoce de sinais de sobrecarga e estresse
- **Gamificação**: Sistema de badges e pontuação para motivar hábitos saudáveis
- **Análise de Produtividade**: Tracking de sprints, tarefas e commits
- **Alertas Inteligentes**: IA para detectar padrões e gerar recomendações

---

## ✅ Funcionalidades Implementadas

### 1. Boas Práticas REST (30 pontos) ✅

- ✅ **Web API** com arquitetura limpa e escalável
- ✅ **Endpoints RESTful** seguindo convenções HTTP (GET, POST, PUT, DELETE)
- ✅ **Paginação** em todos os endpoints de listagem
- ✅ **HATEOAS** implementado com links de navegação
- ✅ **Status codes adequados** (200, 201, 204, 400, 404, 409, 500)
- ✅ **Validação de dados** com Data Annotations
- ✅ **Tratamento de erros** padronizado
- ✅ **Content Negotiation** (JSON)

### 2. Monitoramento e Observabilidade (15 pontos) ✅

- ✅ **Health Check Geral**: Status da aplicação (`/health`)
- ✅ **Health Check do Banco**: Conexão Oracle (`/health/database`)
- ✅ **Health Check da Memória**: Monitoramento de recursos (`/health/memory`)
- ✅ **Health Check Ready**: Verificação de prontidão (`/health/ready`)
- ✅ **Health Check Live**: Verificação de vida (`/health/live`)
- ✅ **Logging** estruturado com ILogger
- ✅ **Tracing** com Application Insights
- ✅ **Métricas** de performance

### 3. Versionamento da API (10 pontos) ✅

- ✅ **Versão única**: v1.0 (anterior v2.0 transformada em v1.0)
- ✅ **Versionamento por URL**: `/api/v1.0/`
- ✅ **Swagger** configurado para documentação
- ✅ **Estratégia de versionamento** documentada
- ✅ **Backward compatibility** mantida

### 4. Integração e Persistência (30 pontos) ✅

- ✅ **Entity Framework Core** com Migrations
- ✅ **Oracle Database** como banco de dados relacional
- ✅ **Repositório Pattern** implementado
- ✅ **Unit of Work** pattern
- ✅ **Migrations** para controle de versão do banco
- ✅ **Transações** e rollback automático
- ✅ **Relacionamentos** entre entidades configurados

### 5. Testes Integrados (15 pontos) ✅

- ✅ **Testes unitários** com xUnit
- ✅ **Testes de integração** com WebApplicationFactory
- ✅ **Cobertura de testes** para serviços críticos
- ✅ **Mocks** e stubs implementados
- ✅ **Testes de autenticação** e autorização
- ✅ **Testes de endpoints** completos

### 6. Autenticação e Segurança (Opcional - Implementado) ✅

- ✅ **Autenticação JWT** completa
- ✅ **Autorização baseada em roles**: PROFISSIONAL, GESTOR
- ✅ **Token validation** e refresh
- ✅ **Claims personalizados** para controle de acesso
- ✅ **Middleware de segurança** configurado
- ✅ **Hash de senhas** com BCrypt
- ✅ **Swagger com autenticação** JWT

### 7. Machine Learning com ML.NET (Opcional - Em desenvolvimento) 🔄

- 🔄 **Placeholder** para análise de bem-estar
- 🔄 **Estrutura preparada** para implementação futura
- 🔄 **Endpoints** preparados para ML

---

## 🏗️ Arquitetura do Projeto

```
MindTrack API/
├── 📁 Controllers/              # Controladores da API
│   ├── AuthController.cs       # Autenticação JWT
│   ├── UsuariosController.cs   # Gestão de usuários
│   ├── HumorController.cs      # Registros de humor
│   ├── SprintsController.cs    # Gestão de sprints
│   ├── AlertasIAController.cs  # Alertas de IA
│   ├── HabitosController.cs    # Hábitos saudáveis
│   ├── BadgesController.cs     # Sistema de badges
│   ├── MLController.cs         # Machine Learning (placeholder)
│   └── HealthController.cs     # Health Checks
├── 📁 Services/                # Camada de serviços
│   ├── Auth/
│   │   └── JwtService.cs      # Serviço JWT
│   ├── UsuarioService.cs       # Serviço de usuários
│   ├── HumorService.cs         # Serviço de humor
│   ├── SprintService.cs        # Serviço de sprints
│   ├── AlertaIAService.cs      # Serviço de alertas IA
│   ├── HabitoService.cs        # Serviço de hábitos
│   ├── BadgeService.cs         # Serviço de badges
│   ├── BaseService.cs          # Classe base com HATEOAS
│   └── HealthChecks/           # Health Check services
├── 📁 Repositories/            # Camada de dados
│   ├── Interfaces/              # Contratos dos repositórios
│   │   ├── IRepository.cs
│   │   ├── IUsuarioRepository.cs
│   │   ├── IHumorRepository.cs
│   │   ├── ISprintRepository.cs
│   │   ├── IAlertaIARepository.cs
│   │   ├── IHabitoRepository.cs
│   │   └── IBadgeRepository.cs
│   └── Repository.cs           # Implementação base
│   ├── UsuarioRepository.cs
│   ├── HumorRepository.cs
│   ├── SprintRepository.cs
│   ├── AlertaIARepository.cs
│   ├── HabitoRepository.cs
│   └── BadgeRepository.cs
├── 📁 Models/                  # Entidades e DTOs
│   ├── Usuario.cs              # Entidade de usuário
│   ├── Humor.cs                # Entidade de humor
│   ├── Sprint.cs               # Entidade de sprint
│   ├── AlertaIA.cs             # Entidade de alerta IA
│   ├── Habito.cs               # Entidade de hábito
│   ├── Badge.cs                # Entidade de badge
│   ├── UsuarioBadge.cs         # Relação usuário-badge
│   └── DTOs/                   # Data Transfer Objects
│       ├── UsuarioDto.cs
│       ├── HumorDto.cs
│       ├── SprintDto.cs
│       ├── AlertaIADto.cs
│       ├── HabitoDto.cs
│       ├── BadgeDto.cs
│       └── CommonDto.cs        # PagedResultDto, LinkDto, etc.
├── 📁 Data/                    # Contexto do banco
│   └── ApplicationDbContext.cs # EF Core Context
├── 📁 Services/Mapping/        # Configuração AutoMapper
│   └── AutoMapperProfile.cs
├── 📁 Tests/                   # Testes
│   ├── Unit/                   # Testes unitários
│   │   └── JwtServiceTests.cs
│   └── Integration/            # Testes de integração
│       ├── CustomWebApplicationFactory.cs
│       ├── UsuarioIntegrationTests.cs
│       ├── AuthIntegrationTests.cs
│       └── HealthCheckIntegrationTests.cs
└── 📁 Migrations/              # Migrações do banco
    └── 20250101000000_MindTrackInitialCreate.cs
```

---

## 🛠️ Tecnologias Utilizadas

| Tecnologia | Versão | Uso |
|------------|--------|-----|
| **.NET** | 9.0 | Framework principal |
| **ASP.NET Core** | 9.0 | Web API |
| **Entity Framework Core** | 9.0 | ORM |
| **Oracle.EntityFrameworkCore** | 9.0 | Provider Oracle |
| **Oracle Database** | - | Banco de dados relacional |
| **JWT Bearer** | - | Autenticação |
| **ML.NET** | - | Machine Learning (preparado) |
| **xUnit** | - | Framework de testes |
| **Moq** | - | Mocking para testes |
| **AutoMapper** | - | Mapeamento de objetos |
| **Swagger/OpenAPI** | - | Documentação da API |
| **BCrypt.Net** | - | Hash de senhas |
| **Application Insights** | - | Telemetria e observabilidade |

---

## 🗄️ Estrutura do Banco de Dados

O banco de dados MindTrack possui as seguintes tabelas:

| Tabela | Descrição | Principais Campos |
|--------|-----------|-------------------|
| **t_mt_usuarios** | Usuários do sistema | id_usuario, nome, email, senha_hash, perfil, empresa |
| **t_mt_humor** | Registros de humor e energia | id_humor, id_usuario, nivel_humor, nivel_energia, comentario |
| **t_mt_sprints** | Sprints de trabalho | id_sprint, id_usuario, nome_sprint, produtividade, tarefas_concluidas |
| **t_mt_alertas_ia** | Alertas gerados por IA | id_alerta, id_usuario, tipo_alerta, nivel_risco, mensagem |
| **t_mt_habitos** | Hábitos saudáveis registrados | id_habito, id_usuario, tipo_habito, pontuacao |
| **t_mt_badges** | Badges disponíveis no sistema | id_badge, nome_badge, pontos_requeridos |
| **t_mt_usuario_badges** | Relação usuário-badge (conquistas) | id_usuario, id_badge, data_conquista |

### Relacionamentos

- `Usuario` → `Humor` (1:N)
- `Usuario` → `Sprint` (1:N)
- `Usuario` → `AlertaIA` (1:N)
- `Usuario` → `Habito` (1:N)
- `Usuario` ↔ `Badge` (N:N via `UsuarioBadge`)

---

## 🚀 Como Executar o Projeto

### 📋 Pré-requisitos

- **.NET 9 SDK** instalado ([Download aqui](https://dotnet.microsoft.com/download))
- **Oracle Database** configurado e acessível
- **Visual Studio 2022** (Community, Professional ou Enterprise) ou **VS Code** com extensão C#
- **Oracle Client** instalado (para conexão com banco)
- **Git** (opcional, para clonar o repositório)

### 🔧 Configuração Inicial

1. **Clone o repositório** (ou extraia o arquivo ZIP):
```bash
   git clone <repository-url>
   cd nexus-gs-1-net
   ```
   
   Ou se você já tem o projeto:
   ```bash
   cd nexus-gs-1-net
   ```

2. **Execute o script SQL para criar as tabelas**:
   - Abra o arquivo `create-mindtrack-tables.sql` (na raiz do projeto)
   - Execute o script completo no Oracle SQL Developer ou outra ferramenta de acesso ao Oracle
   - O script cria todas as tabelas necessárias: `t_mt_usuarios`, `t_mt_humor`, `t_mt_sprints`, `t_mt_alertas_ia`, `t_mt_habitos`, `t_mt_badges`, `t_mt_usuario_badges`

3. **Configure a conexão com o banco de dados**:
   - Abra o arquivo `appsettings.json` na raiz do projeto
   - Atualize a `ConnectionStrings` com suas credenciais:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=oracle.fiap.com.br:1521/ORCL;User Id=SEU_USUARIO;Password=SUA_SENHA;Connection Timeout=30;"
     },
     "JwtSettings": {
       "SecretKey": "MindTrack_Super_Secret_Key_2024_Advanced_Business_Development_With_DotNet",
       "Issuer": "MindTrackAPI",
       "Audience": "MindTrackUsers"
     }
   }
   ```

4. **Restaurar dependências do NuGet**:
   ```bash
   dotnet restore
   ```
   
   Isso baixará todos os pacotes necessários (Entity Framework Core, Oracle Provider, JWT, Swagger, etc.)

5. **Aplicar as Migrations do Entity Framework** (opcional, se usar EF Migrations):
```bash
   dotnet ef database update
   ```
   
   **Nota**: Se você já executou o script SQL manualmente, pode pular esta etapa.

---

## 💻 Como Abrir e Executar no Visual Studio 2022

### 📂 Abrindo o Projeto

1. **Abra o Visual Studio 2022**

2. **Opção 1 - Abrir pela Solution**:
   - Clique em `File` → `Open` → `Project/Solution...`
   - Navegue até a pasta do projeto
   - Selecione o arquivo `challenge-3-net.sln`
   - Clique em `Open`

3. **Opção 2 - Abrir pela Pasta**:
   - Clique em `File` → `Open` → `Folder...`
   - Navegue até a pasta `nexus-gs-1-net`
   - Clique em `Select Folder`

4. **Aguarde o Visual Studio**:
   - Restaurar os pacotes NuGet automaticamente
   - Compilar o projeto
   - Resolver dependências

### ▶️ Executando o Projeto

1. **Selecione o perfil de execução**:
   - No topo da tela, ao lado do botão de executar, você verá um dropdown
   - Selecione `https` (recomendado) ou `http`
   - **Perfil `https`**: Executa em `https://localhost:5001` e `http://localhost:5000`
   - **Perfil `http`**: Executa apenas em `http://localhost:5000`

2. **Execute o projeto**:
   - Pressione `F5` (com debug) ou `Ctrl+F5` (sem debug)
   - Ou clique no botão verde ▶️ "IIS Express" ou "nexus-gs-1-net"
   - Ou clique com botão direito no projeto no Solution Explorer → `Debug` → `Start New Instance`

3. **Aguarde a inicialização**:
   - O Visual Studio abrirá automaticamente o navegador padrão
   - Você será redirecionado para o Swagger UI: `https://localhost:5001/swagger` ou `http://localhost:5000/swagger`

### 🔍 Debugging no Visual Studio

- **Breakpoints**: Clique na margem esquerda do editor para adicionar breakpoints
- **Inspeção de variáveis**: Passe o mouse sobre variáveis durante o debug
- **Watch Window**: Adicione variáveis para monitorar durante a execução
- **Call Stack**: Veja a pilha de chamadas no painel de debug
- **Output Window**: Veja logs e mensagens de console

### 🛠️ Configurações de Build

- **Build Solution**: `Ctrl+Shift+B`
- **Rebuild Solution**: `Build` → `Rebuild Solution`
- **Clean Solution**: `Build` → `Clean Solution`

---

## 📝 Como Abrir e Executar no Visual Studio Code

### 📂 Abrindo o Projeto

1. **Abra o VS Code**

2. **Instale as extensões necessárias** (se ainda não tiver):
   - **C#** (Microsoft) - Extensão ID: `ms-dotnettools.csharp`
   - **C# Dev Kit** (Microsoft) - Extensão ID: `ms-dotnettools.csdevkit` (opcional, mas recomendado)
   - **.NET Extension Pack** (Microsoft) - Extensão ID: `ms-dotnettools.vscode-dotnet-pack` (recomendado)

3. **Abra a pasta do projeto**:
   - Clique em `File` → `Open Folder...`
   - Navegue até a pasta `nexus-gs-1-net`
   - Clique em `Select Folder`

4. **Aguarde o VS Code**:
   - A extensão C# detectará automaticamente o projeto .NET
   - Restaurará os pacotes NuGet
   - Compilará o projeto

### ▶️ Executando o Projeto

1. **Método 1 - Terminal Integrado** (Recomendado):
   - Pressione `` Ctrl+` `` (Ctrl + crase) para abrir o terminal integrado
   - Execute:
     ```bash
     dotnet run
     ```
   - Ou para especificar o perfil:
     ```bash
     dotnet run --launch-profile https
     ```
     ```bash
     dotnet run --launch-profile http
     ```

2. **Método 2 - Menu de Comandos**:
   - Pressione `Ctrl+Shift+P` para abrir a paleta de comandos
   - Digite: `.NET: Run Project`
   - Selecione o perfil desejado (`https` ou `http`)

3. **Método 3 - Debug**:
   - Pressione `F5` para iniciar o debug
   - O VS Code pedirá para criar um arquivo `launch.json` (aceite)
   - Configure o perfil de debug se necessário
   - O projeto será executado e o navegador abrirá automaticamente

### 🔍 Debugging no VS Code

1. **Adicione breakpoints**:
   - Clique na margem esquerda do editor (ao lado dos números de linha)

2. **Inicie o debug**:
   - Pressione `F5`
   - Ou vá em `Run` → `Start Debugging`

3. **Painéis de Debug**:
   - **Variables**: Variáveis locais e globais
   - **Watch**: Expressões personalizadas
   - **Call Stack**: Pilha de chamadas
   - **Breakpoints**: Lista de breakpoints

### 🛠️ Comandos Úteis no Terminal

```bash
# Restaurar pacotes
dotnet restore

# Compilar o projeto
dotnet build

# Executar o projeto
dotnet run

# Executar com perfil específico
dotnet run --launch-profile https
dotnet run --launch-profile http

# Executar testes
dotnet test

# Aplicar migrations
dotnet ef database update

# Criar nova migration
dotnet ef migrations add NomeDaMigration
```

---

## 🌐 Portas e URLs da Aplicação

### 📍 Portas Padrão

O projeto está configurado para usar as seguintes portas:

- **HTTP**: `5000`
- **HTTPS**: `5001`

### 🔗 URLs de Acesso

Após executar o projeto, você pode acessar:

| Recurso | URL HTTP | URL HTTPS |
|---------|----------|-----------|
| **API Base** | `http://localhost:5000` | `https://localhost:5001` |
| **Swagger UI** | `http://localhost:5000/swagger` | `https://localhost:5001/swagger` |
| **Health Check Geral** | `http://localhost:5000/health` | `https://localhost:5001/health` |
| **Health Check Database** | `http://localhost:5000/health/database` | `https://localhost:5001/health/database` |
| **Health Check Memory** | `http://localhost:5000/health/memory` | `https://localhost:5001/health/memory` |
| **Health Check Ready** | `http://localhost:5000/health/ready` | `https://localhost:5001/health/ready` |
| **Health Check Live** | `http://localhost:5000/health/live` | `https://localhost:5001/health/live` |

### ⚙️ Alterando as Portas

Se você precisar alterar as portas, edite o arquivo `Properties/launchSettings.json`:

```json
{
  "profiles": {
    "http": {
      "applicationUrl": "http://localhost:SUA_PORTA_AQUI"
    },
    "https": {
      "applicationUrl": "https://localhost:SUA_PORTA_HTTPS;http://localhost:SUA_PORTA_HTTP"
    }
  }
}
```

**Importante**: Certifique-se de que as portas escolhidas não estejam em uso por outros aplicativos.

---

## 🏃 Executando o Projeto via Terminal/Command Prompt

### 📋 Comandos Básicos

1. **Navegue até a pasta do projeto**:
   ```bash
   cd C:\Users\crist\Downloads\challenge4-net-main\nexus-gs-1-net
   ```

2. **Restaurar dependências** (primeira vez ou após mudanças):
   ```bash
   dotnet restore
   ```

3. **Compilar o projeto**:
   ```bash
   dotnet build
   ```

4. **Executar o projeto**:
   ```bash
   # Executa com o perfil padrão (https)
   dotnet run
   
   # Executa apenas HTTP
   dotnet run --launch-profile http
   
   # Executa HTTPS (recomendado)
   dotnet run --launch-profile https
   ```

5. **Executar em modo Release** (otimizado):
   ```bash
   dotnet run --configuration Release
   ```

### 🧪 Executando Testes

```bash
# Executar todos os testes
dotnet test

# Executar testes com detalhes
dotnet test --verbosity normal

# Executar testes de um projeto específico
dotnet test Tests/Unit/JwtServiceTests.cs
```

### 🔄 Aplicando Migrations

```bash
# Aplicar todas as migrations pendentes
dotnet ef database update

# Criar uma nova migration
dotnet ef migrations add NomeDaMigration

# Remover a última migration (antes de aplicar)
dotnet ef migrations remove
```

---

## 🛑 Parando a Aplicação

### No Terminal/Command Prompt:
- Pressione `Ctrl+C` para parar a aplicação

### No Visual Studio:
- Clique no botão de parar (quadrado vermelho) na barra de ferramentas
- Ou pressione `Shift+F5`

### No VS Code:
- Clique no botão de parar no painel de debug
- Ou pressione `Shift+F5`
- Ou feche o terminal onde o projeto está rodando

---

## 📚 Endpoints da API

### 📝 Exemplos de JSON para Testes

Esta seção contém exemplos de JSON para todos os endpoints que requerem body (POST, PUT).

---

### 🔐 Autenticação

**Descrição Geral**: Os endpoints de autenticação permitem que usuários façam login, obtenham tokens JWT, validem tokens e verifiquem suas permissões no sistema. O sistema utiliza JWT (JSON Web Tokens) para autenticação stateless, onde o token contém informações do usuário (ID, nome, email, perfil) e é usado para autorizar requisições subsequentes.

**Como Funciona**:
1. O usuário faz login com email e senha
2. O sistema valida as credenciais e retorna um token JWT
3. O token deve ser incluído no header `Authorization: Bearer {token}` em todas as requisições protegidas
4. O token expira após 1 hora (3600 segundos)
5. Para continuar usando a API, o usuário deve fazer login novamente após a expiração

Todos os endpoints de autenticação são públicos (não requerem token), exceto `/me` e `/check-admin` que requerem autenticação.

| Método | Endpoint | Descrição | Autenticação | Para que serve |
|--------|----------|-----------|--------------|---------------|
| `POST` | `/api/v1.0/Auth/login` | Login e obtenção de token JWT | Não | Permite que usuários façam login no sistema fornecendo email e senha. Retorna um token JWT que deve ser usado em requisições subsequentes. |
| `POST` | `/api/v1.0/Auth/validate` | Validação de token | Não | Verifica se um token JWT é válido, não expirado e foi emitido pelo sistema. Útil para verificar se o token ainda pode ser usado. |
| `GET` | `/api/v1.0/Auth/me` | Informações do usuário atual | Sim (JWT) | Retorna as informações completas do usuário autenticado (ID, nome, email, perfil, empresa, data de cadastro). O ID do usuário é extraído do token JWT. |
| `GET` | `/api/v1.0/Auth/check-admin` | Verificar permissões | Sim (JWT) | Verifica se o usuário autenticado possui permissões de GESTOR. Retorna informações sobre as permissões do usuário (se é gestor, se é profissional, etc.). |

#### 📤 POST `/api/v1.0/Auth/login` - Login

**O que faz**: Autentica um usuário no sistema usando email e senha. Valida as credenciais no banco de dados e, se corretas, gera um token JWT contendo informações do usuário (ID, nome, email, perfil, empresa). O token é necessário para acessar todos os outros endpoints protegidos.

**Para que serve**: É o ponto de entrada para o sistema. Sem fazer login e obter um token, o usuário não consegue acessar nenhum recurso protegido da API.

**Request:**
```json
{
  "email": "usuario@example.com",
  "senha": "senha123"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
  "tokenType": "Bearer",
  "expiresIn": 3600,
  "message": "Login realizado com sucesso"
}
```

#### 📤 POST `/api/v1.0/Auth/validate` - Validar Token

**O que faz**: Valida se um token JWT fornecido é válido, não expirado e foi emitido pelo sistema. Verifica a assinatura, o emissor (Issuer), a audiência (Audience) e o tempo de expiração.

**Para que serve**: Permite que aplicações cliente verifiquem se um token armazenado ainda é válido antes de fazer requisições à API. Útil para implementar refresh de tokens ou verificar se o usuário ainda está autenticado.

**Request:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Response (200 OK):**
```json
{
  "isValid": true,
  "message": "Token válido"
}
```

---

### 👥 Usuários

**Descrição Geral**: Os endpoints de usuários permitem gerenciar contas de usuários no sistema MindTrack. Usuários podem ser criados, consultados, atualizados e excluídos. O sistema suporta dois perfis: **PROFISSIONAL** (desenvolvedores, analistas, testers) e **GESTOR** (gerentes, líderes de equipe). A criação de usuários é pública (não requer autenticação), mas todas as outras operações requerem autenticação.

**Para que serve**: Permite o cadastro e gerenciamento de profissionais de TI que usarão a plataforma MindTrack para monitorar seu bem-estar, registrar sprints, receber alertas de IA e conquistar badges.

#### 📤 POST `/api/v1.0/Usuarios` - Criar Usuário

**O que faz**: Cria um novo usuário no sistema. A senha é automaticamente hasheada usando BCrypt antes de ser armazenada no banco de dados. Valida se o email já existe (não permite duplicatas) e se os dados fornecidos são válidos.

**Para que serve**: Permite que novos profissionais se cadastrem na plataforma. É o primeiro passo para usar o MindTrack. Após criar a conta, o usuário pode fazer login e começar a usar os recursos da plataforma.

**Request:**
```json
{
  "nome": "João Silva",
  "email": "joao.silva@example.com",
  "senha": "senha123456",
  "perfil": "PROFISSIONAL",
  "empresa": "Tech Solutions"
}
```

**Response (201 Created):**
```json
{
  "idUsuario": 1,
  "nome": "João Silva",
  "email": "joao.silva@example.com",
  "perfil": "PROFISSIONAL",
  "dataCadastro": "2024-01-15T10:30:00Z",
  "empresa": "Tech Solutions",
  "links": [
    {
      "href": "/api/v1.0/Usuarios/1",
      "rel": "self",
      "method": "GET"
    },
    {
      "href": "/api/v1.0/Usuarios/1",
      "rel": "update",
      "method": "PUT"
    },
    {
      "href": "/api/v1.0/Usuarios/1",
      "rel": "delete",
      "method": "DELETE"
    }
  ]
}
```

#### 📤 PUT `/api/v1.0/Usuarios/{id}` - Atualizar Usuário

**O que faz**: Atualiza as informações de um usuário existente. Permite alterar nome, email, perfil e empresa. Não permite alterar a senha (para isso, seria necessário um endpoint específico de alteração de senha).

**Para que serve**: Permite que usuários atualizem suas informações cadastrais ou que gestores promovam profissionais a gestores (alterando o perfil de PROFISSIONAL para GESTOR).

**Request:**
```json
{
  "nome": "João Silva Santos",
  "email": "joao.silva.santos@example.com",
  "perfil": "GESTOR",
  "empresa": "Tech Solutions Brasil"
}
```

**Response (200 OK):**
```json
{
  "idUsuario": 1,
  "nome": "João Silva Santos",
  "email": "joao.silva.santos@example.com",
  "perfil": "GESTOR",
  "dataCadastro": "2024-01-15T10:30:00Z",
  "empresa": "Tech Solutions Brasil",
  "links": [
    {
      "href": "/api/v1.0/Usuarios/1",
      "rel": "self",
      "method": "GET"
    }
  ]
}
```

---

### 😊 Humor

**Descrição Geral**: Os endpoints de humor permitem que profissionais registrem seu estado emocional e nível de energia ao longo do tempo. Cada registro contém um nível de humor (1-5), um nível de energia (1-5) e um comentário opcional. O sistema usa esses dados para identificar padrões de bem-estar e gerar alertas de IA quando necessário.

**Para que serve**: É a funcionalidade central do MindTrack para monitoramento de bem-estar. Permite que profissionais registrem como estão se sentindo, permitindo que o sistema e gestores identifiquem sinais de burnout, sobrecarga ou desequilíbrio entre trabalho e descanso.

**Como usar**: Profissionais devem registrar seu humor regularmente (diariamente ou várias vezes ao dia) para que o sistema tenha dados suficientes para análise. O ID do usuário é automaticamente extraído do token JWT, então não é necessário enviar no body.

#### 📤 POST `/api/v1.0/Humor` - Criar Registro de Humor

**O que faz**: Cria um novo registro de humor e energia para o usuário autenticado. O ID do usuário é automaticamente obtido do token JWT, então não é necessário enviar no body. Valida que os níveis estão entre 1 e 5.

**Para que serve**: Permite que profissionais registrem como estão se sentindo em um determinado momento. Esses dados são usados para análise de padrões e geração de alertas de IA.

**Request:**
```json
{
  "nivelHumor": 4,
  "nivelEnergia": 3,
  "comentario": "Me senti bem hoje, mas um pouco cansado"
}
```

**Response (201 Created):**
```json
{
  "idUsuario": 1,
  "dataRegistro": "2024-01-15T14:30:00Z",
  "nivelHumor": 4,
  "nivelEnergia": 3,
  "comentario": "Me senti bem hoje, mas um pouco cansado",
  "links": [
    {
      "href": "/api/v1.0/Humor/1",
      "rel": "self",
      "method": "GET"
    },
    {
      "href": "/api/v1.0/Humor/1",
      "rel": "update",
      "method": "PUT"
    }
  ]
}
```

#### 📤 PUT `/api/v1.0/Humor/{id}` - Atualizar Registro de Humor

**O que faz**: Atualiza um registro de humor existente. Permite corrigir ou atualizar os níveis de humor, energia e o comentário de um registro já criado.

**Para que serve**: Permite que profissionais corrijam registros feitos por engano ou atualizem informações de registros anteriores.

**Request:**
```json
{
  "nivelHumor": 5,
  "nivelEnergia": 4,
  "comentario": "Agora me sinto muito melhor!"
}
```

**Response (200 OK):**
```json
{
  "idHumor": 1,
  "idUsuario": 1,
  "dataRegistro": "2024-01-15T14:30:00Z",
  "nivelHumor": 5,
  "nivelEnergia": 4,
  "comentario": "Agora me sinto muito melhor!",
  "links": [
    {
      "href": "/api/v1.0/Humor/1",
      "rel": "self",
      "method": "GET"
    }
  ]
}
```

---

### 🏃 Sprints

**Descrição Geral**: Os endpoints de sprints permitem que profissionais registrem informações sobre suas sprints de trabalho (períodos de desenvolvimento, geralmente de 1 a 4 semanas). Cada sprint contém informações sobre produtividade, tarefas concluídas, commits realizados e datas de início/fim.

**Para que serve**: Permite que profissionais e gestores acompanhem a produtividade ao longo do tempo, identificando padrões de performance, sobrecarga ou períodos de baixa produtividade que podem indicar necessidade de descanso ou ajustes no trabalho.

**Como usar**: Profissionais devem criar uma sprint no início de cada período de trabalho e atualizar com informações de produtividade ao longo da sprint. O ID do usuário é automaticamente extraído do token JWT.

#### 📤 POST `/api/v1.0/Sprints` - Criar Sprint

**O que faz**: Cria um novo registro de sprint para o usuário autenticado. O ID do usuário é automaticamente obtido do token JWT. Valida que a produtividade está entre 0.00 e 100.00 e que a data de fim é posterior à data de início.

**Para que serve**: Permite que profissionais registrem informações sobre suas sprints de trabalho, permitindo acompanhamento de produtividade e identificação de padrões.

**Request:**
```json
{
  "nomeSprint": "Sprint 1 - Feature Login",
  "dataInicio": "2024-01-15T09:00:00Z",
  "dataFim": "2024-01-29T18:00:00Z",
  "produtividade": 85.5,
  "tarefasConcluidas": 12,
  "commits": 45
}
```

**Response (201 Created):**
```json
{
  "idSprint": 1,
  "idUsuario": 1,
  "nomeSprint": "Sprint 1 - Feature Login",
  "dataInicio": "2024-01-15T09:00:00Z",
  "dataFim": "2024-01-29T18:00:00Z",
  "produtividade": 85.5,
  "tarefasConcluidas": 12,
  "commits": 45,
  "links": [
    {
      "href": "/api/v1.0/Sprints/1",
      "rel": "self",
      "method": "GET"
    },
    {
      "href": "/api/v1.0/Sprints/1",
      "rel": "update",
      "method": "PUT"
    }
  ]
}
```

#### 📤 PUT `/api/v1.0/Sprints/{id}` - Atualizar Sprint

**Request:**
```json
{
  "nomeSprint": "Sprint 1 - Feature Login (Atualizada)",
  "dataInicio": "2024-01-15T09:00:00Z",
  "dataFim": "2024-01-29T18:00:00Z",
  "produtividade": 92.0,
  "tarefasConcluidas": 15,
  "commits": 52
}
```

**Response (200 OK):**
```json
{
  "idSprint": 1,
  "idUsuario": 1,
  "nomeSprint": "Sprint 1 - Feature Login (Atualizada)",
  "dataInicio": "2024-01-15T09:00:00Z",
  "dataFim": "2024-01-29T18:00:00Z",
  "produtividade": 92.0,
  "tarefasConcluidas": 15,
  "commits": 52,
  "links": [
    {
      "href": "/api/v1.0/Sprints/1",
      "rel": "self",
      "method": "GET"
    }
  ]
}
```

---

### 🤖 Alertas de IA

**Descrição Geral**: Os endpoints de alertas de IA permitem que o sistema (ou gestores) criem alertas para profissionais baseados em análise de padrões. Alertas podem indicar risco de burnout, sobrecarga, necessidade de descanso, ou recomendações de bem-estar. Cada alerta tem um tipo, nível de risco (1-5) e uma mensagem personalizada.

**Para que serve**: É a funcionalidade de inteligência artificial do MindTrack. Permite que o sistema identifique padrões preocupantes nos dados de humor, energia e produtividade e alerte profissionais e gestores sobre possíveis problemas de bem-estar.

**Tipos de alerta comuns**:
- **Burnout**: Detectado quando há padrões consistentes de baixo humor, baixa energia e alta produtividade (indica sobrecarga)
- **Sobrecarga**: Detectado quando há muitas tarefas concluídas mas baixo bem-estar
- **Equilíbrio**: Recomendação positiva quando o profissional está mantendo bom equilíbrio
- **Produtividade**: Alertas sobre padrões de produtividade
- **Bem-estar**: Recomendações gerais de bem-estar

#### 📤 POST `/api/v1.0/AlertasIA` - Criar Alerta de IA

**O que faz**: Cria um novo alerta de IA para o usuário autenticado. O ID do usuário é automaticamente obtido do token JWT. Valida que o nível de risco está entre 1 e 5.

**Para que serve**: Permite que o sistema de IA (ou gestores) criem alertas personalizados para profissionais baseados em análise de padrões de bem-estar e produtividade.

**Request:**
```json
{
  "tipoAlerta": "Burnout",
  "mensagem": "Nível de estresse elevado detectado. Recomenda-se pausa.",
  "nivelRisco": 4
}
```

**Response (201 Created):**
```json
{
  "idAlerta": 1,
  "idUsuario": 1,
  "dataAlerta": "2024-01-15T16:00:00Z",
  "tipoAlerta": "Burnout",
  "mensagem": "Nível de estresse elevado detectado. Recomenda-se pausa.",
  "nivelRisco": 4,
  "links": [
    {
      "href": "/api/v1.0/AlertasIA/1",
      "rel": "self",
      "method": "GET"
    },
    {
      "href": "/api/v1.0/AlertasIA/1",
      "rel": "delete",
      "method": "DELETE"
    }
  ]
}
```

**Outros exemplos de `tipoAlerta`:**
- `"Sobrecarga"`
- `"Equilíbrio"`
- `"Produtividade"`
- `"Bem-estar"`

---

### 🎯 Hábitos

**Descrição Geral**: Os endpoints de hábitos permitem que profissionais registrem hábitos saudáveis que praticam, como hidratação, pausas ativas, meditação, exercícios, alimentação saudável e sono adequado. Cada hábito registrado gera pontuação que contribui para o sistema de gamificação e conquista de badges.

**Para que serve**: É a funcionalidade de gamificação do MindTrack. Incentiva profissionais a adotarem hábitos saudáveis através de um sistema de pontuação e badges. Quanto mais hábitos saudáveis um profissional pratica, mais pontos ele ganha e mais badges ele pode conquistar.

**Tipos de hábitos comuns**:
- **Hidratação**: Registrar consumo adequado de água
- **Pausa ativa**: Registrar pausas para alongamento ou caminhada
- **Meditação**: Registrar sessões de meditação ou mindfulness
- **Exercício**: Registrar atividades físicas
- **Alimentação saudável**: Registrar refeições balanceadas
- **Sono adequado**: Registrar horas de sono adequadas

#### 📤 POST `/api/v1.0/Habitos` - Criar Hábito

**O que faz**: Cria um novo registro de hábito saudável para o usuário autenticado. O ID do usuário é automaticamente obtido do token JWT. Se a data não for fornecida, usa a data/hora atual. Cada hábito tem uma pontuação que contribui para o total de pontos do usuário.

**Para que serve**: Permite que profissionais registrem hábitos saudáveis que praticam, ganhando pontos e contribuindo para conquista de badges.

**Request:**
```json
{
  "tipoHabito": "Hidratação",
  "dataHabito": "2024-01-15T10:00:00Z",
  "pontuacao": 10
}
```

**Response (201 Created):**
```json
{
  "idHabito": 1,
  "idUsuario": 1,
  "tipoHabito": "Hidratação",
  "dataHabito": "2024-01-15T10:00:00Z",
  "pontuacao": 10,
  "links": [
    {
      "href": "/api/v1.0/Habitos/1",
      "rel": "self",
      "method": "GET"
    },
    {
      "href": "/api/v1.0/Habitos/1",
      "rel": "delete",
      "method": "DELETE"
    }
  ]
}
```

**Outros exemplos de `tipoHabito`:**
- `"Pausa ativa"`
- `"Meditação"`
- `"Exercício"`
- `"Alimentação saudável"`
- `"Sono adequado"`

**Request alternativo (sem data, usa data atual):**
```json
{
  "tipoHabito": "Pausa ativa",
  "pontuacao": 15
}
```

---

### 🏆 Badges

**Descrição Geral**: Os endpoints de badges permitem que gestores criem e gerenciem badges (conquistas) no sistema. Badges são recompensas que profissionais podem conquistar ao atingir certos objetivos (como acumular pontos, praticar hábitos saudáveis, manter bom bem-estar, etc.). Apenas gestores podem criar e atualizar badges, mas todos os profissionais podem visualizar badges disponíveis e suas próprias conquistas.

**Para que serve**: É a funcionalidade de gamificação e reconhecimento do MindTrack. Badges incentivam profissionais a manterem hábitos saudáveis e bom bem-estar através de reconhecimento e conquistas. Profissionais podem ver quais badges conquistaram e quais ainda podem conquistar.

**Como funciona**:
1. Gestores criam badges definindo nome, descrição e pontos requeridos
2. Profissionais ganham pontos ao praticar hábitos saudáveis
3. Quando um profissional atinge os pontos requeridos, ele conquista o badge automaticamente
4. Profissionais podem visualizar todos os badges disponíveis e seus próprios badges conquistados

#### 📤 POST `/api/v1.0/Badges` - Criar Badge (Apenas GESTOR)

**O que faz**: Cria um novo badge no sistema. Apenas usuários com perfil GESTOR podem criar badges. Define o nome, descrição e pontos requeridos para conquistar o badge.

**Para que serve**: Permite que gestores criem novos badges para incentivar comportamentos saudáveis e reconhecer conquistas dos profissionais.

**Request:**
```json
{
  "nomeBadge": "Equilíbrio Mental",
  "descricao": "Conquistado por manter equilíbrio entre trabalho e descanso",
  "pontosRequeridos": 100
}
```

**Response (201 Created):**
```json
{
  "idBadge": 1,
  "nomeBadge": "Equilíbrio Mental",
  "descricao": "Conquistado por manter equilíbrio entre trabalho e descanso",
  "pontosRequeridos": 100,
  "links": [
    {
      "href": "/api/v1.0/Badges/1",
      "rel": "self",
      "method": "GET"
    },
    {
      "href": "/api/v1.0/Badges/1",
      "rel": "update",
      "method": "PUT"
    }
  ]
}
```

#### 📤 PUT `/api/v1.0/Badges/{id}` - Atualizar Badge (Apenas GESTOR)

**Request:**
```json
{
  "nomeBadge": "Equilíbrio Mental Pro",
  "descricao": "Conquistado por manter excelente equilíbrio entre trabalho e descanso",
  "pontosRequeridos": 150
}
```

**Response (200 OK):**
```json
{
  "idBadge": 1,
  "nomeBadge": "Equilíbrio Mental Pro",
  "descricao": "Conquistado por manter excelente equilíbrio entre trabalho e descanso",
  "pontosRequeridos": 150,
  "links": [
    {
      "href": "/api/v1.0/Badges/1",
      "rel": "self",
      "method": "GET"
    }
  ]
}
```

#### 📤 POST `/api/v1.0/Badges/usuario/{usuarioId}/badge/{badgeId}` - Conceder Badge a Usuário (Apenas GESTOR)

**Request:** (sem body, apenas path parameters)

**Response (201 Created):**
```json
{
  "idUsuario": 1,
  "idBadge": 1,
  "dataConquista": "2024-01-15T17:00:00Z",
  "badge": {
    "idBadge": 1,
    "nomeBadge": "Equilíbrio Mental",
    "descricao": "Conquistado por manter equilíbrio entre trabalho e descanso",
    "pontosRequeridos": 100,
    "links": []
  },
  "links": [
    {
      "href": "/api/v1.0/Badges/usuario/1/badge/1",
      "rel": "self",
      "method": "GET"
    }
  ]
}
```

---

### 📋 Exemplos de Respostas de Erro

#### 400 Bad Request - Validação
```json
{
  "code": "VALIDATION_ERROR",
  "message": "Dados de entrada inválidos",
  "details": [
    "Nome é obrigatório",
    "Email deve ter um formato válido"
  ],
  "timestamp": "2024-01-15T10:30:00Z",
  "validationErrors": {
    "nome": ["Nome é obrigatório"],
    "email": ["Email deve ter um formato válido"]
  }
}
```

#### 401 Unauthorized - Não autenticado
```json
{
  "code": "UNAUTHORIZED",
  "message": "Token JWT inválido ou ausente",
  "details": "É necessário fazer login para acessar este recurso",
  "timestamp": "2024-01-15T10:30:00Z"
}
```

#### 403 Forbidden - Sem permissão
```json
{
  "code": "FORBIDDEN",
  "message": "Acesso negado",
  "details": "Você não tem permissão para acessar este recurso",
  "timestamp": "2024-01-15T10:30:00Z"
}
```

#### 404 Not Found - Recurso não encontrado
```json
{
  "code": "NOT_FOUND",
  "message": "Recurso não encontrado",
  "details": "Usuário com ID 999 não foi encontrado",
  "timestamp": "2024-01-15T10:30:00Z"
}
```

#### 500 Internal Server Error
```json
{
  "code": "INTERNAL_ERROR",
  "message": "Erro interno do servidor",
  "details": "Ocorreu um erro ao processar sua solicitação",
  "timestamp": "2024-01-15T10:30:00Z"
}
```

---

### 📝 Notas Importantes

1. **Autenticação JWT**: Para endpoints protegidos, inclua o header:
   ```
   Authorization: Bearer {seu_token_jwt}
   ```

2. **ID do Usuário**: Nos endpoints de criação de `Humor`, `Sprint`, `AlertaIA` e `Habito`, o `idUsuario` é obtido automaticamente do token JWT. Não é necessário enviar no body.

3. **Perfis de Usuário**: 
   - `PROFISSIONAL`: Acesso básico aos recursos
   - `GESTOR`: Acesso completo, incluindo criação/edição de badges

4. **Validações**:
   - `nivelHumor` e `nivelEnergia`: valores entre 1 e 5
   - `nivelRisco`: valores entre 1 e 5
   - `produtividade`: valores entre 0.00 e 100.00
   - `perfil`: apenas `"PROFISSIONAL"` ou `"GESTOR"`

5. **Paginação**: Endpoints de listagem aceitam query parameters:
   - `pageNumber`: número da página (padrão: 1)
   - `pageSize`: tamanho da página (padrão: 10)

---

### 👥 Usuários

| Método | Endpoint | Descrição | Autenticação | Para que serve |
|--------|----------|-----------|--------------|---------------|
| `GET` | `/api/v1.0/Usuarios` | Listar usuários (paginado) | Sim (PROFISSIONAL, GESTOR) | Retorna uma lista paginada de todos os usuários cadastrados no sistema. Útil para gestores visualizarem todos os profissionais da plataforma. |
| `GET` | `/api/v1.0/Usuarios/{id}` | Buscar usuário por ID | Sim (PROFISSIONAL, GESTOR) | Retorna as informações completas de um usuário específico pelo seu ID. Útil para visualizar detalhes de um profissional. |
| `POST` | `/api/v1.0/Usuarios` | Criar usuário | Não (público) | Cria um novo usuário no sistema. Endpoint público que permite cadastro de novos profissionais. |
| `PUT` | `/api/v1.0/Usuarios/{id}` | Atualizar usuário | Sim (PROFISSIONAL, GESTOR) | Atualiza as informações de um usuário existente. Permite alterar nome, email, perfil e empresa. |
| `DELETE` | `/api/v1.0/Usuarios/{id}` | Excluir usuário | Sim (PROFISSIONAL, GESTOR) | Remove um usuário do sistema permanentemente. Use com cuidado, pois esta ação não pode ser desfeita. |

### 😊 Humor

| Método | Endpoint | Descrição | Autenticação | Para que serve |
|--------|----------|-----------|--------------|---------------|
| `GET` | `/api/v1.0/Humor` | Listar registros (paginado) | Sim (PROFISSIONAL, GESTOR) | Retorna uma lista paginada de todos os registros de humor do sistema. Útil para gestores visualizarem o bem-estar geral da equipe. |
| `GET` | `/api/v1.0/Humor/{id}` | Buscar registro por ID | Sim (PROFISSIONAL, GESTOR) | Retorna um registro específico de humor pelo seu ID. Útil para visualizar detalhes de um registro específico. |
| `GET` | `/api/v1.0/Humor/usuario/{usuarioId}` | Listar registros de um usuário | Sim (PROFISSIONAL, GESTOR) | Retorna todos os registros de humor de um usuário específico. Útil para visualizar o histórico de bem-estar de um profissional ao longo do tempo. |
| `POST` | `/api/v1.0/Humor` | Criar registro (ID do usuário vem do token) | Sim (PROFISSIONAL, GESTOR) | Cria um novo registro de humor e energia. O ID do usuário é automaticamente obtido do token JWT. |
| `PUT` | `/api/v1.0/Humor/{id}` | Atualizar registro | Sim (PROFISSIONAL, GESTOR) | Atualiza um registro de humor existente. Permite corrigir ou atualizar informações de registros anteriores. |
| `DELETE` | `/api/v1.0/Humor/{id}` | Excluir registro | Sim (PROFISSIONAL, GESTOR) | Remove um registro de humor do sistema permanentemente. Use com cuidado, pois esta ação não pode ser desfeita. |

### 🏃 Sprints

| Método | Endpoint | Descrição | Autenticação | Para que serve |
|--------|----------|-----------|--------------|---------------|
| `GET` | `/api/v1.0/Sprints` | Listar sprints (paginado) | Sim (PROFISSIONAL, GESTOR) | Retorna uma lista paginada de todas as sprints do sistema. Útil para gestores visualizarem a produtividade geral da equipe. |
| `GET` | `/api/v1.0/Sprints/{id}` | Buscar sprint por ID | Sim (PROFISSIONAL, GESTOR) | Retorna uma sprint específica pelo seu ID. Útil para visualizar detalhes de uma sprint específica. |
| `GET` | `/api/v1.0/Sprints/usuario/{usuarioId}` | Listar sprints de um usuário | Sim (PROFISSIONAL, GESTOR) | Retorna todas as sprints de um usuário específico. Útil para visualizar o histórico de produtividade de um profissional ao longo do tempo. |
| `POST` | `/api/v1.0/Sprints` | Criar sprint (ID do usuário vem do token) | Sim (PROFISSIONAL, GESTOR) | Cria um novo registro de sprint. O ID do usuário é automaticamente obtido do token JWT. |
| `PUT` | `/api/v1.0/Sprints/{id}` | Atualizar sprint | Sim (PROFISSIONAL, GESTOR) | Atualiza uma sprint existente. Permite atualizar informações de produtividade, tarefas concluídas e commits ao longo da sprint. |
| `DELETE` | `/api/v1.0/Sprints/{id}` | Excluir sprint | Sim (PROFISSIONAL, GESTOR) | Remove uma sprint do sistema permanentemente. Use com cuidado, pois esta ação não pode ser desfeita. |

### 🤖 Alertas de IA

| Método | Endpoint | Descrição | Autenticação | Para que serve |
|--------|----------|-----------|--------------|---------------|
| `GET` | `/api/v1.0/AlertasIA` | Listar alertas (paginado) | Sim (PROFISSIONAL, GESTOR) | Retorna uma lista paginada de todos os alertas de IA do sistema. Útil para gestores visualizarem todos os alertas gerados. |
| `GET` | `/api/v1.0/AlertasIA/{id}` | Buscar alerta por ID | Sim (PROFISSIONAL, GESTOR) | Retorna um alerta específico pelo seu ID. Útil para visualizar detalhes de um alerta específico. |
| `GET` | `/api/v1.0/AlertasIA/usuario/{usuarioId}` | Listar alertas de um usuário | Sim (PROFISSIONAL, GESTOR) | Retorna todos os alertas de IA de um usuário específico. Útil para profissionais visualizarem seus próprios alertas ou gestores visualizarem alertas de um profissional específico. |
| `POST` | `/api/v1.0/AlertasIA` | Criar alerta (ID do usuário vem do token) | Sim (PROFISSIONAL, GESTOR) | Cria um novo alerta de IA. O ID do usuário é automaticamente obtido do token JWT. Usado pelo sistema de IA ou por gestores para criar alertas personalizados. |
| `DELETE` | `/api/v1.0/AlertasIA/{id}` | Excluir alerta | Sim (PROFISSIONAL, GESTOR) | Remove um alerta de IA do sistema permanentemente. Útil para limpar alertas antigos ou que já foram resolvidos. |

### 🎯 Hábitos

| Método | Endpoint | Descrição | Autenticação | Para que serve |
|--------|----------|-----------|--------------|---------------|
| `GET` | `/api/v1.0/Habitos` | Listar hábitos (paginado) | Sim (PROFISSIONAL, GESTOR) | Retorna uma lista paginada de todos os hábitos registrados no sistema. Útil para gestores visualizarem os hábitos saudáveis praticados pela equipe. |
| `GET` | `/api/v1.0/Habitos/{id}` | Buscar hábito por ID | Sim (PROFISSIONAL, GESTOR) | Retorna um hábito específico pelo seu ID. Útil para visualizar detalhes de um registro específico de hábito. |
| `GET` | `/api/v1.0/Habitos/usuario/{usuarioId}` | Listar hábitos de um usuário | Sim (PROFISSIONAL, GESTOR) | Retorna todos os hábitos registrados por um usuário específico. Útil para profissionais visualizarem seu próprio histórico de hábitos saudáveis ou gestores visualizarem os hábitos de um profissional. |
| `POST` | `/api/v1.0/Habitos` | Criar hábito (ID do usuário vem do token) | Sim (PROFISSIONAL, GESTOR) | Cria um novo registro de hábito saudável. O ID do usuário é automaticamente obtido do token JWT. Cada hábito gera pontuação que contribui para conquista de badges. |
| `DELETE` | `/api/v1.0/Habitos/{id}` | Excluir hábito | Sim (PROFISSIONAL, GESTOR) | Remove um registro de hábito do sistema permanentemente. Use com cuidado, pois esta ação não pode ser desfeita e pode afetar a pontuação do usuário. |

### 🏆 Badges

| Método | Endpoint | Descrição | Autenticação | Para que serve |
|--------|----------|-----------|--------------|---------------|
| `GET` | `/api/v1.0/Badges` | Listar badges (paginado) | Sim (PROFISSIONAL, GESTOR) | Retorna uma lista paginada de todos os badges disponíveis no sistema. Útil para profissionais visualizarem quais badges podem conquistar. |
| `GET` | `/api/v1.0/Badges/{id}` | Buscar badge por ID | Sim (PROFISSIONAL, GESTOR) | Retorna um badge específico pelo seu ID. Útil para visualizar detalhes de um badge, incluindo pontos requeridos e descrição. |
| `GET` | `/api/v1.0/Badges/usuario/{usuarioId}` | Listar badges de um usuário | Sim (PROFISSIONAL, GESTOR) | Retorna todos os badges conquistados por um usuário específico. Útil para profissionais visualizarem suas próprias conquistas ou gestores visualizarem as conquistas de um profissional. |
| `POST` | `/api/v1.0/Badges` | Criar badge | Sim (GESTOR apenas) | Cria um novo badge no sistema. Apenas gestores podem criar badges. Define nome, descrição e pontos requeridos para conquistar o badge. |
| `PUT` | `/api/v1.0/Badges/{id}` | Atualizar badge | Sim (GESTOR apenas) | Atualiza um badge existente. Apenas gestores podem atualizar badges. Permite alterar nome, descrição e pontos requeridos. |
| `POST` | `/api/v1.0/Badges/usuario/{usuarioId}/badge/{badgeId}` | Conceder badge a usuário | Sim (GESTOR apenas) | Concede manualmente um badge a um usuário específico. Apenas gestores podem conceder badges manualmente. Útil para reconhecimento especial ou correção de bugs no sistema de pontuação. |
| `DELETE` | `/api/v1.0/Badges/{id}` | Excluir badge | Sim (GESTOR apenas) | Remove um badge do sistema permanentemente. Apenas gestores podem excluir badges. Use com cuidado, pois esta ação não pode ser desfeita. |

### 🏥 Health Checks

Todos os endpoints de health check são públicos (não requerem autenticação).

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET` | `/api/v1.0/Health` | Health check geral |
| `GET` | `/api/v1.0/Health/database` | Health check do banco |
| `GET` | `/api/v1.0/Health/memory` | Health check da memória |
| `GET` | `/health` | Health check geral (sem versão) |
| `GET` | `/health/database` | Health check do banco (sem versão) |
| `GET` | `/health/ready` | Health check ready |
| `GET` | `/health/live` | Health check live |

---

## 🧪 Testes

O projeto possui uma suíte completa de testes unitários e de integração, garantindo qualidade e confiabilidade do código.

### 📊 Estrutura de Testes

```
Tests/
├── Unit/                           # Testes unitários
│   └── JwtServiceTests.cs         # Testes do serviço JWT (7 testes)
└── Integration/                    # Testes de integração
    ├── CustomWebApplicationFactory.cs  # Factory para testes
    ├── UsuarioIntegrationTests.cs      # Testes de endpoints de usuários (7 testes)
    ├── AuthIntegrationTests.cs         # Testes de autenticação (6 testes)
    ├── HealthCheckIntegrationTests.cs  # Testes de health checks (8 testes)
    ├── HumorIntegrationTests.cs       # Testes de endpoints de humor (7 testes)
    ├── SprintsIntegrationTests.cs     # Testes de endpoints de sprints (7 testes)
    ├── AlertasIAIntegrationTests.cs   # Testes de endpoints de alertas IA (6 testes)
    ├── HabitosIntegrationTests.cs     # Testes de endpoints de hábitos (6 testes)
    └── BadgesIntegrationTests.cs      # Testes de endpoints de badges (8 testes)
```

### 📋 Tabela Completa de Todos os Testes

| # | Controller | Endpoint | Método HTTP | Nome do Teste | Status |
|---|------------|----------|-------------|---------------|--------|
| **Testes Unitários** |
| 1 | JwtService | - | - | `GenerateToken_WithValidUsuario_ShouldReturnValidToken` | ✅ |
| 2 | JwtService | - | - | `GenerateToken_WithDifferentPerfis_ShouldGenerateDifferentTokens` | ✅ |
| 3 | JwtService | - | - | `ValidateToken_WithValidToken_ShouldReturnClaimsPrincipal` | ✅ |
| 4 | JwtService | - | - | `ValidateToken_WithInvalidToken_ShouldReturnNull` | ✅ |
| 5 | JwtService | - | - | `ValidateToken_WithExpiredToken_ShouldReturnNull` | ✅ |
| 6 | JwtService | - | - | `HasRole_WithGestorUser_ShouldReturnTrueForGestorRole` | ✅ |
| 7 | JwtService | - | - | `HasRole_WithProfissionalUser_ShouldReturnFalseForGestorRole` | ✅ |
| **Testes de Integração - Autenticação** |
| 8 | Auth | `/api/v1.0/Auth/login` | POST | `Login_WithValidCredentials_ShouldReturnToken` | ✅ |
| 9 | Auth | `/api/v1.0/Auth/login` | POST | `Login_WithInvalidCredentials_ShouldReturnUnauthorized` | ✅ |
| 10 | Auth | `/api/v1.0/Auth/validate` | POST | `ValidateToken_WithValidToken_ShouldReturnValid` | ✅ |
| 11 | Auth | `/api/v1.0/Auth/validate` | POST | `ValidateToken_WithInvalidToken_ShouldReturnUnauthorized` | ✅ |
| 12 | Auth | `/api/v1.0/Auth/me` | GET | `GetUserInfo_WithValidToken_ShouldReturnUserInfo` | ✅ |
| 13 | Auth | `/api/v1.0/Auth/me` | GET | `GetUserInfo_WithoutToken_ShouldReturnUnauthorized` | ✅ |
| 14 | Auth | `/api/v1.0/Auth/check-admin` | GET | `CheckAdmin_WithValidToken_ShouldReturnOk` | ✅ |
| 15 | Auth | `/api/v1.0/Auth/check-admin` | GET | `CheckAdmin_WithoutToken_ShouldReturnOk` | ✅ |
| **Testes de Integração - Usuários** |
| 16 | Usuarios | `/api/v1.0/Usuarios` | GET | `GetUsuariosV1_WithValidToken_ShouldReturnOk` | ✅ |
| 17 | Usuarios | `/api/v1.0/Usuarios` | GET | `GetUsuariosV1_WithoutToken_ShouldReturnUnauthorized` | ✅ |
| 18 | Usuarios | `/api/v1.0/Usuarios/{id}` | GET | `GetUsuarioByIdV1_WithValidToken_ShouldReturnOkOrNotFound` | ✅ |
| 19 | Usuarios | `/api/v1.0/Usuarios` | POST | `CreateUsuarioV1_WithValidToken_ShouldReturnCreated` | ✅ |
| 20 | Usuarios | `/api/v1.0/Usuarios` | POST | `CreateUsuarioV1_WithoutToken_ShouldReturnUnauthorized` | ✅ |
| 21 | Usuarios | `/api/v1.0/Usuarios/{id}` | PUT | `UpdateUsuarioV1_WithValidToken_ShouldReturnOkOrNotFound` | ✅ |
| 22 | Usuarios | `/api/v1.0/Usuarios/{id}` | DELETE | `DeleteUsuarioV1_WithValidToken_ShouldReturnNoContentOrNotFound` | ✅ |
| **Testes de Integração - Humor** |
| 23 | Humor | `/api/v1.0/Humor` | GET | `GetHumores_WithValidToken_ShouldReturnOk` | ✅ |
| 24 | Humor | `/api/v1.0/Humor` | GET | `GetHumores_WithoutToken_ShouldReturnUnauthorized` | ✅ |
| 25 | Humor | `/api/v1.0/Humor/{id}` | GET | `GetHumorById_WithValidToken_ShouldReturnOkOrNotFound` | ✅ |
| 26 | Humor | `/api/v1.0/Humor/usuario/{usuarioId}` | GET | `GetHumorByUsuario_WithValidToken_ShouldReturnOk` | ✅ |
| 27 | Humor | `/api/v1.0/Humor` | POST | `CreateHumor_WithValidToken_ShouldReturnCreated` | ✅ |
| 28 | Humor | `/api/v1.0/Humor/{id}` | PUT | `UpdateHumor_WithValidToken_ShouldReturnOkOrNotFound` | ✅ |
| 29 | Humor | `/api/v1.0/Humor/{id}` | DELETE | `DeleteHumor_WithValidToken_ShouldReturnNoContentOrNotFound` | ✅ |
| **Testes de Integração - Sprints** |
| 30 | Sprints | `/api/v1.0/Sprints` | GET | `GetSprints_WithValidToken_ShouldReturnOk` | ✅ |
| 31 | Sprints | `/api/v1.0/Sprints` | GET | `GetSprints_WithoutToken_ShouldReturnUnauthorized` | ✅ |
| 32 | Sprints | `/api/v1.0/Sprints/{id}` | GET | `GetSprintById_WithValidToken_ShouldReturnOkOrNotFound` | ✅ |
| 33 | Sprints | `/api/v1.0/Sprints/usuario/{usuarioId}` | GET | `GetSprintsByUsuario_WithValidToken_ShouldReturnOk` | ✅ |
| 34 | Sprints | `/api/v1.0/Sprints` | POST | `CreateSprint_WithValidToken_ShouldReturnCreated` | ✅ |
| 35 | Sprints | `/api/v1.0/Sprints/{id}` | PUT | `UpdateSprint_WithValidToken_ShouldReturnOkOrNotFound` | ✅ |
| 36 | Sprints | `/api/v1.0/Sprints/{id}` | DELETE | `DeleteSprint_WithValidToken_ShouldReturnNoContentOrNotFound` | ✅ |
| **Testes de Integração - Alertas IA** |
| 37 | AlertasIA | `/api/v1.0/AlertasIA` | GET | `GetAlertasIA_WithValidToken_ShouldReturnOk` | ✅ |
| 38 | AlertasIA | `/api/v1.0/AlertasIA` | GET | `GetAlertasIA_WithoutToken_ShouldReturnUnauthorized` | ✅ |
| 39 | AlertasIA | `/api/v1.0/AlertasIA/{id}` | GET | `GetAlertaIAById_WithValidToken_ShouldReturnOkOrNotFound` | ✅ |
| 40 | AlertasIA | `/api/v1.0/AlertasIA/usuario/{usuarioId}` | GET | `GetAlertasIAByUsuario_WithValidToken_ShouldReturnOk` | ✅ |
| 41 | AlertasIA | `/api/v1.0/AlertasIA` | POST | `CreateAlertaIA_WithValidToken_ShouldReturnCreated` | ✅ |
| 42 | AlertasIA | `/api/v1.0/AlertasIA/{id}` | DELETE | `DeleteAlertaIA_WithValidToken_ShouldReturnNoContentOrNotFound` | ✅ |
| **Testes de Integração - Hábitos** |
| 43 | Habitos | `/api/v1.0/Habitos` | GET | `GetHabitos_WithValidToken_ShouldReturnOk` | ✅ |
| 44 | Habitos | `/api/v1.0/Habitos` | GET | `GetHabitos_WithoutToken_ShouldReturnUnauthorized` | ✅ |
| 45 | Habitos | `/api/v1.0/Habitos/{id}` | GET | `GetHabitoById_WithValidToken_ShouldReturnOkOrNotFound` | ✅ |
| 46 | Habitos | `/api/v1.0/Habitos/usuario/{usuarioId}` | GET | `GetHabitosByUsuario_WithValidToken_ShouldReturnOk` | ✅ |
| 47 | Habitos | `/api/v1.0/Habitos` | POST | `CreateHabito_WithValidToken_ShouldReturnCreated` | ✅ |
| 48 | Habitos | `/api/v1.0/Habitos/{id}` | DELETE | `DeleteHabito_WithValidToken_ShouldReturnNoContentOrNotFound` | ✅ |
| **Testes de Integração - Badges** |
| 49 | Badges | `/api/v1.0/Badges` | GET | `GetBadges_WithValidToken_ShouldReturnOk` | ✅ |
| 50 | Badges | `/api/v1.0/Badges` | GET | `GetBadges_WithoutToken_ShouldReturnUnauthorized` | ✅ |
| 51 | Badges | `/api/v1.0/Badges/{id}` | GET | `GetBadgeById_WithValidToken_ShouldReturnOkOrNotFound` | ✅ |
| 52 | Badges | `/api/v1.0/Badges/usuario/{usuarioId}` | GET | `GetBadgesByUsuario_WithValidToken_ShouldReturnOk` | ✅ |
| 53 | Badges | `/api/v1.0/Badges` | POST | `CreateBadge_WithGestorToken_ShouldReturnCreated` | ✅ |
| 54 | Badges | `/api/v1.0/Badges/{id}` | PUT | `UpdateBadge_WithGestorToken_ShouldReturnOkOrNotFound` | ✅ |
| 55 | Badges | `/api/v1.0/Badges/usuario/{usuarioId}/badge/{badgeId}` | POST | `ConcederBadge_WithValidToken_ShouldReturnCreated` | ✅ |
| 56 | Badges | `/api/v1.0/Badges/{id}` | DELETE | `DeleteBadge_WithGestorToken_ShouldReturnNoContentOrNotFound` | ✅ |
| **Testes de Integração - Health Checks** |
| 57 | Health | `/health` | GET | `HealthEndpoint_ShouldReturnOk` | ✅ |
| 58 | Health | `/health/ready` | GET | `HealthReadyEndpoint_ShouldReturnOk` | ✅ |
| 59 | Health | `/health/live` | GET | `HealthLiveEndpoint_ShouldReturnOk` | ✅ |
| 60 | Health | `/health/database` | GET | `HealthDatabaseEndpoint_ShouldReturnOk` | ✅ |
| 61 | Health | `/api/v1.0/Health` | GET | `HealthV1Endpoint_ShouldReturnOk` | ✅ |
| 62 | Health | `/api/v1.0/Health/database` | GET | `HealthV1DatabaseEndpoint_ShouldReturnOk` | ✅ |
| 63 | Health | `/api/v1.0/Health/memory` | GET | `HealthV1MemoryEndpoint_ShouldReturnOk` | ✅ |

**Total: 63 testes (7 unitários + 56 de integração)**

### 🧪 Testes Unitários

Os testes unitários testam componentes individuais isoladamente usando mocks e bancos de dados em memória.

#### Executar Testes Unitários

```bash
# Executar todos os testes unitários
dotnet test --filter "FullyQualifiedName~Unit"

# Executar testes específicos
dotnet test --filter "JwtServiceTests"

# Executar com output detalhado
dotnet test --filter "FullyQualifiedName~Unit" --logger "console;verbosity=detailed"
```

#### Testes Unitários Disponíveis

##### ✅ `JwtServiceTests` - Testes do Serviço JWT

**Localização**: `Tests/Unit/JwtServiceTests.cs`

**Testes implementados**:

1. **`GenerateToken_WithValidUsuario_ShouldReturnValidToken`**
   - Verifica se um token JWT válido é gerado para um usuário válido
   - Valida formato do token (deve conter pontos separadores)

2. **`GenerateToken_WithDifferentPerfis_ShouldGenerateDifferentTokens`**
   - Verifica se tokens diferentes são gerados para perfis diferentes (GESTOR e PROFISSIONAL)
   - Garante que cada perfil tem seu próprio token

3. **`ValidateToken_WithValidToken_ShouldReturnClaimsPrincipal`**
   - Valida se um token válido retorna um ClaimsPrincipal correto
   - Verifica claims: NameIdentifier, Name, Email
   - Confirma que o usuário está autenticado

4. **`ValidateToken_WithInvalidToken_ShouldReturnNull`**
   - Verifica se um token inválido retorna null
   - Testa tratamento de erros

5. **`ValidateToken_WithExpiredToken_ShouldReturnNull`**
   - Verifica se um token expirado retorna null
   - Testa validação de expiração

6. **`HasRole_WithGestorUser_ShouldReturnTrueForGestorRole`**
   - Verifica se um usuário GESTOR tem a role correta
   - Testa métodos: `HasRole`, `IsGestor`, `IsGestorOrProfissional`

7. **`HasRole_WithProfissionalUser_ShouldReturnFalseForGestorRole`**
   - Verifica se um usuário PROFISSIONAL não tem role de GESTOR
   - Testa separação de permissões

**Cobertura**: 
- ✅ Geração de tokens
- ✅ Validação de tokens
- ✅ Claims e roles
- ✅ Tratamento de erros
- ✅ Expiração de tokens

### 🔍 Testes de Integração

Os testes de integração usam `WebApplicationFactory` para testar a aplicação completa em um ambiente de teste real com banco de dados em memória.

#### Executar Testes de Integração

```bash
# Executar todos os testes de integração
dotnet test --filter "FullyQualifiedName~Integration"

# Executar testes específicos
dotnet test --filter "UsuarioIntegrationTests"
dotnet test --filter "AuthIntegrationTests"
dotnet test --filter "HealthCheckIntegrationTests"
dotnet test --filter "HumorIntegrationTests"
dotnet test --filter "SprintsIntegrationTests"
dotnet test --filter "AlertasIAIntegrationTests"
dotnet test --filter "HabitosIntegrationTests"
dotnet test --filter "BadgesIntegrationTests"

# Executar com output detalhado
dotnet test --filter "FullyQualifiedName~Integration" --logger "console;verbosity=detailed"
```

#### Testes de Integração Disponíveis

##### ✅ `UsuarioIntegrationTests` - Testes de Endpoints de Usuários

**Localização**: `Tests/Integration/UsuarioIntegrationTests.cs`

**Testes implementados**:

1. **`GetUsuariosV1_WithValidToken_ShouldReturnOk`**
   - Testa GET `/api/v1.0/Usuarios` com token válido
   - Verifica status 200 OK
   - Valida paginação

2. **`GetUsuariosV1_WithoutToken_ShouldReturnUnauthorized`**
   - Testa GET `/api/v1.0/Usuarios` sem token
   - Verifica status 401 Unauthorized
   - Confirma que autenticação é obrigatória

3. **`GetUsuarioByIdV1_WithValidToken_ShouldReturnOkOrNotFound`**
   - Testa GET `/api/v1.0/Usuarios/{id}` com token válido
   - Aceita 200 OK ou 404 Not Found

4. **`CreateUsuarioV1_WithValidToken_ShouldReturnCreated`**
   - Testa POST `/api/v1.0/Usuarios` com token válido
   - Verifica criação de usuário
   - Valida DTOs do novo modelo (nome, email, perfil, empresa)

5. **`CreateUsuarioV1_WithoutToken_ShouldReturnUnauthorized`**
   - Testa POST `/api/v1.0/Usuarios` sem token
   - Verifica status 401 Unauthorized

6. **`UpdateUsuarioV1_WithValidToken_ShouldReturnOkOrNotFound`**
   - Testa PUT `/api/v1.0/Usuarios/{id}` com token válido
   - Aceita 200 OK, 404 Not Found, 409 Conflict ou 400 Bad Request

7. **`DeleteUsuarioV1_WithValidToken_ShouldReturnNoContentOrNotFound`**
   - Testa DELETE `/api/v1.0/Usuarios/{id}` com token válido
   - Aceita 204 No Content ou 404 Not Found

**Cobertura**:
- ✅ CRUD completo de usuários
- ✅ Autenticação JWT
- ✅ Paginação
- ✅ Validação de DTOs
- ✅ Tratamento de erros

##### ✅ `AuthIntegrationTests` - Testes de Autenticação

**Localização**: `Tests/Integration/AuthIntegrationTests.cs`

**Testes implementados**:

1. **`Login_WithValidCredentials_ShouldReturnToken`**
   - Testa POST `/api/v1.0/Auth/login` com credenciais válidas
   - Verifica status 200 OK
   - Valida presença do token na resposta

2. **`Login_WithInvalidCredentials_ShouldReturnUnauthorized`**
   - Testa POST `/api/v1.0/Auth/login` com credenciais inválidas
   - Verifica status 401 Unauthorized
   - Testa segurança do login

3. **`ValidateToken_WithValidToken_ShouldReturnOk`**
   - Testa POST `/api/v1.0/Auth/validate` com token válido
   - Verifica validação de token

4. **`ValidateToken_WithInvalidToken_ShouldReturnUnauthorized`**
   - Testa POST `/api/v1.0/Auth/validate` com token inválido
   - Verifica tratamento de token inválido

5. **`GetMe_WithValidToken_ShouldReturnUserInfo`**
   - Testa GET `/api/v1.0/Auth/me` com token válido
   - Verifica informações do usuário autenticado

6. **`GetMe_WithoutToken_ShouldReturnUnauthorized`**
   - Testa GET `/api/v1.0/Auth/me` sem token
   - Verifica status 401 Unauthorized

**Cobertura**:
- ✅ Login com credenciais válidas/inválidas
- ✅ Validação de tokens
- ✅ Obtenção de informações do usuário
- ✅ Tratamento de erros de autenticação

##### ✅ `HealthCheckIntegrationTests` - Testes de Health Checks

**Localização**: `Tests/Integration/HealthCheckIntegrationTests.cs`

**Testes implementados**:

1. **`HealthEndpoint_ShouldReturnOk`**
   - Testa GET `/health`
   - Verifica status 200 OK
   - Health check geral

2. **`HealthReadyEndpoint_ShouldReturnOk`**
   - Testa GET `/health/ready`
   - Verifica status 200 OK
   - Health check de prontidão

3. **`HealthLiveEndpoint_ShouldReturnOk`**
   - Testa GET `/health/live`
   - Verifica status 200 OK
   - Health check de vida

4. **`HealthDatabaseEndpoint_ShouldReturnOk`**
   - Testa GET `/health/database`
   - Verifica status 200 OK
   - Health check do banco de dados

5. **`HealthV1Endpoint_ShouldReturnOk`**
   - Testa GET `/api/v1.0/Health`
   - Verifica status 200 OK ou 503 Service Unavailable
   - Health check geral (versão 1.0)

6. **`HealthV1DatabaseEndpoint_ShouldReturnOk`**
   - Testa GET `/api/v1.0/Health/database`
   - Verifica status 200 OK ou 503 Service Unavailable
   - Health check do banco (versão 1.0)

7. **`HealthV1MemoryEndpoint_ShouldReturnOk`**
   - Testa GET `/api/v1.0/Health/memory`
   - Verifica status 200 OK ou 503 Service Unavailable
   - Health check da memória (versão 1.0)

**Cobertura**:
- ✅ Todos os endpoints de health check
- ✅ Verificação de disponibilidade
- ✅ Monitoramento de recursos
- ✅ Health checks versionados e não versionados

### 📊 Executar Todos os Testes

```bash
# Executar todos os testes (unitários + integração)
dotnet test

# Executar com cobertura de código
dotnet test --collect:"XPlat Code Coverage"

# Executar com output detalhado
dotnet test --logger "console;verbosity=detailed"

# Executar testes em paralelo (padrão)
dotnet test --parallel

# Executar testes sequencialmente
dotnet test --no-parallel
```

### 📈 Relatório de Cobertura

```bash
# Gerar relatório de cobertura completo
dotnet test --collect:"XPlat Code Coverage" --results-directory ./TestResults

# Gerar relatório com detalhes
dotnet test --collect:"XPlat Code Coverage" --settings:coverlet.runsettings --results-directory ./TestResults
```

##### ✅ `HumorIntegrationTests` - Testes de Endpoints de Humor

**Localização**: `Tests/Integration/HumorIntegrationTests.cs`

**Testes implementados**:

1. **`GetHumores_WithValidToken_ShouldReturnOk`**
   - Testa GET `/api/v1.0/Humor` com token válido
   - Verifica status 200 OK e paginação

2. **`GetHumores_WithoutToken_ShouldReturnUnauthorized`**
   - Testa GET `/api/v1.0/Humor` sem token
   - Verifica status 401 Unauthorized

3. **`GetHumorById_WithValidToken_ShouldReturnOkOrNotFound`**
   - Testa GET `/api/v1.0/Humor/{id}` com token válido
   - Aceita 200 OK ou 404 Not Found

4. **`GetHumorByUsuario_WithValidToken_ShouldReturnOk`**
   - Testa GET `/api/v1.0/Humor/usuario/{usuarioId}` com token válido
   - Verifica listagem por usuário

5. **`CreateHumor_WithValidToken_ShouldReturnCreated`**
   - Testa POST `/api/v1.0/Humor` com token válido
   - Valida criação de registro de humor

6. **`UpdateHumor_WithValidToken_ShouldReturnOkOrNotFound`**
   - Testa PUT `/api/v1.0/Humor/{id}` com token válido
   - Aceita 200 OK, 404 Not Found ou 400 Bad Request

7. **`DeleteHumor_WithValidToken_ShouldReturnNoContentOrNotFound`**
   - Testa DELETE `/api/v1.0/Humor/{id}` com token válido
   - Aceita 204 No Content ou 404 Not Found

**Cobertura**:
- ✅ CRUD completo de humor
- ✅ Autenticação JWT
- ✅ Listagem por usuário
- ✅ Validação de DTOs

##### ✅ `SprintsIntegrationTests` - Testes de Endpoints de Sprints

**Localização**: `Tests/Integration/SprintsIntegrationTests.cs`

**Testes implementados**:

1. **`GetSprints_WithValidToken_ShouldReturnOk`**
   - Testa GET `/api/v1.0/Sprints` com token válido
   - Verifica status 200 OK e paginação

2. **`GetSprints_WithoutToken_ShouldReturnUnauthorized`**
   - Testa GET `/api/v1.0/Sprints` sem token
   - Verifica status 401 Unauthorized

3. **`GetSprintById_WithValidToken_ShouldReturnOkOrNotFound`**
   - Testa GET `/api/v1.0/Sprints/{id}` com token válido
   - Aceita 200 OK ou 404 Not Found

4. **`GetSprintsByUsuario_WithValidToken_ShouldReturnOk`**
   - Testa GET `/api/v1.0/Sprints/usuario/{usuarioId}` com token válido
   - Verifica listagem por usuário

5. **`CreateSprint_WithValidToken_ShouldReturnCreated`**
   - Testa POST `/api/v1.0/Sprints` com token válido
   - Valida criação de sprint com dados completos

6. **`UpdateSprint_WithValidToken_ShouldReturnOkOrNotFound`**
   - Testa PUT `/api/v1.0/Sprints/{id}` com token válido
   - Aceita 200 OK, 404 Not Found, 400 Bad Request ou 409 Conflict

7. **`DeleteSprint_WithValidToken_ShouldReturnNoContentOrNotFound`**
   - Testa DELETE `/api/v1.0/Sprints/{id}` com token válido
   - Aceita 204 No Content ou 404 Not Found

**Cobertura**:
- ✅ CRUD completo de sprints
- ✅ Autenticação JWT
- ✅ Validação de dados (produtividade, tarefas, commits)
- ✅ Tratamento de conflitos

##### ✅ `AlertasIAIntegrationTests` - Testes de Endpoints de Alertas de IA

**Localização**: `Tests/Integration/AlertasIAIntegrationTests.cs`

**Testes implementados**:

1. **`GetAlertasIA_WithValidToken_ShouldReturnOk`**
   - Testa GET `/api/v1.0/AlertasIA` com token válido
   - Verifica status 200 OK e paginação

2. **`GetAlertasIA_WithoutToken_ShouldReturnUnauthorized`**
   - Testa GET `/api/v1.0/AlertasIA` sem token
   - Verifica status 401 Unauthorized

3. **`GetAlertaIAById_WithValidToken_ShouldReturnOkOrNotFound`**
   - Testa GET `/api/v1.0/AlertasIA/{id}` com token válido
   - Aceita 200 OK ou 404 Not Found

4. **`GetAlertasIAByUsuario_WithValidToken_ShouldReturnOk`**
   - Testa GET `/api/v1.0/AlertasIA/usuario/{usuarioId}` com token válido
   - Verifica listagem por usuário

5. **`CreateAlertaIA_WithValidToken_ShouldReturnCreated`**
   - Testa POST `/api/v1.0/AlertasIA` com token válido
   - Valida criação de alerta (tipo, mensagem, nível de risco)

6. **`DeleteAlertaIA_WithValidToken_ShouldReturnNoContentOrNotFound`**
   - Testa DELETE `/api/v1.0/AlertasIA/{id}` com token válido
   - Aceita 204 No Content ou 404 Not Found

**Cobertura**:
- ✅ CRUD de alertas (sem atualização)
- ✅ Autenticação JWT
- ✅ Validação de nível de risco
- ✅ Listagem por usuário

##### ✅ `HabitosIntegrationTests` - Testes de Endpoints de Hábitos

**Localização**: `Tests/Integration/HabitosIntegrationTests.cs`

**Testes implementados**:

1. **`GetHabitos_WithValidToken_ShouldReturnOk`**
   - Testa GET `/api/v1.0/Habitos` com token válido
   - Verifica status 200 OK e paginação

2. **`GetHabitos_WithoutToken_ShouldReturnUnauthorized`**
   - Testa GET `/api/v1.0/Habitos` sem token
   - Verifica status 401 Unauthorized

3. **`GetHabitoById_WithValidToken_ShouldReturnOkOrNotFound`**
   - Testa GET `/api/v1.0/Habitos/{id}` com token válido
   - Aceita 200 OK ou 404 Not Found

4. **`GetHabitosByUsuario_WithValidToken_ShouldReturnOk`**
   - Testa GET `/api/v1.0/Habitos/usuario/{usuarioId}` com token válido
   - Verifica listagem por usuário

5. **`CreateHabito_WithValidToken_ShouldReturnCreated`**
   - Testa POST `/api/v1.0/Habitos` com token válido
   - Valida criação de hábito (tipo, data, pontuação)

6. **`DeleteHabito_WithValidToken_ShouldReturnNoContentOrNotFound`**
   - Testa DELETE `/api/v1.0/Habitos/{id}` com token válido
   - Aceita 204 No Content ou 404 Not Found

**Cobertura**:
- ✅ CRUD de hábitos (sem atualização)
- ✅ Autenticação JWT
- ✅ Validação de pontuação
- ✅ Listagem por usuário

##### ✅ `BadgesIntegrationTests` - Testes de Endpoints de Badges

**Localização**: `Tests/Integration/BadgesIntegrationTests.cs`

**Testes implementados**:

1. **`GetBadges_WithValidToken_ShouldReturnOk`**
   - Testa GET `/api/v1.0/Badges` com token válido
   - Verifica status 200 OK e paginação

2. **`GetBadges_WithoutToken_ShouldReturnUnauthorized`**
   - Testa GET `/api/v1.0/Badges` sem token
   - Verifica status 401 Unauthorized

3. **`GetBadgeById_WithValidToken_ShouldReturnOkOrNotFound`**
   - Testa GET `/api/v1.0/Badges/{id}` com token válido
   - Aceita 200 OK ou 404 Not Found

4. **`GetBadgesByUsuario_WithValidToken_ShouldReturnOk`**
   - Testa GET `/api/v1.0/Badges/usuario/{usuarioId}` com token válido
   - Verifica listagem de badges conquistados por usuário

5. **`CreateBadge_WithGestorToken_ShouldReturnCreated`**
   - Testa POST `/api/v1.0/Badges` com token de GESTOR
   - Verifica que apenas GESTOR pode criar badges
   - Aceita 201 Created, 400 Bad Request ou 403 Forbidden

6. **`UpdateBadge_WithGestorToken_ShouldReturnOkOrNotFound`**
   - Testa PUT `/api/v1.0/Badges/{id}` com token de GESTOR
   - Verifica que apenas GESTOR pode atualizar badges
   - Aceita 200 OK, 404 Not Found, 400 Bad Request ou 403 Forbidden

7. **`ConcederBadge_WithValidToken_ShouldReturnCreated`**
   - Testa POST `/api/v1.0/Badges/usuario/{usuarioId}/badge/{badgeId}`
   - Verifica concessão de badge a usuário
   - Aceita 201 Created, 404 Not Found ou 409 Conflict

8. **`DeleteBadge_WithGestorToken_ShouldReturnNoContentOrNotFound`**
   - Testa DELETE `/api/v1.0/Badges/{id}` com token de GESTOR
   - Verifica que apenas GESTOR pode excluir badges
   - Aceita 204 No Content, 404 Not Found ou 403 Forbidden

**Cobertura**:
- ✅ CRUD completo de badges
- ✅ Autenticação JWT
- ✅ Autorização baseada em roles (GESTOR)
- ✅ Concessão de badges
- ✅ Validação de permissões

### 🎯 Resumo da Cobertura de Testes

| Categoria | Testes | Cobertura |
|-----------|--------|-----------|
| **Unitários** | 7 testes | JwtService (100%) |
| **Integração - Usuários** | 7 testes | CRUD completo |
| **Integração - Autenticação** | 6 testes | Login, validação, user info |
| **Integração - Health Checks** | 8 testes | Todos os endpoints |
| **Integração - Humor** | 7 testes | CRUD completo |
| **Integração - Sprints** | 7 testes | CRUD completo |
| **Integração - Alertas IA** | 6 testes | CRUD (sem atualização) |
| **Integração - Hábitos** | 6 testes | CRUD (sem atualização) |
| **Integração - Badges** | 8 testes | CRUD completo + permissões |
| **Total** | **63 testes** | **100% dos endpoints principais** |

---

## 📊 Versionamento da API

A API utiliza versionamento por URL:
- **v1.0**: Versão atual (anteriormente v2.0, transformada em v1.0)

Todas as rotas seguem o padrão: `/api/v1.0/{controller}`

### Estratégia de Versionamento

- **Versionamento por URL**: `/api/v1.0/`
- **Swagger**: Documentação separada por versão
- **Backward Compatibility**: Mantida entre versões
- **Deprecation**: Versões antigas são mantidas até migração completa

---

## 🔒 Segurança

### Autenticação JWT

- **Autenticação JWT** obrigatória para a maioria dos endpoints
- **Roles**: PROFISSIONAL e GESTOR
- **Hash de senhas** com BCrypt
- **Validação de tokens** com expiração configurável (60 minutos)
- **Claims personalizados**: NameIdentifier, Name, Email, Perfil, Empresa

### Endpoints Públicos (sem autenticação)

- `POST /api/v1.0/Auth/login` - Login
- `POST /api/v1.0/Usuarios` - Criar usuário (registro)
- `GET /health/*` - Health checks

### Endpoints Protegidos

Todos os outros endpoints requerem token JWT válido no header:
```
Authorization: Bearer {seu_token_jwt}
```

### Configuração JWT

  ```json
  {
  "JwtSettings": {
    "SecretKey": "MindTrack_Super_Secret_Key_2024_Advanced_Business_Development_With_DotNet",
    "Issuer": "MindTrackAPI",
    "Audience": "MindTrackUsers",
    "ExpiryMinutes": 60
  }
}
```

---

## 📝 Scripts SQL

O arquivo `create-mindtrack-tables.sql` contém o script completo para criação das tabelas no Oracle Database.

### Executar Script SQL

```sql
-- Execute o arquivo create-mindtrack-tables.sql no Oracle SQL Developer
-- ou via linha de comando:
sqlplus rm555241/230205@oracle.fiap.com.br:1521/ORCL @create-mindtrack-tables.sql
```

---

## 🎯 Próximos Passos

- [ ] Implementar serviço de ML para análise de bem-estar
- [ ] Adicionar endpoints de relatórios e dashboards
- [ ] Implementar sistema de pontuação e ranking
- [ ] Adicionar notificações push
- [ ] Desenvolver aplicativo mobile
- [ ] Adicionar testes de carga e performance
- [ ] Implementar cache para melhor performance
- [ ] Adicionar documentação OpenAPI mais detalhada

---

## 📄 Licença

Este projeto foi desenvolvido para fins acadêmicos no contexto do curso **ADVANCED BUSINESS DEVELOPMENT WITH .NET** da FIAP.

---

## 👨‍💻 Desenvolvido com ❤️ pela equipe MindTrack

Para mais informações, entre em contato através do email: dev@fiap.com

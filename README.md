# 🎸 BandHub — BandService

<p align="center">
  <strong>Microsserviço de gerenciamento de bandas do BandHub</strong>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 8" />
  <img src="https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white" alt="PostgreSQL" />
  <img src="https://img.shields.io/badge/EF_Core-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt="EF Core" />
  <img src="https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black" alt="Swagger" />
  <img src="https://img.shields.io/badge/xUnit-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt="xUnit" />
</p>

---

## 📋 Índice

- [Sobre o Serviço](#-sobre-o-serviço)
- [Arquitetura](#-arquitetura)
- [Tecnologias](#-tecnologias)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Pré-requisitos](#-pré-requisitos)
- [Configuração e Instalação](#-configuração-e-instalação)
- [Banco de Dados e Migrations](#-banco-de-dados-e-migrations)
- [Executando a Aplicação](#-executando-a-aplicação)
- [Testes](#-testes)
- [Endpoints da API](#-endpoints-da-api)
- [Contribuindo](#-contribuindo)

---

## 📖 Sobre o Serviço

O **BandService** é o microsserviço responsável pelo gerenciamento de bandas do BandHub. Ele provê funcionalidades de criação e listagem de bandas vinculadas a contas, com seu próprio banco de dados isolado.

> 🔐 **Autenticação:** os endpoints protegidos exigem um JWT válido emitido pelo [BandHub.AuthService](https://github.com/bandhub-br/bandhub-authservice-dotnet). Obtenha o token via `POST /auth/login`.

| Porta | Banco de Dados | Descrição |
|-------|----------------|-----------|
| `5081` | `bands_db` | Gerenciamento de bandas (vinculadas a contas) |

---

## 🏗 Arquitetura

O projeto segue a arquitetura **Vertical Slice Architecture**, onde cada feature é organizada em sua própria pasta contendo todos os componentes necessários (endpoint, handler, request, response e validator).

```
Feature/
├── Endpoint.cs      → Define a rota HTTP (Minimal API)
├── Handler.cs       → Lógica de negócio
├── Request.cs       → Contrato de entrada
├── Response.cs      → Contrato de saída
└── Validator.cs     → Validação de entrada
```

### Princípios aplicados

- **Vertical Slice Architecture** — cada feature isolada com seus próprios componentes
- **Minimal APIs** — endpoints leves e performáticos
- **Repository Pattern** — abstração do acesso a dados
- **Dependency Injection** — inversão de dependência nativa do .NET
- **Database per Service** — banco de dados exclusivo deste microsserviço

---

## 🛠 Tecnologias

| Tecnologia | Versão | Uso |
|------------|--------|-----|
| .NET | 8.0 | Framework principal |
| ASP.NET Core | 8.0 | Web API com Minimal APIs |
| Entity Framework Core | 8.0.24 | ORM / Acesso a dados |
| Npgsql | 8.0.11 | Provider PostgreSQL para EF Core |
| PostgreSQL | — | Banco de dados relacional |
| JwtBearer | 8.0.28 | Validação de tokens JWT emitidos pelo AuthService |
| Swagger / Swashbuckle | 6.6.2 | Documentação da API |
| xUnit | 2.5.3 | Framework de testes |
| Moq | 4.20.72 | Mocking para testes unitários |
| FluentAssertions | 8.8.0 | Assertions expressivas |

---

## 📁 Estrutura do Projeto

```
BandHub.BandService/
│
├── BandHub.BandService/                    # Projeto principal
│   ├── Features/
│   │   └── Bands/
│   │       ├── CreateBand/
│   │       │   ├── CreateBandEndpoint.cs
│   │       │   ├── CreateBandHandler.cs
│   │       │   ├── CreateBandRequest.cs
│   │       │   ├── CreateBandResponse.cs
│   │       │   └── CreateBandValidator.cs
│   │       ├── GetBands/
│   │       │   ├── GetBandsEndpoint.cs
│   │       │   ├── GetBandsHandler.cs
│   │       │   └── GetBandsResponse.cs
│   │       └── Domain/
│   │           ├── Band.cs
│   │           └── IBandRepository.cs
│   ├── Infrastructure/
│   │   └── Persistence/
│   │       ├── BandDbContext.cs
│   │       └── BandRepository.cs
│   ├── Common/
│   │   └── DependencyInjection.cs
│   ├── Migrations/
│   ├── Program.cs
│   ├── appsettings.json
│   └── BandHub.BandService.csproj
│
├── tests/
│   └── BandHub.BandService.UnitTests/
│       └── Features/
│           └── Bands/
│               ├── CreateBand/
│               │   ├── CreateBandHandlerTests.cs
│               │   ├── CreateBandHandlerSpotifyTests.cs
│               │   └── CreateBandValidatorTests.cs
│               └── GetBands/
│                   └── GetBandsHandlerTests.cs
│
├── BandHub.BandService.sln
└── README.md
```

---

## ✅ Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- [**.NET 8 SDK**](https://dotnet.microsoft.com/download/dotnet/8.0)
- [**Docker**](https://www.docker.com/products/docker-desktop/) (para rodar o PostgreSQL)
- [**EF Core CLI**](#instalando-o-ef-core-cli) (para gerenciar migrations)

### Instalando o EF Core CLI

```bash
dotnet tool install --global dotnet-ef
```

Para verificar se já está instalado:

```bash
dotnet ef --version
```

> 💡 **Dica:** Se já tiver uma versão antiga, atualize com:
> ```bash
> dotnet tool update --global dotnet-ef
> ```

---

## ⚙ Configuração e Instalação

### 1. Clone o repositório

```bash
git clone https://github.com/bandhub-br/bandhub-bandservice.git
cd bandhub-bandservice
```

### 2. Restaure as dependências

```bash
dotnet restore
```

### 3. Configure o PostgreSQL via Docker

> ⚠️ O banco de dados é sempre executado via **Docker**. A configuração da infraestrutura (Docker Compose, containers, etc.) está no repositório de infra.

Certifique-se de que o container do PostgreSQL está rodando e acessível na porta `5432` antes de prosseguir.

### 4. Verifique a connection string

A connection string está no arquivo `appsettings.json`:

**`BandHub.BandService/appsettings.json`**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=bands_db;Username=bandhub;Password=bandhub"
  }
}
```

---

## 🗄 Banco de Dados e Migrations

O projeto utiliza **Entity Framework Core** com **Code-First Migrations** para gerenciar o schema do banco de dados.

### Conceitos importantes

| Conceito | Descrição |
|----------|-----------|
| **Migration** | Um arquivo C# que descreve alterações no schema do banco de dados |
| **DbContext** | Classe que representa a sessão com o banco de dados |
| **Snapshot** | Arquivo que guarda o estado atual do modelo para comparação |

### Comandos de Migration

> ⚠️ **Importante:** Todos os comandos devem ser executados na **raiz da solution** (onde está o arquivo `.sln`).

#### 📌 Criar uma nova migration

```bash
dotnet ef migrations add NomeDaMigration --project .\BandHub.BandService\BandHub.BandService.csproj --startup-project .\BandHub.BandService\BandHub.BandService.csproj
```

> **Exemplo prático:** Imagine que você adicionou uma nova propriedade `City` na classe `Band`:
> ```bash
> dotnet ef migrations add AddCityToBand --project .\BandHub.BandService\BandHub.BandService.csproj --startup-project .\BandHub.BandService\BandHub.BandService.csproj
> ```
> Isso vai gerar um arquivo em `Migrations/` com as instruções de `Up()` e `Down()`.

#### 📌 Aplicar migrations no banco de dados

```bash
dotnet ef database update --project .\BandHub.BandService\BandHub.BandService.csproj --startup-project .\BandHub.BandService\BandHub.BandService.csproj
```

#### 📌 Remover a última migration (se ainda não foi aplicada)

```bash
dotnet ef migrations remove --project .\BandHub.BandService\BandHub.BandService.csproj --startup-project .\BandHub.BandService\BandHub.BandService.csproj
```

#### 📌 Listar todas as migrations

```bash
dotnet ef migrations list --project .\BandHub.BandService\BandHub.BandService.csproj --startup-project .\BandHub.BandService\BandHub.BandService.csproj
```

#### 📌 Gerar script SQL (para ambientes de produção)

```bash
dotnet ef migrations script --project .\BandHub.BandService\BandHub.BandService.csproj --startup-project .\BandHub.BandService\BandHub.BandService.csproj -o script.sql
```

### Fluxo completo de uma migration

```
1. Altere a entidade (Domain)
       ↓
2. Crie a migration
   $ dotnet ef migrations add AlteracaoFeita \
       --project .\BandHub.BandService\BandHub.BandService.csproj \
       --startup-project .\BandHub.BandService\BandHub.BandService.csproj
       ↓
3. Revise o arquivo gerado em Migrations/
       ↓
4. Aplique no banco
   $ dotnet ef database update \
       --project .\BandHub.BandService\BandHub.BandService.csproj \
       --startup-project .\BandHub.BandService\BandHub.BandService.csproj
       ↓
5. Teste a aplicação ✅
```

---

## 🚀 Executando a Aplicação

```bash
# BandService (porta 5081)
dotnet run --project BandHub.BandService
```

### Acessar o Swagger

Após iniciar o serviço, acesse a documentação interativa:

| URL |
|-----|
| http://localhost:5081/swagger |

### Build do projeto

```bash
dotnet build
```

---

## 🧪 Testes

O projeto utiliza **xUnit** como framework de testes, **Moq** para mocking e **FluentAssertions** para assertions expressivas.

### Executar todos os testes

```bash
dotnet test
```

### Executar testes do projeto de testes

```bash
dotnet test tests/BandHub.BandService.UnitTests
```

### Executar com output detalhado

```bash
dotnet test --verbosity detailed
```

### Executar com cobertura de código

```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Estrutura dos testes

```
tests/
└── BandHub.BandService.UnitTests/
    └── Features/Bands/
        ├── CreateBand/
        │   ├── CreateBandHandlerTests.cs        → Testa lógica de criação
        │   ├── CreateBandHandlerSpotifyTests.cs → Testa integração com Spotify
        │   └── CreateBandValidatorTests.cs      → Testa validações de entrada
        └── GetBands/
            └── GetBandsHandlerTests.cs          → Testa listagem de bandas
```

### Padrão dos testes

Todos os testes seguem o padrão **AAA (Arrange-Act-Assert)**:

```csharp
[Fact]
public async Task HandleAsync_ShouldCreateBand_WhenRequestIsValid()
{
    // Arrange - preparar dados e mocks
    var request = new CreateBandRequest(accountId, "Arctic Monkeys", "Indie Rock", "Banda inglesa", "7Ln80lUS6He07XvHI8qqHH");

    // Act - executar a ação
    var response = await _handler.HandleAsync(request, CancellationToken.None);

    // Assert - verificar o resultado
    response.Name.Should().Be("Arctic Monkeys");
}
```

---

## 📡 Endpoints da API

| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/bands` | Criar uma nova banda (vinculada a uma conta) |
| `GET` | `/bands` | Listar todas as bandas |

#### `POST /bands`

**Request:**
```json
{
  "accountId": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Arctic Monkeys",
  "description": "Banda inglesa de indie rock",
  "genre": "Indie Rock",
  "spotifyId": "7Ln80lUS6He07XvHI8qqHH"
}
```

**Response (201):**
```json
{
  "id": "660e8400-e29b-41d4-a716-446655440000",
  "accountId": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Arctic Monkeys",
  "genre": "Indie Rock",
  "description": "Banda inglesa de indie rock",
  "spotifyId": "7Ln80lUS6He07XvHI8qqHH",
  "createdAt": "2026-03-07T15:30:00Z"
}
```

#### `GET /bands`

**Response (200):**
```json
[
  {
    "id": "660e8400-e29b-41d4-a716-446655440000",
    "accountId": "550e8400-e29b-41d4-a716-446655440000",
    "name": "Arctic Monkeys",
    "genre": "Indie Rock",
    "description": "Banda inglesa de indie rock",
    "spotifyId": "7Ln80lUS6He07XvHI8qqHH",
    "createdAt": "2026-03-07T15:30:00Z"
  }
]
```

---

## 🤝 Contribuindo

1. Crie uma branch a partir da `main`:
   ```bash
   git checkout -b feature/minha-feature
   ```

2. Faça suas alterações seguindo a **Vertical Slice Architecture**

3. Escreva testes unitários para sua feature

4. Execute os testes e garanta que todos passam:
   ```bash
   dotnet test
   ```

5. Faça o commit e abra um Pull Request

---

<p align="center">
  Feito com ❤️ pelo time <strong>BandHub</strong>
</p>

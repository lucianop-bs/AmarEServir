# Arquitetura do Sistema 🏗️

> Documentação técnica da arquitetura do **Amar&Servir API** - Um guia completo sobre decisões de design, padrões e estrutura do projeto.

---

## 📋 Índice

- [Visão Geral](#-visão-geral)
- [Clean Architecture](#-clean-architecture)
- [Padrões de Design](#-padrões-de-design)
- [Estrutura de Pastas](#-estrutura-de-pastas)
- [Fluxo de Requisição](#-fluxo-de-requisição)
- [Camadas Detalhadas](#-camadas-detalhadas)
- [Decisões Técnicas](#-decisões-técnicas)
- [Diagramas](#-diagramas)

---

## 🎯 Visão Geral

O **Amar&Servir API** é construído seguindo os princípios de **Clean Architecture** (Arquitetura Limpa), garantindo:

- ✅ **Separação de Responsabilidades**: Cada camada tem um propósito específico
- ✅ **Independência de Frameworks**: O domínio não depende de tecnologias externas
- ✅ **Testabilidade**: Regras de negócio isoladas e testáveis
- ✅ **Manutenibilidade**: Código organizado e fácil de evoluir
- ✅ **Escalabilidade**: Preparado para crescer

### Stack Tecnológico

| Camada | Tecnologias |
|--------|-------------|
| **API** | ASP.NET Core 9, Minimal APIs |
| **Application** | MediatR (CQRS), FluentValidation |
| **Domain** | C# 12 Records, Value Objects |
| **Infrastructure** | MongoDB Driver, Docker |
| **Cross-Cutting** | JWT Authentication, Serilog |

---

## 🧅 Clean Architecture

### Princípio de Dependência

A regra fundamental: **as dependências apontam sempre para dentro** (em direção ao domínio).

```
┌─────────────────────────────────────────────────────────────┐
│                   🌐 API Layer                              │
│                (Controllers, Middleware)                    │
│                                                             │
│  Depende de ↓                                               │
└─────────────────────┬───────────────────────────────────────┘
                      │
┌─────────────────────▼───────────────────────────────────────┐
│              📦 Application Layer                           │
│         (Commands, Queries, Handlers, Validators)           │
│                                                             │
│  Depende de ↓                                               │
└─────────────────────┬───────────────────────────────────────┘
                      │
┌─────────────────────▼───────────────────────────────────────┐
│                  🧠 Domain Layer                            │
│    (Entities, Value Objects, Domain Services, Interfaces)   │
│                                                             │
│  ❌ NÃO depende de nada!                                    │
└──────────────────────────────────────────────────────────△──┘
                                                           │
┌──────────────────────────────────────────────────────────┴──┐
│              🗄️ Infrastructure Layer                        │
│        (Repositories, MongoDB, External Services)           │
│                                                             │
│  Depende de ↑ (implementa interfaces do Domain)            │
└─────────────────────────────────────────────────────────────┘
```

### Por que essa separação?

**Exemplo prático**: Se amanhã você quiser trocar MongoDB por PostgreSQL:

- ❌ **Sem Clean Architecture**: Reescrever toda a aplicação
- ✅ **Com Clean Architecture**: Trocar apenas a camada de Infrastructure

O Domain não sabe que MongoDB existe! Ele só conhece `IUserRepository`.

---

## 🎨 Padrões de Design

### 1. CQRS (Command Query Responsibility Segregation)

Separa operações de **leitura** (Queries) e **escrita** (Commands).

```
┌─────────────────────────────────────────────────────┐
│                    Request                          │
└────────────────────┬────────────────────────────────┘
                     │
         ┌───────────▼──────────┐
         │   É modificação?     │
         └───────┬──────────┬───┘
                 │          │
           Sim   │          │ Não
                 │          │
         ┌───────▼──────┐   ┌──▼────────┐
         │   Command    │   │   Query   │
         │  (Escrita)   │   │ (Leitura) │
         └───────┬──────┘   └──┬────────┘
                 │              │
         ┌───────▼──────────────▼────────┐
         │        MediatR                │
         └───────┬───────────────────────┘
                 │
         ┌───────▼──────────────┐
         │  ValidationBehavior  │ ← Pipeline
         └───────┬──────────────┘
                 │
         ┌───────▼──────────────┐
         │      Handler         │
         └──────────────────────┘
```

**Exemplo**:

```csharp
// Command (Escrita)
public record CreateUserCommand(CreateUserRequest User) 
    : IRequest<Result<UserResponse>>;

// Query (Leitura)
public record GetUserByIdQuery(Guid Id) 
    : IRequest<Result<UserResponse>>;
```

**Vantagens**:
- ✅ Separação clara de responsabilidades
- ✅ Otimizações específicas (cache em queries, transactions em commands)
- ✅ Escalabilidade (leitura e escrita podem ter bancos separados)

---

### 2. Result Pattern

Retorna erros de forma **explícita** sem usar exceptions para fluxo de negócio.

```csharp
// ❌ Abordagem Tradicional (Exceptions)
public User GetUser(Guid id) 
{
    var user = _repository.Find(id);
    if (user == null)
        throw new NotFoundException(); // 💥 Exception cara
    
    return user;
}

// ✅ Result Pattern
public async Task<Result<User>> GetUser(Guid id)
{
    var user = await _repository.Find(id);
    if (user == null)
        return Result<User>.Fail(UserErrors.NotFound);
    
    return Result<User>.Ok(user);
}
```

**Estrutura do Result**:

```csharp
public class Result<TValue>
{
    public bool IsSuccess { get; }
    public TValue Value { get; }
    public IReadOnlyCollection<IError> Errors { get; }
    
    public static Result<TValue> Ok(TValue value);
    public static Result<TValue> Fail(IError error);
}
```

**Vantagens**:
- ✅ Performance (sem overhead de exceptions)
- ✅ Explícito (assinatura deixa claro que pode falhar)
- ✅ Rastreável (códigos de erro consistentes)

---

### 3. Repository Pattern

Abstrai o acesso a dados, permitindo trocar a implementação sem afetar a lógica de negócio.

```csharp
// Interface (Domain Layer)
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}

// Implementação MongoDB (Infrastructure Layer)
public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _collection;
    
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _collection
            .Find(u => u.Id == id)
            .FirstOrDefaultAsync();
    }
}
```

**Vantagens**:
- ✅ Testável (mock do repository em testes)
- ✅ Flexível (trocar MongoDB por SQL sem mudar handlers)
- ✅ Centralizado (lógica de acesso em um lugar)

---

### 4. Mediator Pattern (MediatR)

Desacopla quem **envia** a requisição de quem **processa**.

```
Controller                       Handler
    │                               │
    │  Send(CreateUserCommand)      │
    ├──────────────►MediatR─────────►
    │                               │
    │        ◄─────Result────────────┤
    │                               │
```

**Vantagens**:
- ✅ Controller não conhece o Handler
- ✅ Pipeline de comportamentos (Validation, Logging, etc)
- ✅ Fácil adicionar novos casos de uso

---

### 5. Dependency Injection

Inverte o controle: quem **usa** não cria, quem **configura** injeta.

```csharp
// Registration (Program.cs)
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Usage (Handler)
public class CreateUserHandler
{
    private readonly IUserRepository _repository;
    
    // ASP.NET injeta automaticamente
    public CreateUserHandler(IUserRepository repository)
    {
        _repository = repository;
    }
}
```

**Lifetimes**:
- `Singleton`: Uma instância para toda aplicação
- `Scoped`: Uma instância por requisição HTTP
- `Transient`: Nova instância a cada injeção

---

## 📂 Estrutura de Pastas

```
AmarEServir/
├── backend/
│   ├── Auth.API/                         ← Projeto principal
│   │   │
│   │   ├── Api/                          ← Entry Points
│   │   │   ├── Controllers/              
│   │   │   │   ├── AuthController.cs     # Login, Refresh Token
│   │   │   │   ├── UserController.cs     # CRUD Usuários
│   │   │   │   └── CellsController.cs    # CRUD Células
│   │   │   │
│   │   │   └── Configurations/           
│   │   │       ├── ApiConfig.cs          # DI, MediatR, Filters
│   │   │       ├── AuthConfig.cs         # JWT, Policies
│   │   │       └── JwtSettings.cs        # Configurações JWT
│   │   │
│   │   ├── Application/                  ← Casos de Uso (CQRS)
│   │   │   │
│   │   │   ├── Users/                    
│   │   │   │   ├── CreateUser/           
│   │   │   │   │   ├── CreateUserCommand.cs
│   │   │   │   │   ├── CreateUserHandler.cs
│   │   │   │   │   ├── CreateUserValidator.cs
│   │   │   │   │   └── CreateUserMapper.cs
│   │   │   │   │
│   │   │   │   ├── UpdateUser/
│   │   │   │   ├── DeleteUser/
│   │   │   │   ├── GetUserByGuid/
│   │   │   │   │
│   │   │   │   └── Models/               
│   │   │   │       └── UserResponse.cs   # DTO de resposta
│   │   │   │
│   │   │   ├── Cells/                    # Mesma estrutura
│   │   │   ├── Auth/                     # Login, Refresh
│   │   │   │
│   │   │   ├── Common/                   
│   │   │   │   ├── ValidationBehavior.cs # Pipeline MediatR
│   │   │   │   └── Validators/           # Validadores reutilizáveis
│   │   │   │
│   │   │   └── Services/                 
│   │   │       ├── IJwtTokenService.cs
│   │   │       ├── JwtTokenService.cs
│   │   │       └── CurrentUserService.cs # Contexto do usuário logado
│   │   │
│   │   ├── Domain/                       ← Regras de Negócio
│   │   │   │
│   │   │   ├── Entities/                 # Entidades ricas
│   │   │   │   ├── User.cs               
│   │   │   │   ├── Cell.cs
│   │   │   │   ├── Address.cs            # Value Object
│   │   │   │   └── RefreshToken.cs
│   │   │   │
│   │   │   ├── Enums/
│   │   │   │   └── UserRole.cs           # Admin, Leader, etc
│   │   │   │
│   │   │   ├── Contracts/                # Interfaces (Abstrações)
│   │   │   │   ├── IUserRepository.cs
│   │   │   │   └── ICellRepository.cs
│   │   │   │
│   │   │   └── Errors/                   # Domain Errors
│   │   │       ├── UserErrors.cs
│   │   │       ├── CellError.cs
│   │   │       └── AuthErrors.cs
│   │   │
│   │   ├── Infrastructure/               ← Implementações Externas
│   │   │   │
│   │   │   ├── Persistence/
│   │   │   │   ├── Context/
│   │   │   │   │   └── MongoContext.cs   # Configuração MongoDB
│   │   │   │   │
│   │   │   │   ├── Mapping/
│   │   │   │   │   └── MongoDbMapping.cs # BsonClassMap
│   │   │   │   │
│   │   │   │   └── Repositories/         # Implementações
│   │   │   │       ├── UserRepository.cs
│   │   │   │       └── CellRepository.cs
│   │   │   │
│   │   │   └── DependencyInjection.cs    # Registration de infra
│   │   │
│   │   ├── Program.cs                    # Entry Point
│   │   ├── appsettings.json
│   │   ├── Dockerfile
│   │   └── docker-compose.yml
│   │
│   └── Core/                             ← Biblioteca Compartilhada
│       ├── Entities/
│       │   └── BaseEntity.cs             # Classe base (Id, CreatedAt, etc)
│       │
│       ├── Results/
│       │   ├── Base/
│       │   │   └── Result.cs
│       │   ├── Errors/
│       │   │   ├── Error.cs
│       │   │   ├── IError.cs
│       │   │   └── ErrorType.cs
│       │   └── Extensions/
│       │       ├── ResultExtensions.cs
│       │       └── ApiResultExtensions.cs
│       │
│       ├── Filters/
│       │   └── ApiResultFilter.cs        # Converte Result em HTTP
│       │
│       └── Middlewares/
│           └── GlobalExceptionHandler.cs # Captura exceptions globais
│
├── docs/
│   ├── ARCHITECTURE.md                   # Este arquivo!
│   └── postman/
│
├── .editorconfig
├── .gitignore
├── README.md
└── AmarEServir.sln
```

---

## 🔄 Fluxo de Requisição

### Exemplo: Criar Usuário

```
1️⃣ HTTP Request
   POST /api/user
   {
     "name": "João Silva",
     "email": "joao@example.com",
     ...
   }
        │
        ▼
2️⃣ Controller (UsersController.cs)
   public async Task<IActionResult> CreateUser(CreateUserRequest request)
   {
       var result = await _mediator.Send(new CreateUserCommand(request));
       return result.ToApiResult().ToActionResult();
   }
        │
        ▼
3️⃣ MediatR Pipeline
   ┌─────────────────────────────────┐
   │  ValidationBehavior             │
   │  • Executa CreateUserValidator  │
   │  • Se houver erros, retorna Fail│
   └─────────────┬───────────────────┘
                 │ ✅ Validação OK
                 ▼
4️⃣ Handler (CreateUserCommandHandler.cs)
   public async Task<Result<UserResponse>> Handle(...)
   {
       // 1. Verifica se email existe
       var exists = await _userRepository.GetByEmail(request.Email);
       if (exists) return Result.Fail(UserErrors.EmailExists);
       
       // 2. Hash da senha
       var hash = BCrypt.HashPassword(request.Password);
       
       // 3. Cria entidade
       var user = new User(...);
       
       // 4. Valida domínio
       var validation = user.Validate();
       if (!validation.IsSuccess) return Result.Fail(validation.Errors);
       
       // 5. Persiste
       await _userRepository.AddAsync(user);
       
       // 6. Retorna DTO
       return Result.Ok(user.ToResponse());
   }
        │
        ▼
5️⃣ Repository (UserRepository.cs)
   public async Task AddAsync(User user)
   {
       await _collection.InsertOneAsync(user);
   }
        │
        ▼
6️⃣ MongoDB
   Documento salvo!
        │
        ▼
7️⃣ Response
   HTTP 201 Created
   {
     "id": "550e8400-...",
     "name": "João Silva",
     "email": "joao@example.com",
     ...
   }
```

---

## 🔍 Camadas Detalhadas

### 1. API Layer (Apresentação)

**Responsabilidade**: Receber requisições HTTP e devolver respostas.

**Componentes**:
- **Controllers**: Endpoints REST
- **Filters**: Transformam `Result` em HTTP responses
- **Middlewares**: Exception handling, logging
- **Configurations**: DI, Swagger, CORS

**Exemplo**:
```csharp
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        var command = new CreateUserCommand(request);
        var result = await _mediator.Send(command);
        
        return result.ToApiResult(HttpStatusCode.Created).ToActionResult();
    }
}
```

**Regra de Ouro**: Controller não tem lógica de negócio! Só **delega** para o MediatR.

---

### 2. Application Layer (Casos de Uso)

**Responsabilidade**: Orquestrar a execução de casos de uso.

**Componentes**:
- **Commands**: Operações de escrita (Create, Update, Delete)
- **Queries**: Operações de leitura (Get, List)
- **Handlers**: Executam a lógica do caso de uso
- **Validators**: FluentValidation
- **Mappers**: Conversão Entity ↔ DTO

**Exemplo de Handler**:
```csharp
public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<UserResponse>>
{
    private readonly IUserRepository _repository;
    
    public async Task<Result<UserResponse>> Handle(CreateUserCommand request, ...)
    {
        // 1. Validação de negócio
        var emailExists = await _repository.GetByEmail(request.Email);
        if (emailExists != null)
            return Result.Fail(UserErrors.EmailAlreadyExists);
        
        // 2. Criação da entidade
        var user = new User(request.Name, request.Email, ...);
        
        // 3. Validação de domínio
        var validation = user.Validate();
        if (!validation.IsSuccess)
            return Result.Fail(validation.Errors);
        
        // 4. Persistência
        await _repository.AddAsync(user);
        
        // 5. Retorno
        return Result.Ok(user.ToResponse());
    }
}
```

**Regra de Ouro**: Handler **orquestra**, não implementa regras de domínio!

---

### 3. Domain Layer (Coração do Sistema)

**Responsabilidade**: Conter as regras de negócio puras.

**Componentes**:
- **Entities**: Objetos com identidade (User, Cell)
- **Value Objects**: Objetos sem identidade (Address)
- **Domain Services**: Lógica que não pertence a uma entidade
- **Errors**: Erros de domínio tipados
- **Interfaces**: Contratos (IUserRepository)

**Exemplo de Entidade**:
```csharp
public class User : BaseEntity<Guid>
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public Address Address { get; private set; }
    
    // Construtor
    public User(string name, string email, ...)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        ...
    }
    
    // Validação de domínio
    public Result Validate()
    {
        if (string.IsNullOrEmpty(Name))
            return Result.Fail(UserErrors.NameRequired);
        
        if (string.IsNullOrEmpty(Email))
            return Result.Fail(UserErrors.EmailRequired);
        
        return Address.Validate(); // Cascata
    }
    
    // Comportamento de domínio
    public void UpdateProfile(string name, string phone)
    {
        if (!string.IsNullOrEmpty(name)) Name = name;
        if (!string.IsNullOrEmpty(phone)) Phone = phone;
        
        SetUpdatedAtDate(DateTime.UtcNow);
    }
}
```

**Regras de Ouro**:
- ❌ Domínio NÃO conhece: ASP.NET, MongoDB, MediatR
- ✅ Domínio CONHECE: Apenas C# puro e suas próprias interfaces
- ✅ Setters `private`: Só pode mudar através de métodos (encapsulamento)

---

### 4. Infrastructure Layer (Implementações)

**Responsabilidade**: Implementar as interfaces do domínio usando tecnologias concretas.

**Componentes**:
- **Repositories**: Implementações MongoDB
- **Context**: Configuração do banco
- **Mappings**: BsonClassMap (como MongoDB serializa)
- **External Services**: APIs externas, Email, etc

**Exemplo de Repository**:
```csharp
public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _collection;
    
    public UserRepository(MongoContext context)
    {
        _collection = context.GetCollection<User>("User");
    }
    
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _collection
            .Find(u => u.Id == id)
            .FirstOrDefaultAsync();
    }
    
    public async Task AddAsync(User user)
    {
        await _collection.InsertOneAsync(user);
    }
}
```

**Regra de Ouro**: Infrastructure **implementa**, não define contratos!

---

## 🧠 Decisões Técnicas

### Por que MongoDB?

| Critério | Decisão |
|----------|---------|
| **Estrutura de Dados** | Dados aninhados (User tem Address dentro) |
| **Flexibilidade** | Schema pode evoluir (adicionar campos sem migrations) |
| **Performance** | Uma query busca tudo (sem joins) |
| **Escalabilidade** | Sharding nativo para crescimento horizontal |

**Trade-off**: Menos adequado para relacionamentos complexos (N:N). Neste projeto, os relacionamentos são simples (Cell → Leader).

---

### Por que CQRS?

| Benefício | Descrição |
|-----------|-----------|
| **Separação de Responsabilidades** | Queries otimizadas (cache), Commands com validação |
| **Escalabilidade** | Read/Write podem ter bancos separados no futuro |
| **Testabilidade** | Handlers isolados e fáceis de testar |
| **Manutenibilidade** | Adicionar novo caso de uso = criar novo Handler |

---

### Por que Result Pattern?

**Problema com Exceptions**:
```csharp
public User GetUser(Guid id)
{
    var user = _repository.Find(id);
    
    if (user == null)
        throw new NotFoundException(); // 💥 Cara!
    
    return user;
}
```

**Problemas**:
- 🐌 Exceptions têm overhead (stack trace)
- 🤷 Assinatura não indica que pode falhar
- 🔀 Difícil saber onde capturar

**Solução com Result**:
```csharp
public async Task<Result<User>> GetUser(Guid id)
{
    var user = await _repository.Find(id);
    
    if (user == null)
        return Result<User>.Fail(UserErrors.NotFound); // ✅ Rápido e explícito
    
    return Result<User>.Ok(user);
}
```

**Vantagens**:
- ⚡ Sem overhead
- 📝 Assinatura clara
- 🎯 Tratamento centralizado

---

### Por que JWT?

| Característica | Descrição |
|----------------|-----------|
| **Stateless** | Servidor não guarda sessão (escalável) |
| **Self-contained** | Token carrega dados do usuário |
| **Seguro** | Signature garante integridade |
| **Padrão** | RFC 7519 (amplamente suportado) |

**Estrutura do Token**:
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9  ← Header
.
eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ  ← Payload
.
SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c  ← Signature
```

**Refresh Token Strategy**:
- Access Token (5 min): Autenticação das requisições
- Refresh Token (7 dias): Renovação sem pedir senha

---

## 📊 Diagramas

### Diagrama de Componentes

```
┌──────────────────────────────────────────────────────────────┐
│                         Client                               │
│                    (React, Mobile App)                       │
└────────────────────────┬─────────────────────────────────────┘
                         │ HTTP/REST
                         │
┌────────────────────────▼─────────────────────────────────────┐
│                      API Gateway                             │
│               (CORS, Auth, Rate Limit)                       │
└────────────────────────┬─────────────────────────────────────┘
                         │
         ┌───────────────┼───────────────┐
         │               │               │
┌────────▼────────┐ ┌───▼──────┐ ┌─────▼────────┐
│ AuthController  │ │UsersCtrl │ │ CellsCtrl    │
└────────┬────────┘ └───┬──────┘ └─────┬────────┘
         │              │               │
         └──────────────┼───────────────┘
                        │
                ┌───────▼───────┐
                │    MediatR    │
                │  (Pipeline)   │
                └───────┬───────┘
                        │
         ┌──────────────┼──────────────┐
         │              │              │
    ┌────▼────┐    ┌───▼───┐    ┌────▼────┐
    │Commands │    │Queries│    │Behaviors│
    └────┬────┘    └───┬───┘    └────┬────┘
         │             │              │
         └─────────────┼──────────────┘
                       │
              ┌────────▼────────┐
              │    Handlers     │
              └────────┬────────┘
                       │
              ┌────────▼────────┐
              │  Repositories   │
              └────────┬────────┘
                       │
              ┌────────▼────────┐
              │    MongoDB      │
              └─────────────────┘
```

---

### Diagrama de Sequência (Login)

```
Cliente          Controller       MediatR      Handler      Repository      MongoDB
  │                  │               │            │             │             │
  │ POST /login      │               │            │             │             │
  ├─────────────────►│               │            │             │             │
  │                  │ Send(Command) │            │             │             │
  │                  ├──────────────►│            │             │             │
  │                  │               │ Validate   │             │             │
  │                  │               ├───────────►│             │             │
  │                  │               │   OK       │             │             │
  │                  │               │◄───────────┤             │             │
  │                  │               │ Handle()   │             │             │
  │                  │               ├───────────►│             │             │
  │                  │               │            │ GetByEmail()│             │
  │                  │               │            ├────────────►│   Find()    │
  │                  │               │            │             ├────────────►│
  │                  │               │            │             │◄────────────┤
  │                  │               │            │◄────────────┤             │
  │                  │               │            │ Verify Password           │
  │                  │               │            ├─────────────              │
  │                  │               │            │ Generate JWT              │
  │                  │               │            ├─────────────              │
  │                  │               │  Result.Ok │             │             │
  │                  │               │◄───────────┤             │             │
  │                  │  Result       │            │             │             │
  │                  │◄──────────────┤            │             │             │
  │   200 OK + Token │               │            │             │
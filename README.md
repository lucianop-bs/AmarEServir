# Amar&Servir 🤝

API backend para o projeto **Amar & Servir**, desenvolvida em **.NET 9** seguindo os princípios de **Clean Architecture**, **Clean Code** e utilizando padrões modernos como **CQRS** com **MediatR** e **Result Pattern**.

---
    
## 📋 Índice

- [Tecnologias](#tecnologias)
- [Arquitetura](#arquitetura)
  - [Clean Architecture](#clean-architecture)
  - [CQRS com MediatR](#cqrs-com-mediatr)
  - [Result Pattern](#result-pattern)
  - [Validation Pipeline](#validation-pipeline)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Pré-requisitos](#pré-requisitos)
- [Configuração do Ambiente](#configuração-do-ambiente)
- [Como Executar](#como-executar)
- [Endpoints da API](#endpoints-da-api)
- [Validações](#validações)
- [Exemplos de Requisições](#exemplos-de-requisições)

---

## 🚀 Tecnologias

| Tecnologia | Descrição |
|------------|-----------|
| **.NET 9** | Framework principal |
| **MongoDB** | Banco de dados NoSQL |
| **MediatR** | Implementação do padrão CQRS |
| **FluentValidation** | Validação declarativa |
| **Docker** | Containerização |

---

## 🏗️ Arquitetura

### Clean Architecture

O projeto segue os princípios da **Clean Architecture** (Arquitetura Limpa), garantindo:

- ✅ **Independência de frameworks** - O domínio não depende de bibliotecas externas
- ✅ **Testabilidade** - Regras de negócio testáveis isoladamente
- ✅ **Independência de UI** - A API pode ser substituída sem alterar o domínio
- ✅ **Independência de banco** - MongoDB pode ser trocado por outro banco

```
┌─────────────────────────────────────────────────────────────┐
│                      API Layer                              │
│                    (Controllers)                            │
└─────────────────────────┬───────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────────┐
│                  Application Layer                          │
│         (Commands, Queries, Handlers, Validators)           │
└─────────────────────────┬───────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────────┐
│                    Domain Layer                             │
│           (Entities, Contracts, Errors)                     │
└─────────────────────────┬───────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────────┐
│                 Infrastructure Layer                        │
│              (Repositories, MongoDB)                        │
└─────────────────────────────────────────────────────────────┘
```

---

### CQRS com MediatR

O projeto utiliza o padrão **CQRS (Command Query Responsibility Segregation)** através do **MediatR**:

#### Commands (Escrita)
```csharp
// Definição do Command
public record DeleteCellCommand(Guid Id) : IRequest<Result>;

// Handler que processa o Command
public class DeleteCellCommandHandler : IRequestHandler<DeleteCellCommand, Result>
{
    public async Task<Result> Handle(DeleteCellCommand request, CancellationToken cancellationToken)
    {
        var cell = await _cellRepository.GetCellByGuid(request.Id);
        if (cell is null)
            return Result.Fail(CellError.NotFound);

        await _cellRepository.Delete(cell.Id);
        return Result.Ok();
    }
}
```

#### Queries (Leitura)
```csharp
public record GetUserByGuidQuery(Guid Id) : IRequest<Result<UserModelView>>;
```

#### Benefícios do CQRS + MediatR
- 📦 **Desacoplamento** - Controllers não conhecem a implementação
- 🔄 **Pipeline de comportamentos** - Validação automática antes dos handlers
- 📊 **Separação clara** - Operações de leitura vs escrita bem definidas

---

### Result Pattern

O projeto implementa o **Result Pattern** para tratamento de erros de forma elegante, evitando exceções para fluxos de negócio:

```csharp
// Estrutura do Result
public class Result : ResultBase
{
    public static Result Ok() => new();
    public static Result Fail(IError error) => new(error);
    public static Result<TValue> Ok<TValue>(TValue value) => Result<TValue>.Ok(value);
}

// Estrutura do Error
public record Error(string Code, string Message, ErrorType Type) : IError;

// Tipos de erro disponíveis
public enum ErrorType
{
    Validation = 400,
    Unauthorized = 401,
    NotFound = 404,
    Conflict = 409,
    Internal = 500
}
```

#### Uso nos Handlers
```csharp
// Retornando sucesso
return Result.Ok(cellModelView);

// Retornando erro
return Result.Fail(CellError.NotFound);

// Retornando múltiplos erros
return Result.Fail(validationErrors);
```

---

### Validation Pipeline

O **MediatR Pipeline** intercepta todas as requisições e executa validações automaticamente antes de chegarem aos handlers:

```csharp
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // 1. Executa todas as validações
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        // 2. Coleta os erros
        var failures = validationResults.SelectMany(r => r.Errors).ToList();

        // 3. Se houver erros, retorna Result.Fail
        if (failures.Any())
            return Result.Fail(errors);

        // 4. Se não houver erros, continua para o Handler
        return await next();
    }
}
```

#### Validators com FluentValidation
```csharp
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithErrorCode("USER.NAME_REQUIRED")
            .Length(3, 50).WithErrorCode("USER.NAME_LENGTH");

        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress();

        RuleFor(x => x.Password)
            .MinimumLength(6);
    }
}
```

---

## 📂 Estrutura do Projeto

```
backend/
├── Auth.API/
│   ├── Api/
│   │   ├── Controllers/              # Endpoints da API
│   │   │   ├── UserController.cs
│   │   │   └── CellsController.cs
│   │   └── Configurations/           # Configurações (DI, Swagger, etc)
│   │
│   ├── Application/                  # Casos de uso (CQRS)
│   │   ├── Common/
│   │   │   └── ValidateBehavior.cs   # Pipeline de validação
│   │   ├── Users/
│   │   │   ├── CreateUser/
│   │   │   │   ├── CreateUserCommand.cs
│   │   │   │   ├── CreateUserCommandHandler.cs
│   │   │   │   ├── CreateUserCommandValidator.cs
│   │   │   │   └── CreateUserMapper.cs
│   │   │   ├── UpdateUser/
│   │   │   ├── DeleteUser/
│   │   │   └── GetUserByGuid/
│   │   └── Cells/
│   │       ├── CreateCell/
│   │       ├── UpdateCell/
│   │       ├── DeleteCell/
│   │       └── GetCellByGuid/
│   │
│   ├── Domain/                       # Entidades e regras de negócio
│   │   ├── User.cs
│   │   ├── Cell.cs
│   │   ├── Address.cs
│   │   ├── Enums/
│   │   │   └── UserRole.cs
│   │   ├── Contracts/                # Interfaces dos repositórios
│   │   │   ├── IUserRepository.cs
│   │   │   └── ICellRepository.cs
│   │   └── Errors/                   # Erros de domínio
│   │       ├── UserErrors.cs
│   │       └── CellErrors.cs
│   │
│   └── Infrastructure/               # Implementações externas
│       └── Persistence/
│           ├── Context/
│           │   └── MongoContext.cs
│           └── Repositories/
│               ├── UserRepository.cs
│               └── CellRepository.cs
│
└── Core/                             # Biblioteca compartilhada
    ├── Entities/
    │   └── BaseEntity.cs
    ├── Results/
    │   ├── Base/
    │   │   └── Result.cs             # Result Pattern
    │   └── Errors/
    │       ├── Error.cs
    │       ├── IError.cs
    │       └── ErrorType.cs
    ├── Filters/
    │   └── ApiResultFilter.cs
    └── Middlewares/
        └── GlobalExceptionHandler.cs
```

---

## 📦 Pré-requisitos

- [Docker](https://www.docker.com/get-started) e Docker Compose
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (para desenvolvimento)

---

## ⚙️ Configuração do Ambiente

### 1. Clone o repositório

```bash
git clone https://github.com/lucianop-bs/AmarEServir.git
cd AmarEServir
```

### 2. Configure as variáveis de ambiente

Crie um arquivo `.env` na pasta `backend/Auth.API/` baseado no `example.env`:

```bash
cd backend/Auth.API
cp example.env .env
```

Edite o arquivo `.env`:

```env
MONGO_ROOT_USER="admin"
MONGO_ROOT_PASS="sua_senha_segura"
DATABASE_NAME="amarservir_db"
```

| Variável | Descrição |
|----------|-----------|
| `MONGO_ROOT_USER` | Usuário root do MongoDB |
| `MONGO_ROOT_PASS` | Senha do usuário root |
| `DATABASE_NAME` | Nome do banco de dados |

---

## ▶️ Como Executar

### Com Docker (Recomendado)

```bash
cd backend/Auth.API
docker-compose up -d
```

🌐 API disponível em: `http://localhost:8080`

### Desenvolvimento Local

```bash
cd backend/Auth.API
dotnet restore
dotnet run
```

---

## 📡 Endpoints da API

**Base URL:** `http://localhost:8080/api`

### Usuários (`/api/user`)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `POST` | `/user` | Criar usuário |
| `GET` | `/user/{id}` | Buscar por ID |
| `PATCH` | `/user/{id}` | Atualizar |
| `DELETE` | `/user/{id}` | Excluir |

### Células (`/api/cells`)

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `POST` | `/cells` | Criar célula |
| `GET` | `/cells/{id}` | Buscar por ID |
| `PATCH` | `/cells/{id}` | Atualizar |
| `DELETE` | `/cells/{id}` | Excluir |

---

## ✅ Validações

### Usuário

| Campo | Regras |
|-------|--------|
| `name` | Obrigatório, 3-50 caracteres |
| `email` | Obrigatório, formato válido |
| `phone` | Obrigatório, 11-13 caracteres |
| `password` | Obrigatório, mínimo 6 caracteres |
| `role` | Enum válido (1=Admin, 2=Leader, 3=Volunteer, 4=Beneficiary) ou ("Admin", "Leader", "Volunteer", "Beneficiary") |
| `address` | Obrigatório (rua, numero, bairro, cidade, estado, cep) |

### Célula

| Campo | Regras |
|-------|--------|
| `name` | Obrigatório, 3-100 caracteres, Uma célula não pode ter o mesmo nome|
| `leaderId` | GUID válido (não vazio), Um líder só pode ter uma célula |

---

## 📝 Exemplos de Requisições

### Criar Usuário

```http
POST http://localhost:8080/api/user
Content-Type: application/json

{
  "name": "João Silva",
  "email": "joao@email.com",
  "phone": "11999998888",
  "password": "senha123",
  "role": 3,
  "address": {
    "rua": "Rua das Flores",
    "quadra": "A",
    "numero": "123",
    "bairro": "Centro",
    "cidade": "São Paulo",
    "estado": "SP",
    "complemento": "Apto 45",
    "cep": "01234567"
  }
}
```

### Resposta de Sucesso

```json
{
  "isSuccess": true,
  "value": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "name": "João Silva",
    "email": "joao@email.com",
    "phone": "11999998888",
    "role": 3,
    "address": { ... }
  }
}
```

### Resposta de Erro (Validação)

```json
{
  "isSuccess": false,
  "errors": [
    {
      "code": "USER.NAME_REQUIRED",
      "message": "O nome é obrigatório",
      "type": 400
    }
  ]
}
```

### Criar Célula

```http
POST http://localhost:8080/api/cells
Content-Type: application/json

{
  "name": "Célula Esperança",
  "leaderId": "550e8400-e29b-41d4-a716-446655440000"
}
```

### Buscar Usuário

```http
GET http://localhost:8080/api/user/550e8400-e29b-41d4-a716-446655440000
```

### Atualizar Usuário

```http
PATCH http://localhost:8080/api/user/550e8400-e29b-41d4-a716-446655440000
Content-Type: application/json

{
  "name": "João Silva Atualizado",
  "email": "joao.novo@email.com",
  "phone": "11888887777",
  "role": 2,
  "address": {
    "rua": "Rua Nova",
    "quadra": "B",
    "numero": "456",
    "bairro": "Jardim",
    "cidade": "São Paulo",
    "estado": "SP",
    "complemento": "",
    "cep": "01234999"
  }
}
```

### Excluir Usuário

```http
DELETE http://localhost:8080/api/user/550e8400-e29b-41d4-a716-446655440000
```

### Atualizar Célula

```http
PATCH http://localhost:8080/api/cells/660e8400-e29b-41d4-a716-446655440001
Content-Type: application/json

{
  "name": "Célula Renovada",
  "leaderId": "770e8400-e29b-41d4-a716-446655440002"
}
```

---

## 🐳 Docker

```bash
# Iniciar containers
docker-compose up -d

# Ver logs
docker-compose logs -f auth-api

# Parar containers
docker-compose down

# Rebuild
docker-compose up -d --build
```

---

## 📄 Licença

Este projeto está sob a licença MIT.

---

## 🤝 Contribuição

1. Fork o projeto
2. Crie sua branch (`git checkout -b feature/nova-feature`)
3. Commit (`git commit -m 'Add nova feature'`)
4. Push (`git push origin feature/nova-feature`)
5. Abra um Pull Request
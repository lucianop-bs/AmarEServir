# Amar&Servir API 🤝

> API REST para gerenciamento de células comunitárias, desenvolvida com .NET 9, Clean Architecture e MongoDB.

## 📋 Sobre o Projeto

Sistema para gestão de grupos comunitários (células), permitindo:

- ✅ Autenticação JWT com refresh token
- 👥 Gerenciamento de usuários (Admin, Líder, Voluntário, Beneficiário)
- 🏠 Administração de células e seus membros
- 🔒 Controle de acesso baseado em roles

---

## 🚀 Início Rápido

### Pré-requisitos

- [Docker](https://www.docker.com/get-started) e Docker Compose
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) _(opcional, para desenvolvimento local)_

### Execução com Docker

1. **Clone o repositório**

```bash
git clone https://github.com/seu-usuario/AmarEServir.git
cd AmarEServir/backend/Auth.API
```

2. **Configure as variáveis de ambiente**

```bash
cp example.env .env
```

Edite o arquivo `.env`:

```env
MONGO_ROOT_USER=admin
MONGO_ROOT_PASS=suaSenhaSegura123
DATABASE_NAME=AmarEServir
JWT_SECRET_KEY=sua-chave-secreta-aqui
```

3. **Inicie os containers**

```bash
docker-compose up -d
```

4. **Acesse a API**

- **API**: http://localhost:8080
- **Documentação Scalar**: http://localhost:8080/scalar/v1

---

## 📡 Endpoints Principais

### 🔐 Autenticação

| Método | Endpoint                  | Descrição               |
| ------ | ------------------------- | ----------------------- |
| `POST` | `/api/auth/login`         | Autenticação            |
| `POST` | `/api/auth/refresh-token` | Renovar token           |
| `GET`  | `/api/auth/me`            | Dados do usuário logado |

**Exemplo - Login:**

```json
POST /api/auth/login
{
  "email": "admin@example.com",
  "password": "senha123"
}
```

**Resposta:**

```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "a1b2c3d4e5f6...",
  "time": 300
}
```

---

### 👥 Usuários

| Método   | Endpoint         | Descrição       | Requer Token? |
| -------- | ---------------- | --------------- | ------------- |
| `POST`   | `/api/user`      | Criar usuário   | 🔓 Não        |
| `GET`    | `/api/user/{id}` | Buscar por ID   | 🔒 Sim        |
| `PATCH`  | `/api/user/{id}` | Atualizar dados | 🔒 Sim        |
| `DELETE` | `/api/user/{id}` | Excluir usuário | 🔒 Sim        |

**Exemplo - Criar Usuário:**

```json
POST /api/user
{
  "name": "João Silva",
  "email": "joao@example.com",
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

**Roles disponíveis:**

- `1` - Admin
- `2` - Leader (Líder)
- `3` - Volunteer (Voluntário)
- `4` - Beneficiary (Beneficiário)

---

### 🏠 Células

| Método   | Endpoint          | Descrição        | Requer Token? |
| -------- | ----------------- | ---------------- | ------------- |
| `POST`   | `/api/cells`      | Criar célula     | 🔒 Sim        |
| `GET`    | `/api/cells/{id}` | Buscar por ID    | 🔒 Sim        |
| `PATCH`  | `/api/cells/{id}` | Atualizar célula | 🔒 Sim        |
| `DELETE` | `/api/cells/{id}` | Excluir célula   | 🔒 Sim        |

**Exemplo - Criar Célula:**

```json
POST /api/cells
{
  "name": "Célula Esperança",
  "leaderId": "550e8400-e29b-41d4-a716-446655440000"
}
```

**Regras:**

- ✅ Somente usuários com `role: 2` (Leader) podem liderar células
- ✅ Um líder pode ter apenas uma célula
- ✅ Nomes de células devem ser únicos

---

## 🔑 Autenticação nas Requisições

Após o login, use o token JWT no header `Authorization`:

```
Authorization: Bearer SEU_TOKEN_AQUI
```

**Tokens:**

- **Access Token**: Válido por 5 minutos (use nas requisições)
- **Refresh Token**: Válido por 7 dias (use para renovar o access token)

**Renovar token expirado:**

```json
POST /api/auth/refresh-token
{
  "refreshToken": "SEU_REFRESH_TOKEN"
}
```

---

## 🏗️ Arquitetura

```
┌─────────────────────────────────────┐
│  API Layer (Controllers)            │  → Recebe requisições HTTP
├─────────────────────────────────────┤
│  Application (CQRS + MediatR)       │  → Lógica de casos de uso
│  • Commands (Criar/Atualizar)       │
│  • Queries (Buscar)                 │
│  • Validators (FluentValidation)    │
├─────────────────────────────────────┤
│  Domain (Entidades + Regras)        │  → Regras de negócio
│  • User, Cell, Address              │
│  • Validações de domínio            │
├─────────────────────────────────────┤
│  Infrastructure (MongoDB)           │  → Persistência de dados
│  • Repositories                     │
└─────────────────────────────────────┘
```

**Tecnologias:**

- .NET 9 (ASP.NET Core)
- MongoDB (NoSQL)
- MediatR (CQRS)
- FluentValidation
- JWT Authentication
- Docker

---

## 🧪 Testando a API

### Opção 1: Postman/Insomnia

Importe a collection disponível em `docs/AmarEServir.postman_collection.json`

### Opção 2: Scalar UI

Acesse http://localhost:8080/scalar para documentação interativa.

### Fluxo de Teste Completo

**1. Criar usuário líder:**

```json
POST /api/user
{
  "name": "Maria Santos",
  "email": "maria@example.com",
  "phone": "11988887777",
  "password": "senha123",
  "role": 2,
  "address": {
    "rua": "Av. Principal",
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

**2. Fazer login:**

```json
POST /api/auth/login
{
  "email": "maria@example.com",
  "password": "senha123"
}
```

**3. Criar célula (use o token do passo 2):**

```json
POST /api/cells
Authorization: Bearer SEU_TOKEN_AQUI

{
  "name": "Célula Fé",
  "leaderId": "GUID_DA_MARIA"
}
```

---

## 📦 Desenvolvimento Local (sem Docker)

1. **Instale o MongoDB** ou use MongoDB Atlas (cloud)

2. **Configure o `appsettings.Development.json`**:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "AmarEServir"
  }
}
```

3. **Execute o projeto**:

```bash
cd backend/Auth.API
dotnet restore
dotnet run
```

API disponível em: https://localhost:7001

---

## 🐳 Comandos Docker Úteis

```bash
# Ver logs da API
docker-compose logs -f auth-api

# Ver logs do MongoDB
docker-compose logs -f mongodb

# Parar containers
docker-compose down

# Rebuild após mudanças no código
docker-compose up -d --build

# Remover volumes (apaga dados do banco)
docker-compose down -v
```

---

## 📝 Regras de Validação

### Usuário

- **Nome**: 3-50 caracteres
- **Email**: Formato válido, único
- **Telefone**: 11-13 caracteres
- **Senha**: Mínimo 6 caracteres
- **CEP**: Exatamente 8 dígitos
- **Estado**: Exatamente 2 caracteres (UF)

### Célula

- **Nome**: 3-100 caracteres, único
- **Líder**: Deve existir e ter role "Leader"
- **Restrição**: Um líder só pode liderar uma célula

---

## 🔒 Segurança

- ✅ Senhas armazenadas com BCrypt (hash)
- ✅ Tokens JWT com expiração
- ✅ Refresh tokens com revogação automática
- ✅ HTTPS habilitado em produção
- ✅ Validação de entrada em todas as rotas

---

## 🤝 Contribuindo

1. Fork o projeto
2. Crie uma branch: `git checkout -b feature/nova-funcionalidade`
3. Commit: `git commit -m 'Add: nova funcionalidade'`
4. Push: `git push origin feature/nova-funcionalidade`
5. Abra um Pull Request

---

## 📄 Licença

Este projeto está sob a licença MIT.

---

## 📞 Suporte

- 📧 Email: lucianop.borges1@icloud.com
- 🐛 Issues: [GitHub Issues](https://github.com/seu-usuario/AmarEServir/issues)

---

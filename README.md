# Digital Wallet API

## Descrição
API para gerenciamento de carteira digital, permitindo transferências entre usuários e consulta de saldo.

## Tecnologias Utilizadas
- .NET 9
- ASP.NET Core
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger/OpenAPI
- Clean Architecture
- Domain-Driven Design (DDD)

## Escolha do Banco de Dados
Optei pelo SQL Server por ser o banco de dados relacional com o qual tenho maior experiência e familiaridade no ecossistema .NET

## Estrutura do Projeto

```
DigitalWalletAPI/
├── API/                 # Camada de apresentação
├── Application/         # Camada de aplicação
├── Domain/             # Camada de domínio
└── Infrastructure/     # Camada de infraestrutura
```

## Funcionalidades

- Autenticação de usuários com JWT
- Criação e gerenciamento de carteiras
- Transferências entre carteiras
- Consulta de saldo
- Histórico de transações
- Configurações do sistema

## Endpoints da API

### Usuários
- `POST /api/users/register` - Registra um novo usuário
  ```json
  {
    "name": "Nome do Usuário",
    "email": "email@exemplo.com",
    "password": "Senha@123"
  }
  ```
- `POST /api/users/login` - Autentica um usuário
  ```json
  {
    "email": "email@exemplo.com",
    "password": "Senha@123"
  }
  ```

### Carteiras
- `GET /api/wallets/user/{userId}` - Obtém carteira do usuário
- `GET /api/wallets/balance/{walletId}` - Consulta saldo da carteira
- `POST /api/wallets/balance/{walletId}` - Adiciona saldo à carteira
  ```json
  {
    "amount": 100.50
  }
  ```

### Transações
- `POST /api/transactions/wallet/{senderWalletId}` - Realiza transferência
  ```json
  {
    "receiverWalletId": "guid_da_carteira_destino",
    "amount": 100.50
  }
  ```
- `GET /api/transactions/wallet/{walletId}` - Lista transações da carteira
- `GET /api/transactions/{id}` - Obtém detalhes de uma transação

## Pré-requisitos

- .NET 9 SDK
- SQL Server
- Visual Studio 2022 ou VS Code

## Configuração

1. Clone o repositório
2. Configure a string de conexão no `appsettings.json`
3. Execute as migrações:
```bash
dotnet ef database update
```
4. Execute o projeto:
```bash
dotnet run
```

## Documentação da API

A documentação da API está disponível via Swagger em:
```
https://localhost:7001/swagger
```

## Tratamento de Erros

A API utiliza um sistema padronizado de tratamento de erros com códigos HTTP apropriados:

- 400: Requisição inválida
- 401: Não autorizado
- 404: Recurso não encontrado
- 422: Erro de validação
- 500: Erro interno do servidor

## Autenticação

A API utiliza JWT (JSON Web Token) para autenticação. Para acessar endpoints protegidos:

1. Faça login via endpoint `/api/users/login`
2. Use o token retornado no header `Authorization: Bearer {token}`

## Configurações do Sistema
As configurações do sistema são gerenciadas através da tabela `SystemConfigurations` no banco de dados. As seguintes configurações são necessárias:

### Wallet
- `value_initial_wallet`: Valor inicial da carteira.

### JWT
- `jwt_secret_key`: Chave secreta para assinatura do token JWT
- `jwt_expiration_minutes`: Tempo de expiração do token em minutos (padrão: "60")
- `jwt_issuer`: Emissor do token (padrão: "DigitalWalletAPI")
- `jwt_audience`: Audiência do token (padrão: "DigitalWalletAPI")
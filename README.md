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

## Escolha do Banco de Dados

Optei pelo SQL Server por ser o banco de dados relacional com o qual tenho maior experiência e familiaridade no ecossistema .NET

## Arquitetura
- Clean Architecture
- Repository Pattern
- DTOs para transferência de dados
- Injeção de Dependência

## Configurações do Sistema
As configurações do sistema são gerenciadas através da tabela `SystemConfigurations` no banco de dados. As seguintes configurações são necessárias:

### Wallet
- `value_initial_wallet`: Valor inicial da carteira.

### JWT
- `jwt_secret_key`: Chave secreta para assinatura do token JWT
- `jwt_expiration_minutes`: Tempo de expiração do token em minutos (padrão: "60")
- `jwt_issuer`: Emissor do token (padrão: "DigitalWalletAPI")
- `jwt_audience`: Audiência do token (padrão: "DigitalWalletAPI")

## Configuração do Banco de Dados
1. Execute as migrações:
```bash
dotnet ef database update
```

2. Insira as configurações iniciais:
```sql
-- Configuração da Wallet
INSERT INTO SystemConfigurations (Parameter, Value, Description, CreatedAt, UpdatedAt)
VALUES ('value_initial_wallet', '0', 'Valor inicial da carteira', GETUTCDATE(), GETUTCDATE());

-- Configurações do JWT
INSERT INTO SystemConfigurations (Parameter, Value, Description, CreatedAt, UpdatedAt)
VALUES ('jwt_secret_key', 'chave_jwt_secret', 'Chave secreta para assinatura do JWT, a mesma que deverá estar em appsettings.json', GETUTCDATE(), GETUTCDATE());

INSERT INTO SystemConfigurations (Parameter, Value, Description, CreatedAt, UpdatedAt)
VALUES ('jwt_expiration_minutes', '60', 'Tempo de expiração do token JWT em minutos', GETUTCDATE(), GETUTCDATE());

INSERT INTO SystemConfigurations (Parameter, Value, Description, CreatedAt, UpdatedAt)
VALUES ('jwt_issuer', 'DigitalWalletAPI', 'Emissor do token JWT', GETUTCDATE(), GETUTCDATE());

INSERT INTO SystemConfigurations (Parameter, Value, Description, CreatedAt, UpdatedAt)
VALUES ('jwt_audience', 'DigitalWalletAPI', 'Audiência do token JWT', GETUTCDATE(), GETUTCDATE());
```

## Autenticação
A API utiliza autenticação JWT (JSON Web Token). Para acessar os endpoints protegidos:

1. Registre um usuário usando o endpoint `POST /api/Users/register`
2. Faça login usando o endpoint `POST /api/Users/login`
3. Use o token retornado no header `Authorization: Bearer {token}`

## Endpoints

### Usuários
- `POST /api/Users/register` - Registra um novo usuário
  ```json
  {
    "name": "Nome do Usuário",
    "email": "email@exemplo.com",
    "password": "senha123"
  }
  ```

- `POST /api/Users/login` - Autentica um usuário
  ```json
  {
    "email": "email@exemplo.com",
    "password": "senha123"
  }
  ```

### Carteira
- `GET /api/Wallets/balance` - Consulta saldo da carteira

### Transações
- `POST /api/Transactions/wallet/{senderWalletId}` - Realiza transferência
  ```json
  {
    "receiverWalletId": "guid_da_carteira_destino",
    "amount": 100.50
  }
  ```
- `GET /api/Transactions/wallet/{walletId}` - Lista transações da carteira
- `GET /api/Transactions/{id}` - Obtém detalhes de uma transação

## Executando o Projeto
1. Clone o repositório
2. Restaure os pacotes:
```bash
dotnet restore
```
3. Execute as migrações:
```bash
dotnet ef database update
```
4. Inicie o projeto:
```bash
dotnet run --project DigitalWalletAPI.API
```
5. Acesse o Swagger em `https://localhost:7001/swagger` 
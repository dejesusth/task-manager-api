# Task Manager API

API REST segura desenvolvida em C# com ASP.NET Core 8.

O projeto tem como objetivo aplicar, na prática, os conceitos de desenvolvimento de API's REST, autenticação de usuários, criptografia de senhas, operações HTTP (CRUD), segurança de rotas e persistência de dados utilizando o framework Entity Framework Core com banco de dados SQLite.

## Status do Projeto:

✅ Concluído

## 🏗️ Arquitetura

A aplicação foi desenvolvida separando as responsabilidades de recebimento de requisições, transferência de dados (DTOs) e comunicação com o banco:

```text
Cliente (Swagger / Navegador)
   ↓
Controller (Validações, JWT e Regras de Negócio)
   ↓
Entity Framework Core (Mapeamento ORM)
   ↓
SQLite (Banco de Dados Local)
```

### Responsabilidade de cada camada

* **Controller:** recebe as requisições HTTP, verifica os crachás de acesso (Tokens JWT), executa a lógica e retorna as respostas da API.
* **DTOs (Data Transfer Objects):** moldes de dados que garantem que o cliente envie e receba apenas as informações estritamente necessárias.
* **Entity Framework (AppDbContext):** realiza a ponte e a tradução entre o código C# e o banco de dados.
* **SQLite:** responsável pela persistência leve e local dos dados.

## 📚 Sobre o Projeto

Durante o desenvolvimento desta API, foram trabalhados conceitos fundamentais do mercado de trabalho, como:
- Criação de uma API REST
- Utilização de métodos HTTP
- Criação e Organização de Models e DTOs
- Autenticação e Autorização com JWT (JSON Web Tokens)
- Segurança e Hashing de senhas com BCrypt
- Relacionamento de tabelas (Usuário 1:N Tarefas)
- Proteção de dados (Usuário só acessa as próprias tarefas)
- Mapeamento Objeto-Relacional (ORM) com Entity Framework Core
- Code-First e Migrations
- Testes dos Endpoints utilizando Swagger
- Versionamento com Git e Github

## 🛠 Tecnologias Utilizadas

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Git](https://img.shields.io/badge/Git-F05032?style=for-the-badge&logo=git&logoColor=white)
![GitHub](https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)
![Visual Studio Code](https://img.shields.io/badge/Visual%20Studio%20Code-007ACC?style=for-the-badge&logo=visual-studio-code&logoColor=white)

### 📁 Estrutura do Projeto

Atualmente, o projeto está organizado da seguinte forma:
```
MinhaApi/
├── Controllers/
|   ├── AuthController.cs
│   └── TasksController.cs
│
├── Data/
|   └── AppDbContext.cs
|
├── DTOs/
|   ├── TaskCreateDTO.cs
│   ├── UserLoginDTO.cs
│   └── UserRegisterDTO.cs
├── Models/
|   ├── TaskItem.cs
│   └── User.cs
│
├── Program.cs/
├── task-manager.csproj
├── appsettings.json
└── taskmanager.db
```

### Controllers

Os Controllers são responsáveis por receber as requisições HTTP e definir quais ações devem ser executadas, exigindo ou não o Token de acesso.

Atualmente, o projeto possui:
- AuthController: Gerencia cadastro, login e geração de tokens.
- TasksController: Gerencia o CRUD de tarefas, bloqueado apenas para usuários autenticados.

### Models

#### User
Representa os clientes cadastrados na aplicação. A senha nunca é salva em texto limpo.

Possui:
- Id
- Name
- Email
- PasswordHash (Criptografado com BCrypt)
- Tasks (Lista de tarefas pertencentes ao usuário)

#### TaskItem
Representa uma tarefa a ser realizada.

Possui:
- Id
- Title
- Description
- IsCompleted
- CreatedAt
- UserId

Durante qualquer ação nas tarefas (Criar, Ler, Atualizar ou Deletar), o sistema intercepta o Token JWT, descobre quem é o usuário logado e garante que a operação ocorra estritamente nas tarefas de propriedade dele.

### 🌐 Endpoints

### 🔐 Autenticação (Auth)

|  Método  |      Endpoint       |          Descrição           |  Trancado? |
| -------- | ------------------- | ---------------------------- | ---------- |
|  `POST`  | `/api/Auth/register`| Cadastra um novo usuário     |     NÃO    |
|  `POST`  | ` /api/Auth/login`  | Autentica e gera o Token JWT |     NÃO    |
|  `GET`   |    `/api/perfil`    | Retorna os dados do Token    |     SIM    |

### 📋 Tarefas

| Método   |         Endpoint           |           Descrição            |  Trancado? |
| -------- | -------------------------- | ------------------------------ | ---------- |
| `GET`    |        `/api/Tasks`        | Lista as tarefas do usuário    |     SIM    |
| `POST`   |        `/api/Tasks`        | Cria uma nova tarefa           |     SIM    |
| `PATCH`  | `/api/Tasks/{id}/concluir` | Alterna status (concluído/não) |     SIM    |
| `DELETE` |      `/api/Tasks/{id}`     | Remove uma tarefa do usuário   |     SIM    |

### Banco de Dados

A aplicação utiliza SQLite para armazenamento de dados locais sem necessidade de instalar servidores pesados.

Toda a estrutura do banco (Tabelas, Colunas e Relacionamentos) foi gerada de forma automatizada através do recurso de Migrations do Entity Framework Core.

#### ⚙ Configuração do Banco e Segurança

A conexão com o banco e a Chave Secreta para a geração e validação dos tokens JWT ficam armazenadas em segurança no arquivo appsettings.json.

## ▶️ Como executar o projeto
### Pré-requisitos

Para executar o projeto, é necessário ter instalado:

- .NET 8 SDK;
- Git;
- Visual Studio Code ou IDE similar.
(Não é necessário instalar SGBD, o SQLite cria o arquivo de banco automaticamente na pasta).

### 📖 Swagger

O projeto utiliza Swagger configurado com OpenApiSecurityScheme para facilitar o teste das rotas protegidas.

Para testar as rotas com cadeado:
1. Crie uma conta em /api/Auth/register.
2. Faça o login em /api/Auth/login e copie o token gigante retornado.
3. Suba até o topo do Swagger e clique no botão verde Authorize.
4. Digite Bearer  (com espaço), cole o token na frente e clique em Authorize.
5. Agora as rotas protegidas de Tarefas estarão liberadas!

### 🔄 Desenvolvimento com Git e GitHub

O código-fonte do projeto é versionado utilizando Git e armazenado no GitHub.

O GitHub é utilizado como repositório remoto para armazenar e sincronizar as versões do projeto.

👨‍💻 Autor

Thiago Silva

Projeto pessoal desenvolvido a fim de praticar criação de API's.

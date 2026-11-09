# Sistema de Gestão de Eventos

Projeto acadêmico desenvolvido em grupo, utilizando ASP.NET Core MVC, com o objetivo de facilitar o gerenciamento de eventos acadêmicos, inscrições de participantes, controle de presença e emissão de certificados.

A aplicação oferece diferentes níveis de acesso, permitindo que diretores acadêmicos, coordenadores e alunos utilizem funcionalidades específicas de acordo com seus respectivos perfis.

## Funcionalidades

### Diretor Acadêmico
- Cadastro e gerenciamento de coordenadores acadêmicos.
- Acesso às funcionalidades administrativas do sistema.

### Coordenador
- Criação e gerenciamento de eventos.
- Definição de informações como título, descrição, data, horário e local.
- Disponibilização de código QR para confirmação de presença.
- Gerenciamento dos eventos cadastrados.

### Aluno
- Consulta de eventos disponíveis.
- Realização de inscrições em eventos.
- Consulta das próprias inscrições.
- Confirmação de presença por meio de código QR.
- Geração e download de certificados de participação.

## Tecnologias utilizadas

| Tecnologia | Aplicação |
|---|---|
| C# | Linguagem principal |
| ASP.NET Core MVC | Desenvolvimento da aplicação web |
| Entity Framework Core | Mapeamento objeto-relacional (ORM) |
| SQL Server | Banco de dados relacional |
| HTML5 | Estrutura das páginas |
| CSS3 | Estilização e identidade visual |
| JavaScript | Interatividade no front-end |
| Bootstrap | Componentes e responsividade |

## Arquitetura do projeto

A aplicação utiliza o padrão arquitetural **Model-View-Controller (MVC)**, separando as responsabilidades da aplicação em três camadas principais:

- **Models:** representação das entidades e dados do sistema.
- **Views:** interface visual e apresentação das informações.
- **Controllers:** processamento das requisições e comunicação entre as camadas.

O Entity Framework Core é utilizado para realizar a comunicação entre a aplicação e o banco de dados.

## Estrutura de diretórios

```text
SistemaEventos/
│
├── Controllers/
├── Data/
├── Migrations/
├── Models/
├── Properties/
├── Views/
├── wwwroot/
│
├── Program.cs
├── appsettings.json
├── SistemaEventos.csproj
└── SistemaEventos.sln
```

## Como executar o projeto

### Pré-requisitos

- Visual Studio 2022 ou compatível.
- .NET SDK correspondente à versão utilizada no projeto.
- Microsoft SQL Server.
- Entity Framework Core.

### Configuração

1. Clone este repositório.
2. Abra o arquivo `SistemaEventos.sln` no Visual Studio.
3. Configure a conexão com o SQL Server no arquivo `appsettings.json`.
4. Verifique a configuração do banco de dados e das migrations.
5. Restaure os pacotes NuGet necessários.
6. Execute a aplicação pelo Visual Studio.

## Interface do sistema

A interface utiliza uma identidade visual baseada em tons escuros, com predominância de preto e roxo, buscando proporcionar uma experiência visual consistente e intuitiva.

## Objetivo acadêmico

O projeto foi desenvolvido com o propósito de aplicar conhecimentos relacionados a:

- Desenvolvimento de aplicações web com C#.
- Arquitetura MVC.
- Persistência e manipulação de dados com Entity Framework Core.
- Integração com banco de dados SQL Server.
- Desenvolvimento de interfaces com HTML, CSS e JavaScript.
- Organização e desenvolvimento colaborativo de software.

---

**Projeto acadêmico — Sistema de Gestão de Eventos.**

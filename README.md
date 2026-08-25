# LMS — Learning Management System

Sistema de Gerenciamento de Aprendizagem completo desenvolvido com .NET 8 e Angular 17+, aplicando Clean Architecture, boas praticas de mercado e padroes usados em ambientes corporativos.

![CI](https://github.com/jeduardorj/Learning-Management-System---Sistema-de-Gest-o-de-Aprendizagem/actions/workflows/ci.yml/badge.svg)

## Tecnologias

### Backend
- .NET 8 / ASP.NET Core Web API
- Entity Framework Core 8 + SQL Server
- JWT Authentication + Refresh Token
- BCrypt para hash de senhas
- AutoMapper + FluentValidation
- Swagger / OpenAPI
- xUnit + Moq + FluentAssertions (28 testes)
- Docker + Docker Compose
- GitHub Actions (CI)

### Frontend
- Angular 17+ com Standalone Components
- Angular Signals para gerenciamento de estado
- Reactive Forms com validacao em tempo real
- Tailwind CSS
- Lazy Loading por rota
- Interceptors HTTP para JWT automatico

## Arquitetura

Clean Architecture com 4 camadas e regra de dependencia rigorosa:

LMS.Domain — Entidades, regras de negocio, interfaces (zero dependencias externas)
LMS.Application — Casos de uso, DTOs, validacoes, AutoMapper profiles
LMS.Infrastructure — EF Core, repositorios, JWT, BCrypt, Unit of Work
LMS.API — Controllers, middlewares, configuracao HTTP


O compilador C# garante a arquitetura: se alguem tentar usar EF Core no Domain, o projeto nao compila.

## Funcionalidades

### Administrador
- CRUD completo de cursos, modulos e aulas com ordenacao
- Dashboard com indicadores: alunos, cursos ativos, matriculas e certificados
- Cursos mais populares por numero de matriculas
- Ativar e desativar cursos
- Listagem de alunos matriculados por curso

### Aluno
- Cadastro e autenticacao com JWT + Refresh Token automatico
- Matricula em cursos ativos sem duplicidade
- Acompanhamento de progresso por aula com percentual
- Certificado emitido automaticamente ao concluir 100% do curso
- Validacao publica de certificado por codigo unico sem autenticacao
- Dashboard pessoal com progresso de todos os cursos

## Padroes e Praticas

| Padrao | Implementacao |
|---|---|
| Clean Architecture | 4 camadas com regra de dependencia |
| Repository Pattern | IBaseRepository generico e repositorios especializados |
| Unit of Work | Transacoes atomicas entre repositorios |
| Soft Delete | Filtro global no EF Core com HasQueryFilter |
| JWT + Refresh Token | Rotacao automatica e revogacao no logout |
| Middleware de excecoes | Erros padronizados 400/401/403/404/500 |
| Paginacao | Metadados completos em todas as listagens |
| Testes unitarios | Moq e FluentAssertions no Domain e Application |
| Testes de integracao | WebApplicationFactory com banco em memoria |
| CI/CD | GitHub Actions com build e testes em cada push |
| Docker | Multi-stage build e SQL Server em container |

## Como rodar localmente

### Pre-requisitos
- .NET 8 SDK: https://dotnet.microsoft.com/download
- Docker Desktop: https://www.docker.com/products/docker-desktop
- Node.js 18+: https://nodejs.org

### Backend

Terminal 1 — Sobe o SQL Server:
```bash
docker-compose up -d
```

Terminal 2 — Roda a API (migrations aplicadas automaticamente):
```bash
dotnet run --project src/LMS.API
```

Acesse o Swagger em http://localhost:5000

### Frontend

Terminal 3:
```bash
cd frontend/lms-angular
npm install
ng serve
```

Acesse http://localhost:4200

### Testes

```bash
dotnet test
```

Resultado esperado: 28 testes passando (19 unitarios e 9 de integracao).

## Credenciais de teste

Cadastre um usuario pela tela de registro em http://localhost:4200/register ou pelo endpoint POST /api/auth/register no Swagger.

Para promover para Admin, conecte no SSMS em localhost,1433 com usuario SA e senha LMS@SqlServer2024 e execute:

```sql
UPDATE Users SET Role = 1 WHERE Email = 'seu@email.com'
```

## Estrutura do projeto

LMS/
├── src/
│ ├── LMS.Domain/ Entidades, enums, interfaces de repositorio
│ ├── LMS.Application/ Servicos, DTOs, validators, AutoMapper profiles
│ ├── LMS.Infrastructure/ EF Core, repositorios concretos, JWT, BCrypt
│ └── LMS.API/ Controllers, middlewares, Program.cs
├── tests/
│ ├── LMS.UnitTests/ 19 testes unitarios Domain e Application
│ └── LMS.IntegrationTests/ 9 testes de integracao com banco em memoria
├── frontend/
│ └── lms-angular/ Angular 17+ com Tailwind CSS
├── Dockerfile Multi-stage build
├── docker-compose.yml SQL Server e API
└── .github/workflows/ci.yml Pipeline de CI


## Decisoes arquiteturais

**Por que Clean Architecture?**
Permite testar regras de negocio sem banco de dados e trocar infraestrutura sem impactar o dominio. A separacao e verificada pelo proprio compilador.

**Por que JWT com Refresh Token?**
Tokens de curta duracao (60 min) reduzem a janela de risco. O Refresh Token (7 dias) permite renovacao transparente sem re-autenticacao e pode ser revogado no logout.

**Por que Soft Delete com filtro global?**
Preserva historico de matriculas e progresso. O HasQueryFilter no EF Core garante que registros deletados nunca aparecem nas queries sem que o desenvolvedor precise lembrar de filtrar manualmente.

**Por que Unit of Work?**
Garante que operacoes relacionadas como salvar progresso e emitir certificado sejam confirmadas juntas ou nenhuma seja confirmada, evitando estados inconsistentes no banco.

**O que faria diferente em producao real?**
CQRS com MediatR para separar leitura e escrita, eventos de dominio para desacoplar a emissao de certificados do progresso, cache com Redis para o dashboard e secrets em Azure Key Vault em vez de variaveis de ambiente.

## Perguntas frequentes em entrevistas

**Como voce garantiu que a logica de negocio nao ficou acoplada ao banco de dados?**
O Domain define interfaces de repositorio. O Infrastructure implementa. O Application usa as interfaces, nunca as implementacoes. Se amanha trocar SQL Server por outro banco, Domain e Application nao mudam uma linha.

**Como voce implementou autorizacao por recurso?**
O userId e extraido do token JWT pelo ICurrentUserService, nunca do body da requisicao. Assim e impossivel um usuario fazer acoes em nome de outro mesmo que tente enviar um userId diferente no payload.

**O que e o problema N+1 e como voce evitou?**
E quando uma query em loop gera N queries adicionais no banco. Evitei usando Eager Loading com Include e ThenInclude para carregar relacionamentos em uma unica query, e CountAsync para contagens diretas no banco sem trazer dados para memoria.

**Como voce garantiria que codigo quebrado nao vai para producao?**
Pipeline de CI no GitHub Actions que roda build e todos os testes automaticamente em cada push. O merge de PRs so e permitido quando o pipeline passa.

## Autor

Jose Eduardo Rodrigues
GitHub: https://github.com/jeduardorj

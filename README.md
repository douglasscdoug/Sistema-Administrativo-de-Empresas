# Sistema Administrativo Empresarial
![Angular](https://img.shields.io/badge/Angular-17-red)
![.NET](https://img.shields.io/badge/.NET-8-purple)
![SQL Server](https://img.shields.io/badge/SQL_Server-Database-blue)

## Visão Geral

O Sistema Administrativo Empresarial é uma aplicação web desenvolvida para centralizar e organizar o cadastro de empresas, seus contatos e usuários, substituindo controles manuais como planilhas e anotações dispersas.

O sistema é voltado para uso interno, com foco em simplicidade, organização e facilidade de manutenção.

---

## 🚀 Tecnologias Utilizadas

### Backend
- ASP.NET Core
- Entity Framework Core
- SQLITE
- AutoMapper
- JWT Authentication

### Front-end
- Angular
- TypeScript
- Bootstrap
- ngx-bootstrap

---

## Funcionalidades

- Autenticação de usuários (login)
- Cadastro, edição e visualização de empresas
- Cadastro de endereço vinculado à empresa
- Cadastro de contato vinculado à empresa
- Cadastro e gerenciamento de usuários
- Listagem de empresas com busca e filtro por status

---

## 🔐 Funcionalidades Técnicas

- Autenticação com JWT
- Controle de acesso por perfil
- CRUD completo de empresas
- Paginação
- Filtros dinâmicos
- Validação de formulários
- Integração entre Angular e API REST
- Arquitetura em camadas
- DTOs e AutoMapper

---

## 🏗️ Arquitetura

O projeto foi desenvolvido utilizando arquitetura em camadas, promovendo separação de responsabilidades e facilidade de manutenção.

Estrutura:
- API
- Service
- Repository
- Domain
- DTOs

---

## 📸 Screenshots

### Tela de Login
<img src="./screenshots/login.png" width="800"/>

### Listagem de Empresas
<img src="./screenshots/empresas.png" width="800"/>

---

## ▶️ Como Executar o Projeto

### Backend

```bash
dotnet restore
dotnet ef database update
dotnet run
```

### Frontend

```bash
npm install
ng serve
```

## Cenário de Uso

Uma empresa que controla seus cadastros por meio de planilhas passa a utilizar o sistema para centralizar informações, padronizar dados e facilitar consultas e atualizações.

---

## 📬 Contato

- LinkedIn: https://www.linkedin.com/in/douglas-cabral-531a49165
- GitHub: https://github.com/douglasscdoug

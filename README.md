# 🚀 AcessosAPI
Este projeto é uma aplicação Web.API desenvolvida em ASP.NET Core, criada com o objetivo de simular um sistema de controle de documentos. Ele serve como um exercício prático para consolidar os conceitos aprendidos durante a formação ASP.Net Core na Alura. 🎓

## 🎯 Características Principais

- Implementação da estrutura MVC 🏗️
- Utilização do Entity Framework 🗃️
- Uso de Linq 🧮
- Documentação com Swagger 📚
- Autenticação via geração de token 🔑

Embora o sistema de controle de documentos seja hipotético e não necessite de funcionalidade completa, a aplicação inclui várias funcionalidades adicionais para simular um ambiente de produção real, incluindo:

- Cadastro de usuários 👥
- Cadastro de grupos 🏷️
- Cadastro de permissões 🛡️

## ⚙️ Regras de Negócio 

- As APIs de edição de documentos (CRUD) requerem autenticação de token e verificam se o grupo do usuário tem permissão para usá-las. 📝
- As permissões são verificadas individualmente para cada API do sistema. 🕵️‍♂️
- A autenticação é realizada através de token, e a autorização é baseada na política de uso do grupo do usuário. 🚦
- O token tem um prazo de validade de 10 minutos. ⏳
- O cadastro de usuários inclui dados cadastrais do usuário e o vínculo com uma tabela de relacionamento que informa a quais grupos o usuário pertence. 📋
- Cada usuário pode ler e alterar seus próprios dados cadastrais, mas o CRUD da tabela de usuários é permitido apenas para usuários com essa permissão. 🔄
- As permissões são concedidas pelo grupo para cada API, e o CRUD na tabela de grupos também requer permissão. 🎫
- A edição do relacionamento Usuário x Grupo x API só pode ser feita por usuários com essa permissão. 🔧
# Grupo2_Identity_Server

Esse é o servidor de Identidade do Grupo 2 onde ele foi desenvolvido em DotNet 7

### Ele foi desenvolvido em subdivisões de Pastas, onde nelas contém a pasta de Entities, DbContext, Interfaces, Repository, Dtos, Services, ModelViews e Controllers.

### Banco de Dados

Foi usado o Elephant(banco de Dados Postgres) para a persitencia dos dados.

## O Controller desse servidor são de : Login, Register, Valid-Token e Refresh-Token

O Controller criar no banco de Dados um usuario e uma senha com salt para mascarar a senha colocada por questões de seguranças.

## Autenticação 

Ela é feita por meio de Headers com a palavra chave Authorization e o metodo usado é o JWT Bearer.
Nele é gerado um token para o usuario fazer as requisições em outras APIs.

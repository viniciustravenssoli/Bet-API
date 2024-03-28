# Web API de Gerenciamento de Usuários e Apostas

Este projeto consiste em uma API web desenvolvida em C# utilizando o framework .NET 8 para gerenciamento de usuários e apostas. A API permite que os usuários se cadastrem, realizem apostas e, para administradores, oferece funcionalidades adicionais como definir o vencedor de uma aposta e realizar pagamentos.

## Funcionalidades Principais
- Cadastro de Usuários: Usuários podem se cadastrar na plataforma.
- Gerenciamento de Apostas: Usuários podem entrar em várias apostas, desde que estas não tenham sido pagas.
- Validação de Requisições: Utiliza Fluent Validation para validar as requisições enviadas à API.
- Papéis de Usuário: Admins têm permissões especiais, como definir o vencedor de uma aposta e realizar pagamentos.
- Controle de Saldo: Usuários normais só podem realizar apostas se tiverem saldo igual ou maior que o valor da aposta.
- Cálculo de Odd: Cada aposta possui um Odd que varia com base no montante de dinheiro nos times escolhidos e seus adversários.
- Créditos e Saldo: O saldo do usuário é creditado quando uma aposta é ganha. Em caso de perda, não há alterações no saldo.

## Tecnologias Utilizadas
- C# .NET 8: Utilizado como linguagem principal de desenvolvimento.
- Fluent Validation: Utilizado para realizar validações de requisições.
- Entity Framework (EF): Utilizado para o gerenciamento de usuários e dados da aplicação.

## Implementações Futuras
- [ ] Implementar sistema de cadastro de usuários.
- [ ] Desenvolver funcionalidade de gerenciamento de apostas.
- [ ] Utilizar Fluent Validation para validar requisições.
- [ ] Implementar controle de papéis de usuário (Admin vs. Normal).
- [ ] Integrar cálculo de Odd para apostas.
- [ ] Notificar por e-mail quando uma aposta é ganha.

### Nota:
A funcionalidade de notificação por e-mail quando uma aposta é ganha está planejada para ser implementada em uma versão futura do projeto.

## Instalação e Uso
1. Clone o repositório para sua máquina local.
2. Abra o projeto em sua IDE preferida.
3. Certifique-se de ter o .NET 8 SDK instalado em sua máquina.
4. Compile e execute o projeto.

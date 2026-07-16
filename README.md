# 💡 Sugestão de projeto

Esta é uma sugestão de projeto, para o caso de você não ter uma ideia própria. Se preferir seguir por outro caminho, pode ignorar essa pasta sem problema (veja o [README principal](../README.md) para as opções).

O código inicial da sugestão já está aqui nesta pasta (o projeto `TheatricalPlayersRefactoringKata`, em C#/.NET). Se você quiser, dá para refazer a mesma ideia em qualquer outra linguagem ou stack; o código aqui serve só como ponto de partida.

A ideia é um exercício de refatoração, design de software e implementação de novas funcionalidades em cima de uma aplicação que já existe: deixar ela mais testável, dar conta de novos requisitos e garantir que a solução é confiável por meio de testes.

## Solução implementada

A solução usa uma **Clean Architecture leve**, mantendo o projeto pequeno e
separando as responsabilidades em três áreas:

- `Domain`: entidades, modelo do extrato e regras de preço/créditos.
- `Application`: geração do extrato e contrato dos formatadores.
- `Infrastructure`: apresentação do extrato nos formatos texto e XML.

O `StatementPrinter` permanece como uma facade para preservar a API original.
O cálculo de cada gênero segue o padrão **Strategy** por meio de
`IPlayTypeCalculator`. Tragédia, comédia e histórico possuem implementações
independentes, e novos gêneros podem ser adicionados sem alterar o fluxo de
geração do extrato. O `StatementGenerator` produz um modelo neutro, que pode ser
convertido por `TextStatementFormatter` ou `XmlStatementFormatter` sem repetir
regras de negócio.

Os cálculos são cobertos por testes unitários com xUnit. As saídas completas de
texto e XML são protegidas por testes de aprovação com Verify.

Para executar:

```bash
dotnet test
```

Para iniciar a API REST:

```bash
dotnet run --project TheatricalPlayersRefactoringKata.Api
```

Com a API em execução, a documentação Swagger fica disponível em `/swagger`.
Os endpoints `POST /statements/text` e `POST /statements/xml` recebem os mesmos
dados e retornam o extrato no formato solicitado. Exemplos de requisição estão
em `TheatricalPlayersRefactoringKata.Api/TheatricalPlayersRefactoringKata.Api.http`.

Valores monetários são mantidos em centavos durante os cálculos, evitando perda
de precisão; a conversão ocorre somente na formatação da saída.

## 📜 Apresentação e estado atual da aplicação

Essa aplicação é usada por uma companhia de teatro para gerar extratos impressos a partir das faturas de seus clientes.

A companhia é contratada pelos clientes para múltiplas apresentações e a cobrança é feita baseada no número de linhas de cada peça apresentada, no tamanho da platéia e no gênero da peça. Atualmente os gêneros trabalhados pela companhia são tragédia e comédia.

Para cada apresentação são também gerados créditos, que são um tipo de mecanismo de fidelização que os clientes podem usar para obter descontos em futuras apresentações. O total de créditos gerados é também mostrado no extrato.

## ✨ Novas funcionalidades desejadas

A companhia de teatro pretende adicionar o gênero histórico ao seu repertório, então o software deve ser capaz de calcular os valores e créditos também para esse gênero. Provavelmente virão mais gêneros no futuro, então o design deve estar pronto para acomodar novos gêneros sem muita dificuldade.

Também desejam que o extrato possa ser gerado como um XML, além do formato de texto atualmente suportado. Novamente, é bom que o design facilite que futuramente esse extrato seja emitido em novos formatos, pois certamente é uma questão de tempo até surgir essa demanda.

## 🛠️ Especificação da atividade

Este é um exercício de refatoração. O design inicial da aplicação é pouco testável, portanto os únicos testes que a aplicação possui no momento são testes de aprovação (usando a biblioteca [Verify](https://github.com/VerifyTests/Verify)) para validar a saída final. É esperado que você torne o código mais testável e então adicione testes unitários que validem a aplicação de forma mais granular e que dêem segurança para futuras refatorações e para o acréscimo das novas funcionalidades.

Também serão observadas a abordagem para desenvolvimento da solução (Design Patterns, DDD, SOLID, etc.) e a arquitetura utilizada (Clean Architecture, Onion Architecture, etc.).

O projeto de testes possui três testes de aprovação.

* O teste TestStatementExampleLegacy, está passando no estado atual do código. Este teste servirá para te dar segurança das primeiras refatorações até que você escreva os testes unitários, mas ao final, com as funcionalidades novas implementadas, este teste se torna obsoleto.
* O teste TestTextStatementExample está implementado, porém não executa, pois o gênero histórico ainda não está implementado.
* O teste TestXmlStatementExample não está implementado e deve ser implementado por você e gerar a saída aprovada que está no projeto de testes.

O código dos testes pode ser refatorado, desde que a saída continue a mesma e os testes continuem cumprindo o mesmo propósito. É esperado que você implemente as novas funcionalidades pedidas para que todos os testes de aprovação passem.

## 🚀 Extras (Opcional)

Não é mandatório, mas de maneira opcional os seguintes requisitos poderão ser implementados:

* Implementar processamento assíncrono de extratos, os dados devem ser imputados, enfileirados, processados assincronicamente e gerar o XML resultante em um diretório
* API REST para expor os métodos para futuras integrações
  * Expor documentação da API por Swagger
* Persistência dos dados em um banco de dados para salvar o extrato com suas respectivas peças

## 📜 Regras de negócio

* O valor base para a cobrança de todas as peças é o número de linhas da peça dividido por 10
* O número de linhas da peça considerado para o cálculo do valor base deve ser forçado a estar no intervalo entre 1000 e 4000
* O valor para uma peça de tragédia é igual ao valor base caso a platéia seja menor ou igual a 30, somando mais 10.00 para cada espectador adicional a esses 30
* Para uma peça de comédia, o cálculo base é sempre somado a 3.00 por espectador. Além disso, se a platéia for maior que 20, o valor deve ser aumentado em 100.00 e deve se somar mais 5.00 por espectador adicional aos 20 de base
* Todas performances dão 1 crédito para cada espectador acima de 30, não valendo nenhum crédito para uma platéia menor ou igual a 30
* Existe um bônus de créditos de um quinto da platéia arredondados para baixo, exclusivo para peças de comédia
* As peças históricas são, por algum motivo, mais complicadas e têm o valor igual à soma dos valores correspondentes a uma peça de tragédia e uma de comédia
* A estrutura do XML deve seguir como referência a saída aprovada no teste de aprovação correspondente

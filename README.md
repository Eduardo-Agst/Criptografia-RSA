## Criptografia RSA em C#

### _Católica de Santa Catarina - N3 - Dupla: Eduardo Augusto Ferreira e João Vitor de Souza Tomio._

## Planejamento e operação
Para a criação do projeto utilizamos a linguagem C# por já termos um nivel maior de afinidade com a linguagem. O projeto foi desenvolvido com base na documentação oficial da Microsoft que pode ser acessada diretamente por [este link](https://docs.microsoft.com/pt-br/dotnet/standard/security/cryptography-model) e implementações próprias.

O projeto não possui uma arquitetura visto que o objetivo é um aplicativo de console que consiga fazer a criptografia e descriptografia de um arquivo. Organizamos a solução em três pastas para ficar mais visível, Na pasta ENCRIPTADOR temos o arquivo .csproj e sua respectiva classe principal com os detalhes para a execução do encriptador. Na pasta DECRIPTADOR temos o arquivo .csproj e sua respectiva classe principal com os detalhes para a execução do decriptador. E na pasta encrypt temos os arquivos genéricos utilizados na aplicação.

## Testes
Realizamos os testes executando o projeto através da IDE Visual Studio, os resultados dos testes foram positivos e conseguimos criptografar  descriptografar os arquivos. Tivemos um único impeditivo que alocar o arquivo criptografado em uma stream e despejar ele em um arquivo que já existia (Fizemos a criptografia uma vez e executamos o aplicativo novamente para ver o que acontecia) e o aplicativo retornou erro pois o arquivo de destino já existia e não conseguimos dar override no arquivo final. Tirando esse caso único que não conseguimos solucionar o aplicativo sobreviveu aos testes feitos.

## Conclusões

Criar o projeto em dupla serviu de grande aprendizado, explorar e analisar a documentação da microsoft para a criação do projeto foi muito produtivo possibilitando um melhor entendimento de como a criptografia RSA em si é montada pelos métodos comumente utilizados. Foram diversas horas de implementação até conseguirmos o resultado final e nós gostamos do resultado obtido!
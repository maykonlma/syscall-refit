# Syscall-Refit

🚀 **De Syscalls a Refit: Como as Abstrações Transformam o Desenvolvimento de Software**

Esse projeto é um estudo prático que explora diferentes formas de realizar requisições HTTP em C#, utilizando várias abordagens e implementações, começando com soluções de baixo nível e evoluindo para abstrações que tornam o desenvolvimento mais fácil e modular.

## Estrutura do Projeto

O projeto é composto por dois componentes principais:

1. **Server**: Uma API simples, construída com ASP.NET Core, expondo a rota `/weatherforecast`.
2. **Client**: Um projeto console que demonstra diferentes formas de fazer requisições HTTP para a API. As abordagens vão desde implementações mais diretas até soluções com mais abstrações.

## Abordagens Implementadas

💻 **Abordagens que implementei**:
- **SysCall (Linux e Windows)**: Implementações específicas para cada sistema operacional, utilizando chamadas diretas ao kernel para realizar operações de baixo nível.
- **Socket (Classe .NET)**
- **TCPClient (Classe .NET)**
- **HTTPClient (Classe .NET)**
- **RestSharp**
- **Refit**

A ideia central é mostrar como cada uma dessas abordagens, desde chamadas de sistema até bibliotecas como **Refit**, possuem um grau diferente de abstração, o que facilita o desenvolvimento e a reutilização do código.

## Aprendizado

🔧 **A escolha da abstração certa** pode simplificar muito o processo de desenvolvimento, tornando o código mais modular e flexível. Além disso, é sempre bom entender como essas abstrações funcionam por baixo dos panos para tomar decisões mais assertivas.

## Como Usar

1. Clone o repositório:
   ```
    git clone https://github.com/maykonlma/syscall-refit.git
   ```

2. Navegue até o diretório do projeto e execute o Server:
   ```
    cd Server
    dotnet run
   ```

3. Execute o Client para testar as diferentes implementações de requisição HTTP:
   ```
    cd Client
    dotnet run
   ```

4. Comando para visualizar os sockets no Linux
   ```
   tcpdump -i any 'tcp' port 5000
   ```

## Contribuições

Sinta-se à vontade para contribuir! Abra uma issue ou envie um pull request.

🔗 Confira o código no [GitHub](https://github.com/maykonlma/syscall-refit)

#CSharp #ASPNetCore #SoftwareDevelopment #API #Coding #DevCommunity #Tecnologia
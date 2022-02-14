# TestePusher
Aplicação para testar a conexão com o provedor de comunicação em tempo real "Pusher"

https://pusher.com/

O objetivo aqui é utilizar a plataforma para integrações em tempo real então estaremos escutando os eventos com uma rotina de backend utilizano .net core.

Exemplos de uso da api .net: https://github.com/pusher/pusher-websocket-dotnet

Consiste de uma aplicação asp.net core contendo:
 - Controller para enviar as mensagens de teste para o servidor do Pusher;
 - Hosted service que se conecta ao cluster do pusher e escuta um evento pré determinado.

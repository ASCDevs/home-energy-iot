const char *pageAuthenticate = R"raw(
<!DOCTYPE html>
<html onload="getEndMac()">
  <head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>Configurações do Device</title>

    <style>
      .center {
        text-align: center;
      }
      body {
        margin: 0;
        padding: 0;
        background-color: #b5de6aba;
        font-family: "Arial";
      }
      h2, p {
        text-align: center;
        color: #277582;
        padding: 20px;
      }
    </style>
  </head>
  <body>
    <div id="loader"></div>
        <h2>PowerMetrics - AUTENTICADO COM SUCESSO</h2><br>
        <h2 id="endmac">Aguarde, obtendo dados......</h2><br>
        <p>Remover SSID e Senha: <a href="/cleareeprom">ClearEEPROM</a></p>
        <p>Acessar Página de Cadastro: <a href="https://energy-iot.netlify.app">Home</a></p>
    </div>
    <script>
        let ttxt = document.getElementById("endmac");
        fetch("/mac")
            .then(resp => {
                return resp.text();
            })
            .then(dados => {
                ttxt.innerHTML = "";
                ttxt.innerHTML = "MacAddress: " + dados;
            });
    </script>

  </body>
</html>)raw";

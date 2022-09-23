// Defina a chave DIP para o Modo 1 (SW5, SW6 e SW7 estão ligados) com a alimentação desligada.
// Restaure a energia e carregue este Scketch

// apos gravar os 2 sketchs alterar para:
// Coloque a chave DIP no Modo 4 (SW1 e SW2 estão ligados)

// Funcionamento da Rede WIFI Esp

// STA "Station" - "Estação".
// AP "Access Point" - "Ponto de Acesso".

// No modo STA - permite que o ESP8266 se conecte a uma rede Wi-Fi (uma criada pelo seu roteador wireless).
// O ESP pode funcionar como um Client "cliente" ou como AP "ponto de acesso" ou até mesmo em ambas as configurações programadas.

// No modo AP - ele permite que você crie sua própria rede e tenha outros dispositivos conectados a ele como por exemplo:
// Um Smartphone, um Notebook, entre outros dispositivos que se conecte a uma rede Wireless.

// Resumo do funcionamento:
// 1 - Ao ligar pela primeira vez, o ESP estará em modo AP (AccessPoint), ou seja, ele terá um SSID e uma Senha para que o cliente se conecte a ele
//     e possa realizar a escolha da sua rede WIFI local.
// 2 - O Usuario ira selecionar a sua rede WIFI e inserir a senha, assim que o ESP conectar ele sairá do modo AP e entrará em modo STA (Estação).
//     No modo STA o ESP estará conectado na rede Wifi do usuario e ja iniciará o envio de dados capturados pelo sensor elétrico.
// 3 - Quando o ESP conecta na rede Wifi do usuário ele armazena os dados na EEPRON (não volátil) para que mesmo sendo desligado, assim que conectado
//     novamente na energia eletrica consiga realizar a autenticação.

#include <ArduinoWebsockets.h>
using namespace websockets;
WebsocketsClient client;  // https://github.com/gilmaimon/ArduinoWebsockets

#include <EEPROM.h>        // Lib para gravação da EEPROM
#include <Arduino_JSON.h>  // Lib dados JSON em Arduino

const int relePin = 4;  // Pino que o Relé esta ligado, funciona como porta Digital (ESP8266)

#include "index_html.h" pageLogin  // Imports das páginas HTML
#include "page_authenticate.h" pageAuthenticate

// USANDO ESP8266
#include <WiFiClient.h>
#include <ESP8266WebServer.h>   // https://github.com/esp8266/Arduino/tree/master/libraries/ESP8266WebServer

ESP8266WebServer server(80);    // Criando um objeto de servidor web que escuta solicitação HTTP na porta 80

const char *ssid = "PowerControl";  // O nome da rede Wi-Fi que será criada (Modo de funcionamento AP "Access Point")
const char *password = "21570199";  // A senha necessária para se conectar a ele, deixe em branco para uma rede aberta
String ssids = "";

int contadorTempoConexao = 0;    // Apos inserir o SSID e a SENHA este contador controla o tempo e verifica se conectou no WIFI ou nao

String receiveSSID = "";
String receivePASS = "";
IPAddress apIP(192, 168, 4, 1);  // IP para acessar o dispositivo antes de estar autenticado em sua Rede WIFI, quando em modo AP

String incomingString;           // Valor recebido através da serial pelo ESP, dado enviado pelo Arduino

// ------------------------------------------------------------ SETUP (Configurações Iniciais) ----------------------------------------------------
void setup() {

  Serial.begin(115200);  // Inicializando a saida Serial
  delay(3000);
  Serial.println('\n');

  pinMode(relePin, OUTPUT);    // Defindo o pino do Relé como Saída
  digitalWrite(relePin, LOW);  // Este pino funciona com lógica invertida, por isso inicializamos ele como LOW mas estará ligado

  // Este metodo é reponsável por iniciar o ESP em modo STA caso já tenha os dados da rede wifi Salvos ou como AP caso nao encontre.
  selectModeInitialization();

  // Configurações dos EndPoint
  server.on("/", HTTP_GET, handleRoot);              // EndPoint de Autenticação (Lista as redes Wifi encontradas)
  server.on("/senddata", postLogin);                 // EndPoint para autenticar na Rede WIFI
  server.on("/autenticado", autenticado);            // após autenticar direciono para esta pagina
  server.on("/desconectasta", desconectaSTA);        // EndPoint para desconectar da Rede WIFI
  server.on("/ssids", HTTP_GET, getssid);            // lista os ssids
  server.on("/cleareeprom", HTTP_GET, clearEEprom);  // Limpar dados de autenticação da Rede WIFI
  server.onNotFound(handleNotFound);

  server.enableCORS(true);
  server.begin();  // Iniciando o servidor
}

// ------------------------------------------------------------ METODO AUTENTICADO ------------------------------------------------------------------------------------
void autenticado() {
  server.send(200, "text/html", pageAuthenticate);
}

// ------------------------------------------------------------ METODO DESCONECTA STA ------------------------------------------------------------------------------------
void desconectaSTA() {
  server.send(200, "text/html", "Desconectado");
  WiFi.disconnect();  // desconecta WIFI STA (Station)
  iniciarModoAP();    // Inicia modo AP
}

// ------------------------------------------------------------ METODO LOGIN ------------------------------------------------------------------------------------
void handleRoot() {
  Serial.println(pageLogin);
  server.send(200, "text/html", pageLogin);
}

// ------------------------------------------------------------ METODO LISTAR SSIDS ------------------------------------------------------------------------------------
void getssid() {
  buscarSSIDS();
  server.send(200, "text/html", ssids);
}

// ------------------------------------------------------------ METODO GRAVAR DADOS NA EEPROM ------------------------------------------------------------------------------------
void clearEEprom() {
  int eepromOffset = 0;
  EEPROM.begin(512);  //Initialize EEPROM
  // Gravando
  String ssid = "";
  String pass = "";
  int str1AddrOffset = writeStringToEEPROM(eepromOffset, ssid);  // Grava na posição 0
  writeStringToEEPROM(str1AddrOffset, pass);                     // Obtem a posição do primeiro texto pra iniciar a gravação do 2
  EEPROM.commit();                                               // Store data to EEPROM
  server.send(200, "text/html", "Sucesso");

  iniciarModoAP();
}

// ------------------------------------------------------------ LOGIN ------------------------------------------------------------------------------------
void postLogin() {

  contadorTempoConexao = 0;

  if (server.method() != HTTP_POST) {
    server.send(405, "text/plain", "Metodo não permitido");
  } else {

    JSONVar myObject = JSON.parse(server.arg("plain"));

    if (myObject.hasOwnProperty("Ssid")) {
      receiveSSID = (const char *)myObject["Ssid"];
    }
    if (myObject.hasOwnProperty("Pass")) {
      receivePASS = (const char *)myObject["Pass"];
    }

    WiFi.begin(receiveSSID.c_str(), receivePASS.c_str());
    // Serial.setDebugOutput(true);
    Serial.println("--------------------------------");
    WiFi.printDiag(Serial);
    Serial.println("--------------------------------");

    Serial.print("Clientes Conectados.... - ");
    Serial.println(WiFi.softAPgetStationNum());

    while (WiFi.status() != WL_CONNECTED) {
      contadorTempoConexao++;
      Serial.println("Tentando Conectar");
      delay(500);
      if (contadorTempoConexao >= 10) {  // cada 500 milissegundos incremento 1 no contador, então apos 30x500(ms) = 15 Segundos tentando
        //Avisa sobre Erro de Conexão
        Serial.write("Erro ao tentar conectar na rede WiFi");
        server.send(404, "text/plain", "Erro");
        WiFi.disconnect();
        return;
      }
    }

    if (WiFi.status() == WL_CONNECTED) {

      server.send(200, "text/plain", WiFi.localIP().toString().c_str());

      int eepromOffset = 0;
      EEPROM.begin(512);  //Initialize EEPROM
      // Gravando
      String ssid = receiveSSID;
      String pass = receivePASS;
      int str1AddrOffset = writeStringToEEPROM(eepromOffset, ssid);  // Grava na posição 0
      writeStringToEEPROM(str1AddrOffset, pass);                     // Obtem a posição do primeiro texto pra iniciar a gravação do 2
      EEPROM.commit();                                               //Store data to EEPROM

      delay(3000);

      Serial.print("\nConectado! Endereço IP: \t");
      Serial.println(WiFi.localIP());

      // Desconecta o Modo AP e conecta em MODO STA
      WiFi.softAPdisconnect(true);
      Serial.println("Desconectando do Modo AP");
      delay(1000);
      initializeWebSocket();
    }
  }
}

void handleNotFound() {
  String message = "File Not Found\n\n";
  message += "URI: ";
  message += server.uri();
  message += "\nMethod: ";
  message += (server.method() == HTTP_GET) ? "GET" : "POST";
  message += "\nArguments: ";
  message += server.args();
  message += "\n";
  for (uint8_t i = 0; i < server.args(); i++) {
    message += " " + server.argName(i) + ": " + server.arg(i) + "\n";
  }
  server.send(404, "text/plain", message);
}

void buscarSSIDS() {
  int n = WiFi.scanNetworks();
  ssids = "";
  for (int i = 0; i < n; i++) {
    ssids = ssids + WiFi.SSID(i) + "<br>";
  }
}

void loop() {
  //ledPiscando();  // indica que esta desconectado

  server.handleClient();

  if (WiFi.status() == WL_CONNECTED) {

    Serial.println("Socket");
    client.poll();  // Para continuar recebendo mensagens

    // Enviando a mensagem para o servidor
    client.send("server>energyvalue>" + String(random(0, 1024)));//getReadDataFromArduino()); // atraves da função recebemos o valor pela serial do arduino e enviamos ao endpoint
    //client.send(WiFi.localIP().toString().c_str()); // Ip do ESP na rede do Cliente
    delay(1000);
  }
}

void selectModeInitialization() {

  EEPROM.begin(512);
  String ssidEEPROM;
  String passEEPROM;
  int newStr1AddrOffset = readStringFromEEPROM(0, &ssidEEPROM);
  int newStr2AddrOffset = readStringFromEEPROM(newStr1AddrOffset, &passEEPROM);

  if (ssidEEPROM.length() > 0 && passEEPROM.length() > 0)  // Se houver dado gravado é o SSID E PASS usamos pra autenticar na REDE Wifi do usuário
  {
    WiFi.begin(ssidEEPROM, passEEPROM);  // Inicia WiFi com os dados preenchidos (Modo STA)
    delay(1000);                         // aguarda 1 segundo
    Serial.print("Iniciando conexão modo STA");

    while (WiFi.status() != WL_CONNECTED) {

      contadorTempoConexao++;

      Serial.println("Tentando Conectar usando dados da EEPROM");
      Serial.println(ssidEEPROM);
      Serial.println(passEEPROM);
      Serial.println(contadorTempoConexao);
      delay(500);
      if (contadorTempoConexao >= 30) {  // cada 500 milissegundos incremento 1 no contador, então apos 30x500(ms) = 15 Segundos tentando
        //Avisa sobre Erro de Conexão
        Serial.write("Erro ao tentar conectar na rede WiFi");
        server.send(404, "text/plain", "Erro");
        clearEEprom();
        WiFi.disconnect();
        iniciarModoAP();  // apos os 15 segundos se não conectar entra em MMODO AP
                          // break;
      }
    }
    if (WiFi.status() == WL_CONNECTED) {
      initializeWebSocket();
    }
  } else {
    iniciarModoAP();  // Caso não encontre dados na EEPROM inicia em modo AP
  }

  EEPROM.end();  //Fecha a EEPROM.
}

void iniciarModoAP() {
  WiFi.disconnect();

  WiFi.softAPConfig(apIP, apIP, IPAddress(255, 255, 255, 0));  // (local_IP, gateway, subnet)
  WiFi.softAP(ssid, password);                                 // INICIA O ACCESS POINT (AP)

  Serial.println("Access Point (AP) \t");
  Serial.print(ssid);
  Serial.println("\nIniciando");
  Serial.print("IP:\t");
  Serial.println(WiFi.softAPIP());
}

int writeStringToEEPROM(int addrOffset, const String &strToWrite) {
  byte len = strToWrite.length();
  EEPROM.write(addrOffset, len);
  for (int i = 0; i < len; i++) {
    EEPROM.write(addrOffset + 1 + i, strToWrite[i]);
  }
  return addrOffset + 1 + len;
}

int readStringFromEEPROM(int addrOffset, String *strToRead) {
  int newStrLen = EEPROM.read(addrOffset);
  char data[newStrLen + 1];
  for (int i = 0; i < newStrLen; i++) {
    data[i] = EEPROM.read(addrOffset + 1 + i);
  }
  data[newStrLen] = '\0';
  *strToRead = String(data);
  return addrOffset + 1 + newStrLen;
}

String getReadDataFromArduino() {

  if (Serial.available() > 0) {
    incomingString = Serial.readString();
    return incomingString;
  }
}

void initializeWebSocket() {

  // Setup Callback - Message
  client.onMessage(onMessageCallback);  // Aguardando as mensagens do servidor

  // Setup Callback - Event
  client.onEvent(onEventsCallback);

  // Conectando ao servidor
  client.connect("wss://monitoring-iot-devices.herokuapp.com/consocket");
  //client.connect("ws://192.168.15.70:9123");
}

void onMessageCallback(WebsocketsMessage message) {

  Serial.print("Mensagem recebida Socket: ");
  Serial.println(message.data());
  if (message.data() == "OFF")
    digitalWrite(relePin, HIGH);  // Caso receba atraves do Socket OFF desliga o Relé
  if (message.data() == "ON")
    digitalWrite(relePin, LOW);  // Caso receba atraves do Socket ON liga o Relé
}

void onEventsCallback(WebsocketsEvent event, String data) {
  if (event == WebsocketsEvent::ConnectionOpened) {
    Serial.println("Conexão Aberta");

  } else if (event == WebsocketsEvent::ConnectionClosed) {
    Serial.println("Conexão Fechada");

  } else if (event == WebsocketsEvent::GotPing) {
    Serial.println("Ping");

  } else if (event == WebsocketsEvent::GotPong) {
    Serial.println("Pong");
  }
}
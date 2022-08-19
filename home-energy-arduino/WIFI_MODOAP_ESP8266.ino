#include <EEPROM.h>
#include <Arduino_JSON.h>

#include "index_html.h" pageLogin
#include "page_authenticate.h" pageAuthenticate

// USANDO ESP8266
#include <WiFiClient.h>
#include <ESP8266WebServer.h> ESP8266
#include <ESP8266mDNS.h> ESP8266

ESP8266WebServer server(80);

const char *ssid      = "PowerControl";   // O nome da rede Wi-Fi que será criada
const char *password  = "21570199";       // A senha necessária para se conectar a ele, deixe em branco para uma rede aberta
String ssids          = "";

#define LED_BUILTIN 2

boolean conectou = false;                // mudar o valor quando conectado
int contadorTempoConexao = 0;            // Apos inserir o SSID e a SENHA este contador controla o tempo e verifica se conectou no WIFI ou nao

String receiveSSID = "";
String receivePASS = "";
IPAddress apIP(192, 168, 4, 1);

// ------------------------------------------------------------ SETUP ------------------------------------------------------------------------------------
void setup() {

  pinMode(LED_BUILTIN, OUTPUT);

  Serial.begin(115200);
  delay(3000);
  Serial.println('\n');

  selectModeInitialization();
    
  // Resolução de IP para nome, basta colocar no navegador powermetrics.local
  if (MDNS.begin("powermetrics.local")) {
     MDNS.addService("http", "tcp", 80);
     Serial.println("MDNS iniciado - http://powermetrics.local");
  }

  server.on("/", HTTP_GET, handleRoot);
  server.on("/senddata", postLogin);
  server.on("/autenticado", autenticado);     // após autenticar direciono para esta pagina
  server.on("/desconectasta", desconectaSTA);
  server.on("/ssids", HTTP_GET, getssid);     // lista os ssids
  server.on("/cleareeprom", HTTP_GET, clearEEprom);     // lista os ssids
  server.onNotFound(handleNotFound);

  server.enableCORS(true);
  server.begin();
}

// ------------------------------------------------------------ METODO AUTENTICADO ------------------------------------------------------------------------------------
void autenticado() {
  server.send(200, "text/html", pageAuthenticate);
}

// ------------------------------------------------------------ METODO DESCONECTA STA ------------------------------------------------------------------------------------
void desconectaSTA() {
  server.send(200, "text/html", "Desconectado");
  WiFi.disconnect(); // desconecta WIFI STA
  iniciarModoAP();   // Inicia modo AP
  conectou = false;
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

// ------------------------------------------------------------ METODO LISTAR SSIDS ------------------------------------------------------------------------------------
void clearEEprom () {
      int eepromOffset = 0;
      EEPROM.begin(512);  //Initialize EEPROM
      // Gravando
      String ssid = "";
      String pass = "";
      int str1AddrOffset = writeStringToEEPROM(eepromOffset, ssid); // Grava na posição 0
      writeStringToEEPROM(str1AddrOffset, pass);                    // Obtem a posição do primeiro texto pra iniciar a gravação do 2
      EEPROM.commit();    //Store data to EEPROM
      server.send(200, "text/html", "Sucesso");

      iniciarModoAP();
      conectou = false; 
}

// ------------------------------------------------------------ LOGIN ------------------------------------------------------------------------------------
void postLogin() {

  contadorTempoConexao = 0;

  if (server.method() != HTTP_POST) {
    server.send(405, "text/plain", "Metodo não permitido");
  } else {

    JSONVar myObject = JSON.parse(server.arg("plain"));

    //if (JSON.typeof(myObject) == "undefined") {
    //   Serial.println("Erro Parsing Json!");
    //   return;
    // }

    if (myObject.hasOwnProperty("Ssid")) {
      receiveSSID = (const char*)myObject["Ssid"];
    }
    if (myObject.hasOwnProperty("Pass")) {
      receivePASS = (const char*)myObject["Pass"];
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
      if (contadorTempoConexao >= 10) { // cada 500 milissegundos incremento 1 no contador, então apos 30x500(ms) = 15 Segundos tentando
        //Avisa sobre Erro de Conexão
        Serial.write("Erro ao tentar conectar na rede WiFi");
        server.send(404, "text/plain", "Erro");
        WiFi.disconnect();
        return;
      }
    }

    if (WiFi.status() == WL_CONNECTED) {

      server.send(200, "text/plain", WiFi.localIP().toString().c_str());
      conectou = true;
      delay(2000);

      int eepromOffset = 0;
      EEPROM.begin(512);  //Initialize EEPROM
      // Gravando
      String ssid = receiveSSID;
      String pass = receivePASS;
      int str1AddrOffset = writeStringToEEPROM(eepromOffset, ssid); // Grava na posição 0
      writeStringToEEPROM(str1AddrOffset, pass);                    // Obtem a posição do primeiro texto pra iniciar a gravação do 2
      EEPROM.commit();    //Store data to EEPROM
    
      Serial.print("\nConectado! Endereço IP: \t");
      Serial.println(WiFi.localIP());

      // Desconecta o Modo AP e conecta em MODO STA
      WiFi.softAPdisconnect(true);
      Serial.println("Desconectando do Modo AP");
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
  ledPiscando();  // indica que esta desconectado
  
  server.handleClient();
}

void selectModeInitialization(){

  EEPROM.begin(512);
  String ssidEEPROM;
  String passEEPROM;
  int newStr1AddrOffset = readStringFromEEPROM(0, &ssidEEPROM);
  int newStr2AddrOffset = readStringFromEEPROM(newStr1AddrOffset, &passEEPROM);

  if (ssidEEPROM.length() > 0 && passEEPROM.length() > 0)   // Se houver dado gravado é o SSID E PASS usamos pra autenticar na REDE Wifi do usuário
   {
     WiFi.begin(ssidEEPROM, passEEPROM); //Inicia WiFi com os dados preenchidos 
     delay(1000); // aguarda 1 segundo
     Serial.print("Conectando"); 

    while (WiFi.status() != WL_CONNECTED) {
      
      contadorTempoConexao++;
      
      Serial.println("Tentando Conectar usando dados da EEPROM");          
      Serial.println(ssidEEPROM);
      Serial.println(passEEPROM);
      
      delay(500);
      if (contadorTempoConexao >= 30) { // cada 500 milissegundos incremento 1 no contador, então apos 30x500(ms) = 15 Segundos tentando
        //Avisa sobre Erro de Conexão
        Serial.write("Erro ao tentar conectar na rede WiFi");
        server.send(404, "text/plain", "Erro");
        WiFi.disconnect();
        iniciarModoAP();   // apos os 15 segundos se não conectar entra em MMODO AP
      }
     }       
      if (WiFi.status() == WL_CONNECTED){
        conectou = true;         
      }
   } else {
      iniciarModoAP();    
   }

   EEPROM.end();//Fecha a EEPROM. 

}

void iniciarModoAP() {
  WiFi.softAPConfig(apIP, apIP, IPAddress(255, 255, 255, 0));
  WiFi.softAP(ssid, password);             // INICIA O ACCESS POINT

  Serial.println("Access Point (AP) \t");
  Serial.print(ssid);
  Serial.println("\nIniciando");

  Serial.print("IP:\t");
  Serial.println(WiFi.softAPIP());         // Send the IP address of the ESP8266 to the computer
}

void ledPiscando() {

  if (!conectou) {
    digitalWrite(LED_BUILTIN, LOW);
    delay(500);
    digitalWrite(LED_BUILTIN, HIGH);
    delay(500);
  } else {
    digitalWrite(LED_BUILTIN, LOW);
    delay(500);
  }
}

int writeStringToEEPROM(int addrOffset, const String &strToWrite)
{
  byte len = strToWrite.length();
  EEPROM.write(addrOffset, len);
  for (int i = 0; i < len; i++)
  {
    EEPROM.write(addrOffset + 1 + i, strToWrite[i]);
  }
  return addrOffset + 1 + len;
}
int readStringFromEEPROM(int addrOffset, String *strToRead)
{
  int newStrLen = EEPROM.read(addrOffset);
  char data[newStrLen + 1];
  for (int i = 0; i < newStrLen; i++)
  {
    data[i] = EEPROM.read(addrOffset + 1 + i);
  }
  data[newStrLen] = '\0'; // !!! NOTE !!! Remove the space between the slash "/" and "0" (I've added a space because otherwise there is a display bug)
  *strToRead = String(data);
  return addrOffset + 1 + newStrLen;
}

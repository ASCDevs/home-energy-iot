#include <ArduinoWebsockets.h>
#include <ESP8266WiFi.h>

const char* ssid = "NOME DO WIFI";
const char* password = "SENHA";

using namespace websockets;

WebsocketsClient client;   // https://github.com/gilmaimon/ArduinoWebsockets

void onMessageCallback(WebsocketsMessage message) {
    Serial.print("Mensagem do Servidor: ");
    Serial.println(message.data());
}

void onEventsCallback(WebsocketsEvent event, String data) {
    if(event == WebsocketsEvent::ConnectionOpened) {
        Serial.println("Conexão Aberta");
        
    } else if(event == WebsocketsEvent::ConnectionClosed) {
        Serial.println("Conexão Fechada");

    } else if(event == WebsocketsEvent::GotPing) {
        Serial.println("Ping");

    } else if(event == WebsocketsEvent::GotPong) {
        Serial.println("Pong");
    }
}

void setup() {
    Serial.begin(9600);

    // Conectando no WiFi
    WiFi.begin(ssid, password);

    // Aguardando enquanto conecta-se ao WiFi
    while(WiFi.status() != WL_CONNECTED) {
        Serial.print(".");
        delay(500);
    }

    // Setup Callback - Message
    client.onMessage(onMessageCallback);  // Aguardando as mensagens do servidor

    // Setup Callback - Event
    client.onEvent(onEventsCallback);

    // Conectando ao servidor
    client.connect("wss://monitoring-iot-devices.herokuapp.com/consocket");
}

void loop() {
    client.poll(); // Para continuar recebendo mensagens

    String strNumber = String(random(0, 1024));  // Simulando a quantidade de Volts

    // Enviando a mensagem para o servidor
    client.send("server>energyvalue>" + strNumber);
}

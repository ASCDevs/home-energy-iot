// Defina a chave DIP para o Modo 1 (SW3 e SW4 estão ligados) com a alimentação desligada.
// Restaure a energia e carregue este Scketch

// apos gravar os 2 sketchs alterar para:
// Coloque a chave DIP no Modo 4 (SW1 e SW2 estão ligados)

// Documentação https://www.notion.so/Detalhamento-El-trico-ACS712-Arduino-c805801b135b4827992d58dd71bef45f

float tensaoAC = 110.000;  // Defina aqui a tensão AC da rede (110V ou 220V)
float correnteRms;         // Armazenamos a corrente medida pelo sensor
float potencia;            // Armazenamos a potência a partir da fórmula P = V x I
float energia = 0;         // Armazenamos a energia gasta a partir da fórmula E = P x t

// Dados do Sensor
// Para 30A, sensibilidade = 0.066;
// Para 20A, sensibilidade = 0.100;
// Para 5A,  sensibilidade = 0.185;
float sensibilidade = 0.185;

void setup() {
  Serial.begin(115200);
}

void loop() {
  correnteRms = currentCalculate(filterMedia());  // Calcula a corrente

  potencia = abs(correnteRms * tensaoAC);  // Calcula a potência a partir da fórmula P = V x I. A potência esta em valor absoluto pela função "abs"
  energia += (potencia * 1.2 / 1000);      // Calcula a energia gasta até o momento a partir da fórmula E = P x t.

  // Valor de Tensão
  Serial.print("Tensao:    ");
  Serial.print(tensaoAC, 3);
  Serial.println(" V");
  // Valor de Corrente
  Serial.print("Corrente:  ");
  Serial.print(correnteRms, 3);
  Serial.println(" A");
  // Valor da Potência
  Serial.print("Potencia:  ");
  Serial.print(potencia, 3);
  Serial.println(" W");
  // Valor da energia gasta (Acumulado)
  Serial.print("Energia:   ");
  Serial.print(energia, 3);
  Serial.println(" kJ");
  Serial.println("------------------------");

  delay(1000);
}

// "currentCalculate" converte o sinal recebido pelo arduino em A0 num valor de corrente.
float currentCalculate(int sinalSensor) {
  return (float)(sinalSensor) * (5.000) / (1023.000 * sensibilidade);
}

// "filterMedia", Filtro da média, para tentarmos reduzir o ruido do sinal
int filterMedia() {

  long somaDasCorrentes = 0;
  long mediaDasCorrentes;

  for (int i = 0; i < 1000; i++) {
    somaDasCorrentes += pow((analogRead(A0) - 508), 2);
    delay(1);
  }

  mediaDasCorrentes = sqrt(somaDasCorrentes / 1000);  //Calcula a média quadrática da corrente

  if (mediaDasCorrentes == 1)
    return 0;
  return mediaDasCorrentes;
}
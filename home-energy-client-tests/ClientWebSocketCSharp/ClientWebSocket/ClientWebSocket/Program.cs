// See https://aka.ms/new-console-template for more information

using System.Net.WebSockets;
using System.Text;

Teste.Init();

static class Teste {

    public static async Task DoClientWebSocket()
    {
        using (ClientWebSocket ws = new ClientWebSocket())
        {
            Uri serverUri = new Uri("wss://monitoring-iot-devices.herokuapp.com/consocket");

            //Timeout
            //var source = new CancellationTokenSource();
            //source.CancelAfter(5000);

            await ws.ConnectAsync(serverUri, CancellationToken.None); //, source.Token
            while (true)
            {
                Random random = new Random();
                int fakeEnergyValue = random.Next(1,40);
                
                string value = "server>energyvalue>" + Convert.ToString(fakeEnergyValue);
                byte[] bytes = Encoding.ASCII.GetBytes(value);

                await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                //Console.WriteLine("[enviado] "+value);
                Thread.Sleep(1000);
            }
        }
    }
    public static void Init()
    {
        var taskWebConnect = Task.Run(() => DoClientWebSocket());
        taskWebConnect.Wait();
    }

}


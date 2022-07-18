class DeviceInterface {
    constructor() {
        this.urlSocket = "wss://localhost:7056/consocket";
        this.setFunctions();
        this.setEvents();
    }

    setEvents() {
        var Self = this;
        $("#btn-start").click(() => {
            Self.makeConnection();
        });

        $("#btn-generate-volts").click(() => {
            Self.initEnergyValues();
        })

        $("#btn-stop-volts").click(() => {
            Self.stopEnergyValues();
        })
    }

    setFunctions() {
        var Self = this;

        this.generateFakeValue = function () {
            //parseFloat(((Math.random() * 30)+1).toFixed(2))
            return ((Math.random() * 30) + 1).toFixed(2);
        }

        this.initEnergyValues = function () {
            Self.intervalEnergyValues = setInterval(function () {
                var value = Self.generateFakeValue();
                $("#value-volts").text(value)
            },800)
        }

        this.stopEnergyValues = function () {
            $("#value-volts").text("0")
            clearInterval(Self.intervalEnergyValues)
        }

    }

    makeConnection() {
        var Self = this;
        //var nomeConexao = prompt("Dê um nome para a conexão. ");
        var socket = new WebSocket(Self.urlSocket);
        var logArea = document.getElementById("log-server");

        socket.onopen = function (e) {
            Self.socketConnection = socket;
            let log = "<p>[open] Connection established</p>";
            logArea.insertAdjacentHTML('afterend', log);
            socket.send(nomeConexao + " entroucon");
            socket.send("server>idfinal>" + nomeConexao);
        };

        socket.onmessage = function (event) {
            let log = "<p>[message] " + event.data + "</p>";
            logArea.insertAdjacentHTML('afterend', log);
        }

        socket.onclose = function (event) {
            if (event.wasClean) {
                let log = "<p>[close] " + event.code + " - reason = " + event.reason + "</p>";
                logArea.insertAdjacentHTML('afterend', log);
            } else {
                let log = "<p>[close] connection died</p>";
                logArea.insertAdjacentHTML('afterend', log);
            }
        }

        socket.onerror = function (error) {
            let log = "<p>[close] " + error.message + "</p>";
            logArea.insertAdjacentHTML('afterend', log);
        }


        function SendMessageServer(message) {
            socket.send(message)
        }
    }

    
}



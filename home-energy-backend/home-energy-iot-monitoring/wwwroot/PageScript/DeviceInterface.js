class DeviceInterface {

    constructor() {
        this.DEV = "wss://localhost:7056/consocket";
        this.LOCAL = "wss://localhost:7722/consocket";
        this.PROD = "wss://monitoring-iot-devices.herokuapp.com/consocket";
        this.urlSocket = this.DEV;
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
            Self.sendMessageSocket("server>energyvalue>0");
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
                Self.socketConnection.send("server>energyvalue>" + value)
            },800)
        }

        this.stopEnergyValues = function () {
            $("#value-volts").text("0")
            clearInterval(Self.intervalEnergyValues)
        }

        this.sendMessageSocket = function (message) {
            Self.socketConnection.send(message)
        }

        this.setTimer = function (time) {
            console.log(`[Implementar timer visual para ${time} milisegundos]`)
        }

        this.HandleAction = function (action) {
            let splitAction = action.split(">");

            if (splitAction[1] == "stopenergy") {
                Self.stopEnergyValues();
                Self.sendMessageSocket("server>energyvalue>0");
            } else if (splitAction[1] == "timerenergy") {
                //valor recebido é em segundos
                let valueReceived = parseInt(splitAction[2]);
                let time = valueReceived * 1000; //transformado para milissegundos
                Self.stopEnergyValues();
                Self.sendMessageSocket("server>energyvalue>0");
                setTimeout(() => Self.initEnergyValues(), time);
                Self.setTimer(time);

            }
        }

    }

    makeConnection() {
        var Self = this;
        //var nomeConexao = prompt("Dê um nome para a conexão. ");
        var socket = new WebSocket(Self.urlSocket);
        var logArea = document.getElementById("log-server");

        socket.onopen = function (e) {
            Self.socketConnection = socket;
            Self.idConnection = socket.idConnection;
            let log = "<p>[open] Connection established</p>";
            logArea.insertAdjacentHTML('afterend', log);
            //socket.send(nomeConexao + " entroucon");
            //socket.send("server>idfinal>" + nomeConexao);
        };

        socket.onmessage = function (event) {
            if (event.data.includes("client>")) {
                Self.HandleAction();
            } else {
                let log = "<p>[message] " + event.data + "</p>";
                logArea.insertAdjacentHTML('afterend', log);
            }
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
            let log = "<p>[close] error - " + error.message + "</p>";
            logArea.insertAdjacentHTML('afterend', log);
        }
    }

    
}



class DeviceInterface {

    constructor(urlFromConfig, urlCheckFromConfig,flApiSaveValue) {
        this.flApiSaveValue = flApiSaveValue;
        this.urlCheck = urlCheckFromConfig;
        this.urlSocket = urlFromConfig;
        this.setFunctions();
        this.makeInputDeviceField();
        this.InitialSettings();
        this.setEvents();
    }

    setEvents() {
        var Self = this;
        $("#btn-start").click(() => {
            Self.makeConnection();
        });

        $("#btn-generate-volts").click(() => {
            Self.initEnergyValues();
            Self.sendMessageSocket("server>confirmcontinue");
        })

        $("#btn-stop-volts").click(() => {
            Self.stopEnergyValues();
            Self.sendMessageSocket("server>energyvalue>0");
            Self.sendMessageSocket("server>confirmstop");
        })

        $("#btn-salvar-deviceid").click(() => {
            Self.defineDeviceId();
        })
    }

    setFunctions() {
        var Self = this;

        this.HasDeviceId = function (deviceID) {
            if (Self.urlCheck.trim() != "" && Self.urlCheck != undefined && Self.urlCheck != null) {
                //Implementar chamada ajax
            }
            return false;
        }

        this.defineDeviceId = function () {
            let textDeviceID = $("#txt-deviceid").val();
            if (textDeviceID != null && textDeviceID != undefined && textDeviceID.trim() != "") {
                let isValidDevice = true;

                if (Self.flApiSaveValue) {
                    //if (!Self.HasDeviceId(textDeviceID)) {
                    //    isValidDevice = false;
                    //}
                }

                if (isValidDevice) {
                    Self.deviceId = textDeviceID;
                    $("#area-deviceid").html("");
                    $("#device-id").text(Self.deviceId)

                    $("#btn-start").prop("disabled", false)
                    $("#btn-generate-volts").prop("disabled", false)
                    $("#btn-stop-volts").prop("disabled", false)
                } else {
                    alert("O DeviceID Informado é inválido ou não existe")
                }
            } else {
                console.log("[Erro] Informação do ID do dispositivo é inválida")
            }
        }

        this.InitialSettings = function () {
            $("#btn-start").prop("disabled", true)
            $("#btn-generate-volts").prop("disabled", true)
            $("#btn-stop-volts").prop("disabled", true)
        }

        this.makeInputDeviceField = function () {
            let txtHtml = '<div class="mb-6">'
            txtHtml += '<label class="block text-gray-700 text-sm font-bold mb-2" for="txt-deviceid">Device id:</label>'
            txtHtml += '<input id="txt-deviceid" class="mb-3 shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" type="text" placeholder="ID do Dispositivo" />'
            txtHtml += '<button type="button" id="btn-salvar-deviceid" class="rounded bg-sky-500 p-2 text-sm font-bold text-white hover:bg-sky-700 disabled:cursor-not-allowed">Salvar</button>'
            txtHtml += '</div>'

            $("#area-deviceid").append(txtHtml);
        }

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

        this.initOkConfirmation = function () {
            Self.intervalConfirmations = setInterval(function () {
                Self.socketConnection.send("server>ok");
            },1000)
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
                Self.sendMessageSocket("server>confirmstop");
            } else if (splitAction[1] == "timerenergy") {
                //valor recebido é em segundos
                let valueReceived = parseInt(splitAction[2]);
                let time = valueReceived * 1000; //transformado para milissegundos
                Self.stopEnergyValues();
                Self.sendMessageSocket("server>energyvalue>0");
                setTimeout(() => Self.initEnergyValues(), time);
                Self.setTimer(time);
                Self.sendMessageSocket("server>confirmtimer");
            } else if (splitAction[1] == "continueenergy") {
                Self.initEnergyValues();
                Self.sendMessageSocket("server>confirmcontinue");
            }
        }

    }

    makeConnection() {
        var Self = this;
        var socket = new WebSocket(Self.urlSocket);
        var logArea = document.getElementById("log-server");
        var txtDeviceID = $("#txt-device-id").val();

        socket.onopen = function (e) {
            Self.initOkConfirmation()
            Self.socketConnection = socket;
            Self.idConnection = socket.idConnection;
            let log = "<p>[open] Connection established</p>";
            logArea.insertAdjacentHTML('afterend', log);
            let ipDisp = (Math.floor(Math.random() * 255) + 1) + "." + (Math.floor(Math.random() * 255)) + "." + (Math.floor(Math.random() * 255)) + "." + (Math.floor(Math.random() * 255));
            if (Self.deviceId != null && Self.deviceId != undefined && Self.deviceId != "") {
                Self.socketConnection.send("server>addiddevice>" + Self.deviceId);
                Self.socketConnection.send("server>addipdevice>" + ipDisp);
                
            }
            $("#btn-start").prop("disabled", true);
        };

        socket.onmessage = function (event) {
            console.log(event.data);
            if (event.data.includes("client>")) {
                Self.HandleAction(event.data);
            } else{
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



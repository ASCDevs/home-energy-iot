class Usuario {
    constructor(idDevice,idUser) {
        this.DeviceId = idDevice;
        this.UserId = idUser;
        this.init();
        this.setFunctions();
        this.setEvents();
        this.makeConnection();
    }

    init() {
        $("#device-id").text(this.DeviceId);
    }

    setFunctions() {
    }

    setEvents() {
        var ThisClass = this;
        $("#btn-continue").click(function (e) {
            ThisClass.conn.invoke("ActionContinueDevice");
        })

        $("#btn-stop").click(function (e) {
            ThisClass.conn.invoke("ActionStopDevice");
        })
    }

    makeConnection() {
        var ThisClass = this;
        $(function () {
            var connection = new signalR.HubConnectionBuilder().withUrl("/costumerhub").withAutomaticReconnect().build();

            connection.on("receiveEnergyValue", function (valueEnergy) {
                $("#value-watts").text(valueEnergy)
            });

            connection.on("ActionStopDevice", function () {
                $("#btn-stop").prop("disabled", true)
                $("#btn-continue").prop("disabled", false)
            });

            connection.on("ActionContinueDevice", function () {
                $("#btn-stop").prop("disabled", false)
                $("#btn-continue").prop("disabled", true)
            });

            connection.on("ReceiveDeviceIP", function (ip) {
                $("#device-ip").text(ip)
            });

            connection.on("DeviceIsDisconnected", function () {
                $("#device-status").text("Offline")
            });

            connection.on("DeviceConnected", function () {
                $("#device-status").text("Online")
            });

            connection.start().then(function () {
                ThisClass.conn = connection;
                ThisClass.IdConnection = connection.connectionId;
                connection.invoke("CompleteInfo", ThisClass.DeviceId, ThisClass.UserId);
                connection.invoke("GetDeviceIP");

                console.log("> Hub Costumer conectado!");
            }).catch(function (err) {
                console.log(err.toString());
            })
        })

        
    }
}
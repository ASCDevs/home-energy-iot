class Devices {
    constructor() {
        this.connection = new signalR.HubConnectionBuilder().withUrl("/devicehub").build();
        this.DeviceHubConnection();
        this.setBtnEvents();
    }

    DeviceHubConnection() {
        $("#add-device").attr("disabled", true)
        this.connection.on("ReceiveMessage", function (device, message) {
            
            $("#list-log").append(`<li>Dispostivo:${user} > ${message}</li>`);
        })

        this.connection.start().then(function () {
            $("#add-device").attr("disabled", false)
        }).catch(function (err) {
            return console.log(err.toString());
        })
    }

    setBtnEvents() {
        var ThisClass = this;
        $("#add-device").click(() => {
            
        })
    }
1}
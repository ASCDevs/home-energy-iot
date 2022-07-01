class DeviceItem {
    constructor(DevicesObj,flExist) {
        this.setFunctions();
        this.makeConnection();
        this.Devices = DevicesObj
        
    }

    makeConnection() {
        var ThisClass = this;
        $(function () {
            
            var connection = new signalR.HubConnectionBuilder().withUrl("/devicehub").build();
            $("#add-device").attr("disabled", true);
            connection.on("newDeviceConnected", function (device, message) {
                NotifyConnection(device,message);
            })
            connection.on("updateList", function (connectionId, deviceId, deviceName, action) {
                ThisClass.Devices.UpdateList(connectionId, deviceId, deviceName, action)
            })

            connection.start().then(function () {
                ThisClass.conexao = connection;
                ThisClass.IdConnection = connection.connectionId;
                $("#add-device").attr("disabled", false)
            }).catch(function (err) {
                return console.log(err.toString());
            })
        })
    }

    setFunctions() {
        var ThisClass = this;

        this.closeConnection = function () {
            ThisClass.conexao.stop();
            ThisClass.Devices.removeDeviceFromList(ThisClass.IdConnection)
        }
    }

}
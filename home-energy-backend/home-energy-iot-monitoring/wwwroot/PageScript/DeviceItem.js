class DeviceItem {
    constructor() {
        this.setFunctions();
        this.makeConnection();        
    }

    makeConnection() {
        var ThisClass = this;
        $(function () {
            var connection = new signalR.HubConnectionBuilder().withUrl("/devicehub").build();
            $("#add-device").attr("disabled", true);
            connection.on("connectionsLog", function (device, message) {
                console.log(`>>> ${device}, ${message}`);
            })
            connection.on("updateList", function (listDevices) {
                console.log(listDevices)
            })

            connection.start().then(function () {
                ThisClass.conexao = connection;
                ThisClass.IdConnection = connection.connectionId;
                $("#add-device").attr("disabled", false)
                $("#area-devices").append(ThisClass.makeCardDevice());
            }).catch(function (err) {
                return console.log(err.toString());
            })
        })
    }

    setFunctions() {
        var ThisClass = this;

        this.closeConnection = function () {
            ThisClass.conexao.stop();
        }

        this.makeCardDevice = function () {
            let txtHtml = '<div class="rounded shadow-lg p-10 bg-red-500 hover:shadow-xl" style="background-color:#457B9D">';
            txtHtml += '';
            txtHtml += `<p class="text-white">Conexão ID: ${ThisClass.IdConnection}</p>`;
            txtHtml += '</div>'

            return txtHtml;
        }

        this.sendAction = function (idCon, Action) {
            try {
                ThisClass.conexao.invoke("MandarMensagem", idCon, Action);
            } catch (err) {
                console.error(err);
            }
        }
    }

}
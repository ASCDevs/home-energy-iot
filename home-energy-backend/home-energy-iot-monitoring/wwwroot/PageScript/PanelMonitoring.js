class PanelMonitoring {

    constructor() {
        this.setFunctions();
        this.makeConnection();
    }

    makeConnection() {
        var ThisClass = this;
        $(function () {
            var connection = new signalR.HubConnectionBuilder().withUrl("/panelhub").withAutomaticReconnect().build();
            connection.on("sendPanelLog", function (message) {
                let logMsg = `<p>[log] > ${message}</p>`;
                document.getElementById("area-log").insertAdjacentHTML('afterbegin', logMsg);
            })
            connection.on("updatePanelsOn", function (qtdPanels) {
                $("#qtd-painel-online").text(qtdPanels)
            })
            connection.on("updateClientsOn", function (qtdClients) {
                $("#qtd-clients-on").text(qtdClients)
            })
            connection.on("receiveListClients", function (listClients) {
                let list = JSON.parse(listClients);
                list.map(x => $("#area-devices").append(ThisClass.makeCardDevice(x)))
            })
            connection.on("addNewDeviceCard", function (deviceClient) {
                let device = JSON.parse(deviceClient);
                $("#area-devices").append(ThisClass.makeCardDevice(device))
            })
            connection.on("removeDeviceCard", function (connectionId) {
                $("div[data-connid='" + connectionId+"']").remove();
            })

            connection.start().then(function () {
                ThisClass.conexao = connection;
                ThisClass.IdConnection = connection.connectionId;
                $("#status-onoff").text("online")
                let logMsg = `<p>[start] > Painel conetado</p>`;
                document.getElementById("area-log").insertAdjacentHTML('afterbegin', logMsg);
            }).catch(function (err) {
                console.log(err.toString());
            })

            connection.onreconnecting(function (error) {
                $("#status-onoff").text("offline");
                let logMsg = `<p>[reconnecting] > Conexão perdida (${error.message}])</p>`;
                document.getElementById("area-log").insertAdjacentHTML('afterbegin', logMsg);
            })

            connection.onreconnected(function (connId) {
                $("#status-onoff").text("online");
                ThisClass.IdConnection = connId;
                let logMsg = `<p>[reconnected] > Conexão reestabelecida (${connId}])</p>`;
                document.getElementById("area-log").insertAdjacentHTML('afterbegin', logMsg);
            })

        })
    }

 

    setFunctions() {
        var ThisClass = this;

        this.makeCardDevice = function (dados) {
            let txtHtml = '<div data-connid="' + dados.connectionid + '" class="rounded shadow-lg p-10 bg-indigo-500 hover:shadow-xl">';
            txtHtml += '';
            txtHtml += `<p class="text-white">Conexão ID: ${dados.connectionid}</p>`;
            txtHtml += `<p class="text-white">Device ID: ${dados.deviceid}</p>`;
            txtHtml += `<p class="text-white">Data e hora de conexão: ${dados.dateconn}</p>`;
            txtHtml += '</div>'

            return txtHtml;
        }
    }
}

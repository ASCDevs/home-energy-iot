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
                document.getElementById("area-log").insertAdjacentHTML('afterend', logMsg);
            })
            connection.on("updatePanelsOn", function (qtdPanels) {
                $("#qtd-painel-online").text(qtdPanels)
            })
            connection.on("updateClientsOn", function (qtdClients) {
                $("#qtd-clients-on").text(qtdClients)
            })

            connection.start().then(function () {
                ThisClass.conexao = connection;
                ThisClass.IdConnection = connection.connectionId;
                $("#status-onoff").text("online")
                let logMsg = `<p>[start] > Painel conetado</p>`;
                document.getElementById("area-log").insertAdjacentHTML('afterend', logMsg);
            }).catch(function (err) {
                console.log(err.toString());
            })

            connection.onreconnecting(function (error) {
                $("#status-onoff").text("offline");
                let logMsg = `<p>[reconnecting] > Conexão perdida (${error.message}])</p>`;
                document.getElementById("area-log").insertAdjacentHTML('afterend', logMsg);
            })

            connection.onreconnected(function (connId) {
                $("#status-onoff").text("online");
                ThisClass.IdConnection = connId;
                let logMsg = `<p>[reconnected] > Conexão reestabelecida (${connId}])</p>`;
                document.getElementById("area-log").insertAdjacentHTML('afterend', logMsg);
            })

        })
    }

    setFunctions() {

    }
}

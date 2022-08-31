class PanelMonitoring {

    constructor() {
        this.setEvents();
        this.setFunctions();
        this.makeConnection();
    }

    setEvents() {
        var Self = this;

        this.HandleChangeCurrentState = function (divCard) {
            debugger;
            let state = divCard.data("state");
            let conn = divCard.data("data-connid");

            if (state == "true") {
                
                //Desabilita botão de continuar
                console.log(`<> ${conn} <> Desabilita continuar e habilita parar`)
                //Habilita botão de Parar
                //Habilita botão de Suspender

            }

            if (state == "false") {
                //Habilita botão de continuar
                console.log(`<> ${conn} <> habilita continuar e desabilita parar`)
                //Desabilita botão de Parar
                //Desabilita botão de Suspender
            }

        };

        this.setBtnStopDevice = function () {
            $(".btn-parar-device").off("click");

            $(".btn-parar-device").click(function (e) {
                let divCard = $(this).parent().parent();
                let connId = divCard.data("connid");
                Self.sendStopEnergy(connId);
                //console.log("Conexão id: " + connId);
            })
        }

        this.setBtnContinueDevice = function () {
            $(".btn-continuar-device").off("click");

            $(".btn-continuar-device").click(function (e) {
                let divCard = $(this).parent().parent();
                let connId = divCard.data("connid");
                Self.sendContinueEnergy(connId);
                //console.log("Conexão id: " + connId);
            });
        }

        this.setBtnSuspenderDevice = function () {
            $(".btn-suspender-device").off("click");

            $(".btn-suspender-device").click(function (e) {
                let divCard = $(this).parent().parent();
                let connId = divCard.data("connid");
                Self.sendTimerEnergy(connId);
            });
        }
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
                ThisClass.setBtnStopDevice();
                ThisClass.setBtnContinueDevice();
                //ThisClass.setBtnSuspenderDevice();
                
            })
            connection.on("receiveEnergyValue", function (idConnectionFrom, valueEnergy) {
                $("div[data-connid='" + idConnectionFrom + "'] .field-value").text(valueEnergy+"V")
            })

            connection.on("receiveCurrentState", function (idConnectionFrom, currentState) {
                let divCard = $("div[data-connid='" + idConnectionFrom + "']");
                divCard.attr("data-current", currentState);
                ThisClass.HandleChangeCurrentState(divCard);
            });

            connection.on("addNewDeviceCard", function (deviceClient) {
                let device = JSON.parse(deviceClient);
                if ($("div[data-connid='" + device.connectionid + "']").length == 0) {
                    $("#area-devices").append(ThisClass.makeCardDevice(device))
                    ThisClass.setBtnStopDevice();
                    ThisClass.setBtnContinueDevice();
                    //ThisClass.setBtnSuspenderDevice();
                }
            })
            connection.on("removeDeviceCard", function (connectionId) {
                $("div[data-connid='" + connectionId+"']").remove();
            })

            connection.on("disableButton", function (idConnectionFrom, button) {
                if (button == "stop") {
                    $("div[data-connid='" + idConnectionFrom + "'] .btn-parar-device").prop("disabled", true)
                    $("div[data-connid='" + idConnectionFrom + "'] .btn-continuar-device").prop("disabled", false)
                }

                if (button == "continue") {
                    $("div[data-connid='" + idConnectionFrom + "'] .btn-continuar-device").prop("disabled", true)
                    $("div[data-connid='" + idConnectionFrom + "'] .btn-parar-device").prop("disabled", false)
                }
            })

            connection.start().then(function () {
                ThisClass.conn = connection;
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
        var Self = this;

        this.makeCardDevice = function (dados) {
            let txtHtml = `<div data-connid="${dados.connectionid}" data-state="${dados.state}" class="rounded shadow-lg p-5 bg-indigo-500 hover:shadow-xl">`;
            txtHtml += `<p class="text-white">Conexão ID: ${dados.connectionid}</p>`;
            txtHtml += `<p class="text-white">Device ID: ${dados.deviceid}</p>`;
            txtHtml += `<p class="text-white">Data e hora de conexão: ${dados.dateconn}</p>`;
            txtHtml += `<p class="text-white">Consumo em tempo real: <span class="field-value"></span></p>`;
            txtHtml += `<div class="flex justify-center flex-col p-2 gap-y-1.5">`;
            txtHtml += `<button type="button" href="#" class="btn-continuar-device rounded bg-green-500 p-2 text-sm font-bold text-white hover:bg-green-400 disabled:cursor-not-allowed disabled:hover:bg-zinc-400 disabled:bg-zinc-300">Continuar</button>`;
            txtHtml += `<button type="button" href="#" class="btn-parar-device rounded bg-red-500 p-2 text-sm font-bold text-white hover:bg-red-400 disabled:cursor-not-allowed disable:hover:bg-zinc-400 disabled:bg-zinc-300">Parar</button>`;
            //txtHtml += `<button type="button" href="#" class="btn-suspender-device rounded bg-orange-500 p-2 text-sm font-bold text-white hover:bg-orange-400 disabled:cursor-not-allowed disabled:hover:bg-zinc-400">Suspender 10s</button>`;
            txtHtml += `</div>`;
            txtHtml += '</div>';

            return txtHtml;
        }

        this.sendStopEnergy = function (connId) {
            let txtCommand = "client>stopenergy";
            Self.conn.invoke("SendActionToClient", connId, txtCommand)
        }

        this.sendContinueEnergy = function (connId) {
            let txtCommand = "client>continueenergy";
            Self.conn.invoke("SendActionToClient", connId, txtCommand)
        }

        this.sendTimerEnergy = function (connId,time) {
            let txtCommand = "client>timerenergy>" + time;
            Self.conn.send("SendActionToClient", connId, txtCommand);
        }
    }
}

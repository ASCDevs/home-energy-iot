class Devices {
    constructor() {
        this.devicesConnected = [];
        this.setBtnEvents();
        this.setFunctions();
    }

    setFunctions() {
        var ThisClass = this;

        this.UpdateList = function (connectionId, deviceId, deviceName, action) {
            if (action == "del") {
                let disp = devices.devicesConnected = devices.devicesConnected.filter(x => x.IdConnection == connectionId)
                disp[0].conexao.stop();
                ThisClass.devicesConnected = ThisClass.devicesConnected.filter(x => x.connectionId != connectionId)
            }

            if (action == "add") {
                ThisClass.devicesConnected.push(new DeviceItem(ThisClass,true))
            }

           

        }

        this.NotifyConnection = function (device, message) {
            console.log(`>>> ${device}, ${message}`);
            
            for (let dev of ThisClass.devicesConnected) {
                debugger;
                console.log(dev.connectionId)
                $("#area-devices").append('<div class="rounded shadow-lg p-10 bg-red-500 hover:shadow-xl" style="background-color:#457B9D">' + dev.connectionId + '</div>')
            }
        }
    }

    setBtnEvents() {
        var ThisClass = this;
        $("#add-device").click(() => {
            ThisClass.devicesConnected.push(new DeviceItem(ThisClass,false));
        })
    }
1}
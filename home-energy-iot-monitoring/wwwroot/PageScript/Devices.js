class Devices {
    constructor() {
        this.setBtnEvents();
        this.setFunctions();
    }

    setFunctions() {
        var ThisClass = this;
    }

    setBtnEvents() {
        var ThisClass = this;
        $("#add-device").click(() => {

            connectionList.push(new DeviceItem());
        })
    }
1}
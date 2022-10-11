<template>
    <div id="page-top">
        <div id="wrapper">
            <Sidebar/>

            <div id="content-wrapper" class="d-flex flex-column">
                <div id="content">
                    <NavBarUser/>
                    
                    <div class="container-fluid">
                        <div class="alert alert-danger text-center" role="alert" id="statusConexaoWebSocket" hidden>
                            <small></small>
                        </div>

                        <div class="row">
                            <div class="col-xl-3 col-md-6 mb-4">
                                <div class="card border-left-primary shadow h-100 py-2">
                                    <div class="card-body">
                                        <div class="row no-gutters align-items-center">
                                            <div class="col mr-2">
                                                <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                                                    Medição em tempo real
                                                </div>

                                                <div class="row no-gutters align-items-center">
                                                    <div class="col-auto">
                                                        <div class="h5 mb-0 mr-3 font-weight-bold text-gray-800">
                                                            {{ this.watts }}W 
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-auto">
                                                <i class="fas fa-chart-line fa-2x text-gray-300"></i>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-xl-3 col-md-6 mb-4">
                                <div class="card border-left-success shadow h-100 py-2">
                                    <div class="card-body">
                                        <div class="row no-gutters align-items-center">
                                            <div class="col mr-2">
                                                <div class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                                    Consumo em Reais
                                                </div>

                                                <div class="row no-gutters align-items-center">
                                                    <div class="col-auto">
                                                        <div class="h5 mb-0 mr-3 font-weight-bold text-gray-800">
                                                            R${{ parseFloat(this.deviceConsumption.consumptionInReal).toFixed(4) }}
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-auto">
                                                <i class="fas fa-dollar-sign fa-2x text-gray-300"></i>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-xl-6 col-md-6 mb-4">
                                <div class="card border-left-info shadow h-100 py-2">
                                    <div class="card-body">
                                        <div class="row no-gutters align-items-center">
                                            <div class="col mr-2">
                                                <div class="text-xs font-weight-bold text-info text-uppercase mb-1">
                                                    Tempo de utilização
                                                </div>

                                                <div class="row no-gutters align-items-center">
                                                    <div class="col-auto">
                                                        <div class="h5 mb-0 mr-3 font-weight-bold text-gray-800">
                                                            {{ this.timeUse }}
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-auto">
                                                <i class="fas fa-clock fa-2x text-gray-300"></i>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-12 col-lg-12">
                                <div class="card shadow mb-4">
                                    <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                                        <h6 class="m-0 font-weight-bold text-primary">
                                            Consumo em tempo real
                                        </h6>

                                        <router-link :to="{path: `/report/device/${this.macAddress}`}"> 
                                            Report
                                        </router-link>
                                    </div>

                                    <div class="card-body">
                                        <span v-show="this.ipDevice">
                                            <a :href="`http://${this.ipDevice}/autenticado`" target="_blank" class="text-success" title="IP para configuração do dispositivo"> 
                                                <i class="fas fa-cog"></i> {{ this.ipDevice }}
                                            </a>
                                        </span>

                                        <div class="row mt-3">
                                            <div class="col-xl-12">
                                                <Line 
                                                    :chart-data="chart"
                                                    :chartOptions="chartOptions"
                                                    :width="300"
                                                    :height="400"
                                                />
                                            </div>
                                        </div>

                                        <div class="row text-center">
                                            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 mt-4">
                                                <button @click="continueConnection" id="btnEnableConnection" class="btn btn-success btn-sm" title="Ligar o aparelho">
                                                    <i class="fas fa-play"></i> Ligar dispositivo
                                                </button>
                                            </div>

                                            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 mt-4">
                                                <button @click="stopConnection" id="btnDisableConnection" class="btn btn-danger btn-sm" title="Desligar o aparelho">
                                                    <i class="fas fa-stop"></i> Desligar dispositivo
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    let signalR = require("@microsoft/signalr");

    import { useRoute } from "vue-router";

    import { Line } from "vue-chartjs";
    import { Chart, Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale, LineElement, PointElement } from "chart.js";

    import Sidebar from "../shared/Sidebar.vue";
    import NavBarUser from "../shared/NavBarUser.vue";

    Chart.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale, LineElement, PointElement)

    export default {
        name: "ConsumptionDevice",

        components: {
            Sidebar,
            NavBarUser,
            Line
        },

        data() {
            return {
                chart: {
                    labels: ["", "", "", "", "", ""],
                    datasets: [{
                        label: "Watts em tempo-real",
                        data: [0, 10, 20, 30, 40, 50],
                        backgroundColor: [
                            "rgba(255, 99, 132, 0.2)"
                        ],
                        fill: false,
                    }] 
                },

                chartOptions: {
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                },

                isOnline: false,

                watts: 0.00,

                ipDevice: "",

                connection: null,

                idUser: this.$store.state.user.id,

                macAddress: useRoute().params.id,

                timeUse: "00 hora(s), 00 minuto(s), 00 segundo(s)",

                deviceConsumption: {
                    consumptionInReal: 0.00,
                    consumptionInWatts: 0.00,
                    consumptionDates: []
                }
            };
        },

        methods: {
            connect() {
                var self = this;

                let stateConnection = this.connection.connection._connectionState;

                if(stateConnection == "Disconnected") {
                    this.connection.start().then(() => {
                        this.connection.invoke("CompleteInfo", `${this.macAddress}`, `${this.idUser}`);

                        this.connection.invoke("getDeviceIP");

                        this.connection.on("receiveEnergyValue", function(valueEnergy) {
                            console.log(valueEnergy);

                            console.log(valueEnergy == "");

                            if(valueEnergy == "") {
                                self.watts = 0.00;
                            }

                            let chart = JSON.parse(JSON.stringify(self.chart));

                            let energyFloat = parseFloat(valueEnergy).toFixed(2);

                            self.watts = energyFloat;

                            let list = chart.datasets[0].data;

                            list.push(energyFloat);

                            list.shift();

                            chart.datasets[0].data = list;

                            self.chart = chart
                        });

                        this.connection.on("ReceiveDeviceIP", function(ip) {
                            self.ipDevice = ip;

                            $("#statusConexaoWebSocket").prop("hidden", true);
                        });

                        this.connection.on("DeviceIsDisconnected", function() {
                            self.isOnline = false;

                            self.ipDevice = "";

                            $("#btnDisableConnection").prop("disabled", true);

                            $("#btnEnableConnection").prop("disabled", false);

                            $("#statusConexaoWebSocket small").text("Aparelho está Offline");

                            $("#statusConexaoWebSocket").prop("hidden", false);

                            console.log("Offline");
                        });

                        this.connection.on("DeviceConnected", function() {
                            self.isOnline = true;

                            $("#btnEnableConnection").prop("disabled", true);

                            $("#btnDisableConnection").prop("disabled", false);

                            self.connection.invoke("GetCurrentState");

                            console.log("Online");
                        });

                        this.connection.on("ActionStopDevice", function() {
                            self.isOnline = false;
                            
                            $("#btnEnableConnection").prop("disabled", false);

                            $("#btnDisableConnection").prop("disabled", true);
                        });

                        this.connection.on("ActionContinueDevice", function() {
                            $("#btnEnableConnection").prop("disabled", true);

                            $("#btnDisableConnection").prop("disabled", false);
                        });

                        this.connection.on("ReceiveCurrentState", function(state) {
                            console.log(state);
                            
                            if(state == "ligado") {
                                self.isOnline = true;

                                $("#btnEnableConnection").prop("disabled", true);

                                $("#btnDisableConnection").prop("disabled", false);
                            }

                            if(state == "interrompido") {
                                self.isOnline = false;

                                $("#btnEnableConnection").prop("disabled", false);

                                $("#btnDisableConnection").prop("disabled", true);
                            }
                        });

                        this.connection.onreconnecting(function(error) {
                            $("#statusConexaoWebSocket").prop("hidden", false);

                            $("#statusConexaoWebSocket small").text(error);
                        })

                        this.connection.onreconnected(function(connId) {
                            $("#statusConexaoWebSocket").prop("hidden", false);

                            $("#statusConexaoWebSocket small").text(connId);
                        })
                    })
                    .catch((error) => {
                        console.error(error);
                    })
                }
            },

            stopConnection() {
                this.connection.invoke("ActionStopDevice"); // parar a conexao
            },

            continueConnection() {
                this.connection.invoke("ActionContinueDevice"); // continuar a conexao
            },

            configureSignalR() {
                this.connection = new signalR.HubConnectionBuilder()
                    .withUrl("https://servicehomeiotmonitoring.azurewebsites.net/costumerhub")
                    .configureLogging(signalR.LogLevel.Information)
                    .withAutomaticReconnect()
                    .build();
            }
        },

        mounted() {
            this.configureSignalR();
            this.connect();
        },

        beforeDestroy() {
            delete this.connection;
            delete this.isOnline;
        },

        watch: {
            isOnline(isConnected) {
                setInterval(() => {
                    if(isConnected) {
                        this.$http.get(`/api/deviceConsumption/getDeviceConsumptionTotalValue/${this.macAddress}`)
                            .then((response) => {
                                if(response.status == 200) {
                                    this.deviceConsumption = response.data;

                                    let sizeArray = this.deviceConsumption.consumptionDates.length;

                                    var hours = Math.floor(sizeArray / 3600); // 1 hora = 3600 segundos

                                    var minutes = Math.floor(sizeArray % 3600 / 60); // resto da divisão por 3600 e depois / 60

                                    var seconds = Math.floor(sizeArray % 3600 % 60); // resto da divisão por 3600 e resto da divisão / 60
                                
                                    this.timeUse = `${hours} hora(s), ${minutes} minuto(s), ${seconds} segundo(s)`;
                                }
                            })
                    }
                }, 5000)
            }
        }
    }
</script>

<style scoped></style>
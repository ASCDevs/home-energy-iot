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
                                                        <div class="mb-0 mr-3">
                                                            <h5 class="font-weight-bold text-gray-800" id="value-watts">
                                                                {{ this.watts }}W 
                                                            </h5>
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
                                                        <div class="mb-0 mr-3">
                                                            <h5 class="font-weight-bold text-gray-800" id="real-value-watts">
                                                                R${{ parseFloat(this.deviceConsumption.consumptionInReal).toFixed(4) }}
                                                            </h5>
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

                            <div class="col-xl-6 col-md-12 col-sm-12 mb-4">
                                <div class="card border-left-info shadow h-100 py-2">
                                    <div class="card-body">
                                        <div class="row no-gutters align-items-center">
                                            <div class="col mr-2">
                                                <div class="text-xs font-weight-bold text-info text-uppercase mb-1">
                                                    Tempo de utilização
                                                </div>

                                                <div class="row no-gutters align-items-center">
                                                    <div class="col-auto">
                                                        <div class="mb-0 mr-3">
                                                            <h5 class="font-weight-bold text-gray-800" id="time-use-device">
                                                                {{ this.timeUse }}
                                                            </h5>
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

                                        <router-link :to="{path: `/report/device/${this.macAddress}`}" class="btn btn-secondary btn-sm rounded-sm-circle" title="Visualizar relatório deste dispositivo" id="btn-report-device"> 
                                            <i class="d-sm-block d-md-none fas fa-chart-bar"></i>

                                            <span class="d-none d-md-block">
                                                Relatório
                                            </span>
                                        </router-link>
                                    </div>

                                    <div class="card-body">
                                        <a v-show="this.ipDevice" :href="`http://${this.ipDevice}/autenticado`" target="_blank" class="text-success" title="IP para acessar as configurações da tomada inteligente"> 
                                            <i class="fas fa-cog"></i> {{ this.ipDevice }}
                                        </a>

                                        <div class="row mt-3" id="div-chart">
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
    $(document).ready(function() {
        if($(window).width() <= 540) {
            //\d \w

            console.log($("#time-use-device").text());
        }
    })

    import { useRoute } from "vue-router";

    import { Line } from "vue-chartjs";
    import { Chart, Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale, LineElement, PointElement } from "chart.js";

    import Sidebar from "../shared/Sidebar.vue";
    import NavBarUser from "../shared/NavBarUser.vue";

    Chart.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale, LineElement, PointElement)

    let signalR = require("@microsoft/signalr");
    
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
                        data: [0, 0, 0, 0, 0, 0],
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

                timeUse: "00H:00M:00S",

                deviceConsumption: {
                    consumptionInReal: 0.00,
                    consumptionInWatts: 0.00,
                    consumptionDates: []
                },

                interval: null
            };
        },

        methods: {
            configureSignalR() {
                this.connection = new signalR.HubConnectionBuilder()
                    .withUrl("https://servicehomeiotmonitoring.azurewebsites.net/costumerhub")
                    .configureLogging(signalR.LogLevel.Information)
                    .withAutomaticReconnect()
                    .build();
            },

            connect() {
                var self = this;

                console.log(this.connection.state);

                if(this.connection.state == "Disconnected") {
                    this.connection.start().then(() => {
                        this.connection.invoke("CompleteInfo", `${this.macAddress}`, `${this.idUser}`);

                        this.connection.invoke("getDeviceIP");

                        this.connection.on("receiveEnergyValue", function(valueEnergy) {
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

                            self.watts = 0.00;

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
                            self.isOnline = true;

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

                                self.watts = 0.00;

                                $("#btnEnableConnection").prop("disabled", false);

                                $("#btnDisableConnection").prop("disabled", true);
                            }
                        });

                        this.connection.onclose(function() {
                            self.isOnline = false;

                            console.log("Close");
                        });

                        this.connection.onreconnecting(function(error) {
                            console.error(error);
                        });

                        this.connection.onreconnected(function(connId) {
                            console.log(connId);

                            self.connect();
                        });
                    })
                    .catch((error) => {
                        console.error(error);
                    })
                }
            },

            stopConnection() {
                this.connection.invoke("ActionStopDevice"); // parar a conexao
                this.watts = 0.00;
            },

            continueConnection() {
                this.connection.invoke("ActionContinueDevice"); // continuar a conexao
            },

            getDeviceConsumption() {
                this.interval = setInterval(() => {
                    console.log(`Is connected: ${this.isOnline}`);

                    if(this.isOnline) {
                        this.$http.get(`/api/deviceConsumption/getDeviceConsumptionTotalValue/${this.macAddress}`)
                            .then((response) => {
                                if(response.status == 200) {
                                    this.deviceConsumption = response.data;

                                    let sizeArray = this.deviceConsumption.consumptionDates.length;

                                    var hours = Math.floor(sizeArray / 3600); // 1 hora = 3600 segundos

                                    var minutes = Math.floor(sizeArray % 3600 / 60); // resto da divisão por 3600 e depois / 60

                                    var seconds = Math.floor(sizeArray % 3600 % 60); // resto da divisão por 3600 e resto da divisão / 60
                                
                                    if(this.isScreenSmall()) {
                                        this.timeUse = `${hours}H, ${minutes}M, ${seconds}S`;
                                    } else {
                                        this.timeUse = `${hours} hora(s), ${minutes} minuto(s), ${seconds} segundo(s)`;
                                    }
                                }
                            })
                    }
                }, 5000)
            },

            isScreenSmall() {
                return window.innerWidth <= 540;
            }
        },

        mounted() {
            this.configureSignalR();

            this.connect();

            this.getDeviceConsumption();
        },

        async beforeUnmount() {
            await this.connection.stop();
               
            clearInterval(this.interval);

            console.log("Fechou o hub connection");
        }
    }
</script>

<style scoped>
    @media screen and (max-width: 330px) {
        #btn-report-device {
            display: none;
        }
    }
</style>
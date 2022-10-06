<template>
    <div id="page-top">
        <div id="wrapper">
            <Sidebar/>

            <div id="content-wrapper" class="d-flex flex-column">
                <div id="content">
                    <NavBarUser/>
                    
                    <div class="container-fluid">
                        <div class="col-xl-3 col-md-6 mb-4">
                            <div class="card border-left-info shadow h-100 py-2">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-info text-uppercase mb-1">
                                                Actual value device
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

                        <div class="row">
                            <div class="col-xl-12 col-lg-12">
                                <div class="card shadow mb-4">
                                    <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                                        <h6 class="m-0 font-weight-bold text-primary">
                                            Consumption in RealTime
                                        </h6>

                                        <div class="d-sm-flex align-items-center justify-content-end">
                                            <button class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm">
                                                <i class="fas fa-download fa-sm text-white-50"></i> Generate Report
                                            </button>
                                        </div>
                                    </div>

                                    <div class="card-body">
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

                                        <div class="row">
                                            <div class="col-xl-12 col-md-12 mt-4">
                                                <div class="card-body">
                                                    <div class="text-center">
                                                        <button @click="continueConnection" id="btnEnableConnection" class="btn btn-success btn-sm" title="Ligar o aparelho">
                                                            <i class="fas fa-redo-alt"></i> Continuar
                                                        </button>

                                                        <button @click="stopConnection" id="btnDisableConnection" class="btn btn-danger btn-sm ml-5" title="Desligar o aparelho">
                                                            <i class="fas fa-stop"></i> Parar
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
        </div>
    </div>
</template>

<script>
    let signalR = require("@aspnet/signalr");

    import { useRoute } from "vue-router";

    import { Line } from "vue-chartjs";
    import { Chart, Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale, LineElement, PointElement } from 'chart.js'

    import Sidebar from "../shared/Sidebar.vue";
    import NavBarUser from "../shared/NavBarUser.vue";

    Chart.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale, LineElement, PointElement)

    export default {
        name: "AboutView",

        components: {
            Sidebar,
            NavBarUser,
            Line
        },

        data() {
            return {
                chart: {
                    labels: ["Watts", "Watts", "Watts", "Watts", "Watts", "Watts"],
                    datasets: [{
                        label: "Watts realtime",
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

                watts: 0,

                connection: null,

                idUser: this.$store.state.user.id,

                idDevice: useRoute().params.id,
            };
        },

        methods: {
            connect() {
                var thisVue = this;

                this.connection.start().then(() => {

                    // idDeviceTest = 'HU:34:DS4:D1', idUser = '0'

                    this.connection.invoke("CompleteInfo", "HU:34:DS4:D1", `${this.idUser}`);

                    this.connection.on("receiveEnergyValue", function(valueEnergy) {
                        let chart = JSON.parse(JSON.stringify(thisVue.chart));

                        thisVue.watts = valueEnergy;

                        let list = chart.datasets[0].data;

                        list.push(valueEnergy);

                        list.shift();

                        chart.datasets[0].data = list;

                        thisVue.chart = chart
                    });

                    this.connection.on("stopConfirmed", function() {
                        console.log("stopConfirmed");

                        document.querySelector("#btnEnableConnection").disabled = false;

                        document.querySelector("#btnDisableConnection").disabled = true;
                    });

                    this.connection.on("continueConfirmed", function() {
                        console.log("continueConfirmed");

                        document.querySelector("#btnEnableConnection").disabled = true;

                        document.querySelector("#btnDisableConnection").disabled = false;
                    });

                    this.connection.onclose(() => {
                        console.log("Fechou a conexão");
                    })
                })
                .catch((error) => {
                    console.log(error);

                    setTimeout(() => {
                        this.connect();
                    }, 5000)
                })
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
                    .build();
            }
        },

        mounted() {
            this.configureSignalR();
            this.connect();
        }
    }
</script>

<style scoped></style>
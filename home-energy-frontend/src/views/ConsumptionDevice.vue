<template>
    <div id="page-top">
        <div id="wrapper">
            <Sidebar/>

            <div id="content-wrapper" class="d-flex flex-column">
                <div id="content">
                    <NavBarUser/>
                    
                    <div class="container-fluid">
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
                                        <div class="row">
                                            <div class="col-xl-6 col-md-12 mb-4">
                                                <div class="card-body">
                                                    <h1> WEB Socket</h1>

                                                    <button @click="continueConnection" id="btnEnableConnection" class="btn btn-success btn-sm">
                                                        Continuar
                                                    </button>

                                                    <button @click="stopConnection" id="btnDisableConnection" class="btn btn-danger btn-sm ml-5">
                                                        Parar
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
</template>

<script>
    let signalR = require("@aspnet/signalr");

    import { useRoute } from "vue-router";
    import Sidebar from "../shared/Sidebar.vue";
    import NavBarUser from "../shared/NavBarUser.vue";

    export default {
        name: "AboutView",

        components: {
            Sidebar,
            NavBarUser 
        },

        data() {
            return {
                connection: null,
                watts: 0,
                idUser: this.$store.state.user.id,
                idDevice: useRoute().params.id
            };
        },

        methods: {
            connect() {
                this.connection.start().then(() => {
                    // idDeviceTest = 'HU:34:DS4:D1', idUser = '0'

                    this.connection.invoke("CompleteInfo", `${this.idDevice}`, `${this.idUser}`);

                    this.connection.on("receiveEnergyValue", function(valueEnergy) {                    
                        console.log(valueEnergy);

                        this.watts = valueEnergy;
                    })

                    this.connection.on("stopConfirmed", function() {
                        document.querySelector("#btnEnableConnection").disabled = false;
                        document.querySelector("#btnDisableConnection").disabled = true;
                    });

                    this.connection.on("continueConfirmed", function() {
                        document.querySelector("#btnEnableConnection").disabled = true;
                        document.querySelector("#btnDisableConnection").disabled = false;
                    });
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
            //this.configureSignalR();
            //this.connect();
        }
    }
</script>

<style scoped></style>
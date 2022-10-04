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
                                            About
                                        </h6>
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

                                                    <h3> {{ watts }} </h3>
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

    import Sidebar from "../shared/Sidebar.vue";
    import NavBarUser from "../shared/NavBarUser.vue";
import { watch } from "vue";

    export default {
        name: "AboutView",

        components: { 
            Sidebar, 
            NavBarUser 
        },

        data() {
            return {
                connection: null,
                watts: 0
            };
        },

        methods: {
            connect() {
                this.connection.start().then(() => {
                    let idUser = this.$store.state.user.id;

                    this.connection.invoke("CompleteInfo", "HU:34:DS4:D1", `${idUser}`); // idDevice = 'HU:34:DS4:D1', idUser = 0

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
            this.configureSignalR();
            this.connect();
        },

        watch: {
            watts(newValue, oldValue) {
                console.log(newValue);

                console.log(oldValue);

                this.watts = newValue
            }
        }
    }
</script>

<style scoped></style>
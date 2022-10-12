<template>
    <div id="page-top">
        <div id="wrapper">
            <Sidebar/>

            <div id="content-wrapper" class="d-flex flex-column">
                <div id="content">
                    <NavBarUser/>
                    
                    <div class="container-fluid">
                        <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12">
                            <div class="card shadow mb-4">
                                <div class="card-header py-3 text-lg-left text-center">
                                    <h6 class="m-0 font-weight-bold text-primary">
                                        Relatório de Dispositivos
                                    </h6>
                                </div>

                                <div class="card-body">
                                    <form @submit.prevent="getReportConsumptionBetweenDates">
                                        <div class="form-row align-items-center">
                                            <div class="col-xl-3 col-lg-3 col-md-4 col-sm-12">
                                                <label for="initialDate" class="font-weight-bold">
                                                    Data inicial:
                                                </label>

                                                <input v-model="report.initialDate" class="form-control form-control-sm" type="datetime-local" id="initialDate" required />
                                            </div>

                                            <div class="col-xl-3 col-lg-3 col-md-4 col-sm-12 mt-md-0 mt-3">
                                                <label for="finalDate" class="font-weight-bold">
                                                    Data final:
                                                </label>
                                                
                                                <input v-model="report.finalDate" class="form-control form-control-sm" type="datetime-local" id="finalDate" required />
                                            </div>
                                        
                                            <div class="col-xl-3 col-lg-3 col-md-4 col-sm-12 mt-4 text-lg-left text-center">
                                                <button type="submit" class="btn btn-primary btn-sm"> 
                                                    Gerar Relatorio 
                                                </button>
                                            </div>
                                        </div>
                                    </form>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-8 col-lg-8">
                                <div class="card shadow mb-4">
                                    <div class="card-body">
                                        <div class="mt-5">
                                            <Line 
                                                :chart-data="chart"
                                                :chartOptions="chartOptions"
                                                :width="300"
                                                :height="400"
                                            />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-xl-4 col-lg-4">
                                <div class="card shadow mb-4">
                                    <div class="card-body">
                                        <div class="chart-pie">
                                            <h4> Tempo de consumo </h4>

                                            <i class=""></i>

                                            {{ this.timeUse }}
                                        </div>

                                        <div class="mt-4 text-center small">
                                            <span class="mr-2">
                                                <i class="fas fa-circle text-primary"></i> Direct
                                            </span>

                                            <span class="mr-2">
                                                <i class="fas fa-circle text-success"></i> Social
                                            </span>

                                            <span class="mr-2">
                                                <i class="fas fa-circle text-info"></i> Referral
                                            </span>
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
    import { useRoute } from "vue-router";

    import { Line } from "vue-chartjs";
    import { Chart, Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale, LineElement, PointElement } from "chart.js";

    import Sidebar from "@/shared/Sidebar.vue";
    import NavBarUser from "../shared/NavBarUser.vue";

    Chart.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale, LineElement, PointElement)

    export default {
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
                        label: "TESTE",
                        data: [],
                        backgroundColor: [
                            "rgba(255, 99, 132, 0.2)"
                        ],
                        fill: false,
                    }] 
                },

                timeUse: "",

                chartOptions: {
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                },

                report: {
                    deviceIdentificationCode: useRoute().params.id,
                    initialDate: "2022-10-10T12:00",
                    finalDate: "2022-10-11T12:00"
                },

                status: 0
            }
        },

        methods: {
            getReportConsumptionBetweenDates() {
                this.$http.post("/api/DeviceConsumption/GetConsumptionValueBetweenDates", this.report)
                    .then((response) => {
                        if(response.status == 200) {
                            console.log(response.data);

                            let sizeArray = response.data.consumptionDates.length;

                            var hours = Math.floor(sizeArray / 3600);

                            var minutes = Math.floor(sizeArray % 3600 / 60);

                            var seconds = Math.floor(sizeArray % 3600 % 60);

                            this.timeUse = `${hours} hora(s), ${minutes} minuto(s), ${seconds} segundo(s)`;

                            //this.status = 200;
                        }
                    })
                    .catch((error) => {
                        console.log(error);
                    })
            }
        }
    }
</script>

<style scoped>
    form button[type="submit"] {
        margin-top: 7px;
    }
</style>
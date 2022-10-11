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
                                            Report
                                        </h6>
                                    </div>

                                    <div class="card-body">
                                        <form @submit.prevent="getReportConsumptionBetweenDates">
                                            <input v-model="report.initialDate" class="form-control form-control-sm" type="datetime-local" id="initialDate" />

                                            <input v-model="report.finalDate" class="form-control form-control-sm mt-5" type="datetime-local" id="finalDate" />

                                            <button type="submit" class="btn btn-primary btn-sm mt-5 mb-5"> 
                                                Gerar Relatorio 
                                            </button> <br/>

                                            {{ this.report }}
                                        </form>

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
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import { useRoute } from "vue-router";

    import "../assets/vendor/bootstrap/js/bootstrap.bundle.min.js";
    import "../assets/vendor/jquery-easing/jquery.easing.min.js";
    import "../assets/vendor/sb-admin/js/sb-admin-2.min.js";

    import Sidebar from "@/shared/Sidebar.vue";
    import NavBarUser from "../shared/NavBarUser.vue";

    export default {
        components: { 
            Sidebar, 
            NavBarUser
        },

        data() {
            return {
                report: {
                    deviceIdentificationCode: useRoute().params.id,
                    initialDate: "",
                    finalDate: ""
                }
            }
        },

        methods: {
            getReportConsumptionBetweenDates() {
                this.$http.post("/api/DeviceConsumption/GetConsumptionValueBetweenDates", this.report)
                    .then((response) => {
                        console.log(response);
                    })
                    .catch((error) => {
                        console.log(error);
                    })
            }
        }
    }
</script>

<style scoped></style>
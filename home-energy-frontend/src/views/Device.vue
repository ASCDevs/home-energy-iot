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
                                            My Devices
                                        </h6>

                                        <div class="row">
                                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 text-right">
                                                <button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#exampleModal">
                                                    <i class="fas fa-plus"></i> Add Device
                                                </button>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="card-body">
                                        <div v-if="devices.length > 0" class="row">
                                            <div v-for="(device, index) in devices" :key="index" class="col-xl-4 col-md-12 mb-4">
                                                <div class="card border-left-primary shadow h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col-auto">
                                                                <i class="fas fa-microchip fa-2x text-gray-300"></i>
                                                            </div>

                                                            <div class="col ml-3 mr-2">
                                                                <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                                    {{ device.name }}
                                                                </div>
                                                            </div>

                                                            <div class="col-auto">
                                                                <router-link to="/" title="Edit house">
                                                                    <i class="fas fa-pen"></i>
                                                                </router-link>

                                                                <router-link :to="{path: `/device/${device.id}/consumption`}" class="ml-3" title="View consumption this device">
                                                                    <i class="fas fa-angle-right"></i>
                                                                </router-link>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div v-else class="row">
                                            <div class="col-12 alert alert-info text-center" role="alert">
                                                You don't own any registered device
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Modal -->
                <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <form @submit.prevent="register" class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="exampleModalLabel">
                                    Register device
                                </h5>
                                
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">
                                        &times;
                                    </span>
                                </button>
                            </div>

                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12">
                                        <label for="deviceName"> 
                                            Device name: 
                                        </label>

                                        <input v-model="device.name" type="text" class="form-control form-control-sm" id="deviceName" placeholder="Nome/Apelido do dispositivo" required>
                                    </div>
                                </div>

                                <div class="row mt-3">
                                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12">
                                        <label for="macAddress"> 
                                            MAC Address:
                                        </label>

                                        <input v-model="device.identificationCode" type="text" class="form-control form-control-sm" id="macAddress" placeholder="MAC Address, encontra-se no aparelho" required>
                                    </div>
                                </div>

                                <div class="row mt-3">
                                    <div class="col-xl-4 col-lg-4 col-md-6 col-sm-12">
                                        <label for="wattsDevice"> 
                                            Watts: 
                                        </label>

                                        <input v-model="device.watts" class="form-control form-control-sm" id="wattsDevice" placeholder="Quantidade de watts do aparelho" required>
                                    </div>
                                </div>

                                <div class="row mt-3">
                                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12">
                                        <label for="descriptionDevice"> 
                                            Description: 
                                        </label>

                                        <textarea v-model="device.description" rows="7" class="form-control form-control-sm" id="descriptionDevice" placeholder="Observações" required></textarea>
                                    </div>
                                </div>
                            </div>

                            <div class="modal-footer">
                                <button type="submit" class="btn btn-primary btn-sm"> 
                                    Register 
                                </button>

                                <button type="button" class="btn btn-dark btn-sm" data-dismiss="modal">
                                    Close
                                </button>
                            </div>
                        </form>
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
        name: "RegisterDevice",

        components: { 
            Sidebar, 
            NavBarUser 
        },

        data() {
            return {
                devices: [],
                
                device: {
                    id: 0,
                    idHouse: useRoute().params.id,
                    identificationCode: '', // macAddress
                    name: '',
                    description: '',
                    watts: 0
                }
            }
        },

        methods: {
            register() {
                this.$http.post(`/api/device/create`, this.device)
                    .then((response) => {
                        if(response.status == 200) {
                            $('#exampleModal').modal('hide');

                            this.getDevicesHouse();

                            this.device.name = '';

                            this.device.identificationCode = '';

                            this.device.description = '';

                            this.device.watts = 0;
                        }
                    })
                    .catch((error) => {
                        console.error(error);
                    })
            },

            getDevicesHouse() {
                this.$http.get(`/api/device/getByHouseId/${this.device.idHouse}`)
                    .then((response) => {
                        if(response.status == 200) {
                            this.devices = response.data;
                        }
                    })
                    .catch((error) => {
                        console.error(error);
                    })
            }
        },

        beforeCreate() {
            $(".modal-backdrop").remove();
        },

        created() {
            this.getDevicesHouse();
        }
    }
</script>

<style scoped></style>
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
                                            Meus dispositivos
                                        </h6>

                                        <div class="row">
                                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 text-right">
                                                <button @click="clearFormDevice" type="button" class="btn btn-primary btn-sm rounded-sm-circle" data-toggle="modal" data-target="#exampleModal" title="Adicionar novo dispositivo">
                                                    <i class="d-sm-block d-md-none fas fa-plus"></i>

                                                    <span class="d-none d-md-block">
                                                        Add dispositivo
                                                    </span>
                                                </button>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="card-body">
                                        <div v-if="devices.length > 0" class="row">
                                            <div v-for="(device, index) in devices" :key="index" class="col-xl-6 col-lg-6 col-md-12 col-sm-12 mb-4">
                                                <div class="card border-left-primary shadow h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row">
                                                            <div class="col-xl-1 col-lg-1 d-none d-lg-block">
                                                                <i class="fas fa-microchip fa-2x text-gray-300"></i>
                                                            </div>

                                                            <div class="col-xl-4 col-lg-6 col-md-12 col-sm-12 text-lg-left text-center mt-1">
                                                                <!-- Tela small -->
                                                                <div class="d-lg-none font-weight-bold text-truncate text-dark ">
                                                                    {{ device.name }}
                                                                </div>
                                                                
                                                                <div class="d-none d-lg-block font-weight-bold text-gray-800 ml-2">
                                                                    {{ device.name }}
                                                                </div>
                                                            </div>

                                                            <div class="col-xl-6 col-lg-5 col-md-12 col-sm-12 text-lg-right text-center mt-2">
                                                                <a @click="editDevice(device.id)" type="button" to="/" title="Editar alguma informação do dispositivo">
                                                                    <i class="fas fa-pen"></i>
                                                                </a>

                                                                <router-link :to="{path: `/device/${device.identificationCode}/consumption`}" class="ml-4" title="Visualizar em tempo real o consumo deste dispositivo">
                                                                    <i class="fas fa-angle-right"></i>
                                                                </router-link>

                                                                <router-link :to="{path: `/report/device/${device.identificationCode}`}" class="text-success ml-4" title="Relatório do dispositivo">
                                                                    <i class="fas fa-chart-area"></i>
                                                                </router-link>

                                                                <a type="button" @click="deleteDevice(device)" class="text-danger ml-4" title="Excluir dispositivo">
                                                                    <i class="fas fa-trash-alt"></i>
                                                                </a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div v-else class="row">
                                            <div class="col-12 alert alert-info text-center" role="alert">
                                                Você não possuí nenhum dispositivo cadastrado
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
                                    {{ this.device.id == 0 ? "Cadastrar dispositivo" : "Alteração dispositivo" }}
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
                                            Nome:
                                        </label>

                                        <input v-model="device.name" type="text" class="form-control form-control-sm" id="deviceName" placeholder="Nome/Apelido do dispositivo" required>
                                    </div>
                                </div>

                                <div class="row mt-3">
                                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12">
                                        <label for="macAddress"> 
                                            MAC Address:
                                        </label>

                                        <input v-model="device.identificationCode" type="text" class="form-control form-control-sm" id="macAddress" placeholder="MAC Address, encontra-se na tomada inteligente" required>
                                    </div>
                                </div>

                                <div class="row mt-3">
                                    <div class="col-xl-4 col-lg-4 col-md-4 col-sm-12">
                                        <label for="wattsDevice"> 
                                            Watts: 
                                        </label>

                                        <input v-model="device.watts" class="form-control form-control-sm" id="wattsDevice" placeholder="Quantidade de watts do aparelho" required>
                                    </div>

                                    <div class="col-xl-8 col-lg-8 col-md-8 col-sm-12 mt-xl-4 mt-lg-4 mt-md-4 mt-sm-0">
                                        <small class="text-xs text-muted">
                                            Quantidade de Watts(W) que o produto conectado a tomada inteligente consome
                                        </small>
                                    </div>
                                </div>

                                <div class="row mt-3">
                                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12">
                                        <label for="descriptionDevice"> 
                                            Observações:
                                        </label>

                                        <textarea v-model="device.description" rows="7" class="form-control form-control-sm" id="descriptionDevice" placeholder="Aqui você pode escrever informações como dispositivo está no quarto/sala"></textarea>
                                    </div>
                                </div>
                            </div>

                            <div class="modal-footer">
                                <button type="submit" class="btn btn-primary btn-sm"> 
                                    {{ this.device.id == 0 ? "Cadastrar" : "Alterar"}} 
                                </button>

                                <button type="button" class="btn btn-dark btn-sm" data-dismiss="modal">
                                    Fechar
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
                    identificationCode: "", // macAddress
                    name: "",
                    description: "",
                    watts: 0
                }
            }
        },

        methods: {
            register() {
                if(this.device.id == 0) {
                    this.$http.post("/api/device/create", this.device)
                        .then((response) => {
                            if(response.status == 200) {
                                alert("Salvo com sucesso");

                                $("#exampleModal").modal("hide");

                                this.getDevicesHouse();

                                this.clearFormDevice();
                            }
                        })
                        .catch((error) => {
                            console.error(error);
                        })

                } else {
                    this.$http.put("/api/device/update", this.device)
                        .then((response) => {
                            if(response.status == 200) {
                                alert("Atualizado com sucesso");

                                $("#exampleModal").modal("hide");

                                this.getDevicesHouse();

                                this.clearFormDevice();
                            }
                        })
                }
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
            },

            editDevice(idDevice) {
                this.$http.get(`/api/device/get/${idDevice}`)
                    .then((response) => {
                        if(response.status == 200) {
                            this.device = response.data;

                            $("#exampleModal").modal("show");
                        }
                    })
            },

            deleteDevice(device) {
                if(confirm(`Deseja excluir o dispositivo com nome '${device.name}'?`)) {                    
                    this.$http.delete(`/api/device/delete/${device.id}`)
                        .then((response) => {
                            if(response.status == 200) {
                                this.getDevicesHouse();
                            }
                        })
                        .catch((error) => {
                            console.log(error);
                        })
                }
            },

            clearFormDevice() {
                this.device.id = 0;
                this.device.name = "";
                this.device.identificationCode = "";
                this.device.description = "";
                this.device.watts = 0;
            }
        },

        beforeCreate() {
            $("#overlay").css("display", "none");
            $(".modal-backdrop").remove();
        },

        created() {
            this.getDevicesHouse();
        }
    }
</script>

<style scoped></style>
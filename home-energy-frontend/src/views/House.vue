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
                                            Minhas casas
                                        </h6>

                                        <div class="row">
                                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 text-right">
                                                <button @click="clearFormHouse" type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#exampleModal">
                                                    <i class="fas fa-plus"></i> Add casa
                                                </button>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="card-body">
                                        <div v-if="houses.length > 0" class="row">
                                            <div v-for="(house, index) in houses" :key="index" class="col-xl-6 col-md-12 mb-4">
                                                <div class="card border-left-primary shadow h-100 py-2">
                                                    <div class="card-body">
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col-auto">
                                                                <i class="fas fa-house-user fa-2x text-gray-300"></i>
                                                            </div>

                                                            <div class="col ml-3 mr-2">
                                                                <div class="text-xs font-weight-bold text-dark mb-1">
                                                                    {{ house.name }}
                                                                </div>

                                                                <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                                    {{ house.nameAddress }}, {{house.numberAddress}}
                                                                </div>
                                                            </div>

                                                            <div class="col-auto">
                                                                <a type="button" @click="editHouse(house.id)" title="Editar alguma informação da casa">
                                                                    <i class="fas fa-pen"></i>
                                                                </a>

                                                                <router-link :to="{path: `/house/${house.id}/devices`}" class="ml-3" title="Ver todos os dispositivos desta casa">
                                                                    <i class="fas fa-angle-right"></i>
                                                                </router-link>

                                                                <a type="button" @click="deleteHouse(house)" class="text-danger ml-3" title="Excluir casa">
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
                                                Você não possuí nenhuma casa cadastrada
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
                                    Cadastrar casa
                                </h5>
                                
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">
                                        &times;
                                    </span>
                                </button>
                            </div>

                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12">
                                        <label for="houseName"> 
                                            Nome: 
                                        </label>

                                        <input v-model="house.name" type="text" class="form-control form-control-sm" id="houseName" placeholder="Nome/Apelido da casa" required>
                                    </div>

                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 mt-sm-3 mt-md-0 mt-lg-0 mt-xl-0">
                                        <label for="typeAddress"> 
                                            Logradouro:
                                        </label>

                                        <select v-model="house.typeAddress" class="form-control form-control-sm" id="typeAddress" required>
                                            <option value=""> Selecione o logradouro </option>                                           
                                            <option value="Rua"> Rua </option>
                                            <option value="Avenida"> Avenida </option>
                                        </select>
                                    </div>
                                </div>

                                <div class="row mt-3">
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12">
                                        <label for="nameAddress"> 
                                            Endereço: 
                                        </label>

                                        <input v-model="house.nameAddress" type="text" class="form-control form-control-sm" id="nameAddress" placeholder="Rua/Avenida ..." required>
                                    </div>

                                    <div class="col-xl-3 col-lg-3 col-md-6 col-sm-12">
                                        <label for="numberAddress"> 
                                            Numero: 
                                        </label>

                                        <input v-model="house.numberAddress" type="text" class="form-control form-control-sm" id="numberAddress" required>
                                    </div>
                                </div>

                                <div class="row mt-3">
                                    <div class="col-xl-7 col-lg-7 col-md-6 col-sm-12">
                                        <label for="periodDaysReport"> 
                                            Relatório a cada quantos dias:
                                        </label>

                                        <select v-model="house.periodDaysReport" class="form-control form-control-sm" id="periodDaysReport" required>
                                            <option value="" disabled> Selecione um número </option>
                                            <option value="1"> 1 </option>
                                            <option value="2"> 2 </option>
                                            <option value="3"> 3 </option>
                                            <option value="4"> 4 </option>
                                            <option value="5"> 5 </option>
                                            <option value="6"> 6 </option>
                                            <option value="7"> 7 </option>
                                            <option value="14"> 14 </option>
                                            <option value="30"> 30 </option>
                                        </select>
                                    </div>

                                    <div class="col-xl-5 col-lg-5 col-md-6 col-sm-12">
                                        <label for="valueKWh"> 
                                            Valor em KWh:
                                        </label>

                                        <input v-model="house.valuePerKWH" type="text" class="form-control form-control-sm" id="valueKWh" required>
                                    </div>
                                </div>
                            </div>

                            <div class="modal-footer">
                                <button type="submit" class="btn btn-primary btn-sm"> 
                                    Cadastrar 
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
    import "../assets/vendor/bootstrap/js/bootstrap.bundle.min.js";
    import "../assets/vendor/jquery-easing/jquery.easing.min.js";
    import "../assets/vendor/sb-admin/js/sb-admin-2.min.js";

    import Sidebar from "@/shared/Sidebar.vue";
    import NavBarUser from "../shared/NavBarUser.vue";

    export default {
        name: "RegisterHouse",

        components: { 
            Sidebar, 
            NavBarUser 
        },

        data() {
            return {
                houses: [],

                house: {
                    id: 0,
                    idUser: this.$store.state.user.id,
                    name: '',
                    typeAddress: '',
                    nameAddress: '',
                    numberAddress: 0,
                    periodDaysReport: 1,
                    valuePerKWH: 0.0
                }
            }
        },

        methods: {
            register() {
                if(this.house.id == 0) { 
                    this.$http.post("/api/house/create", this.house)
                        .then((response) => {
                            if(response.status == 200) {
                                $("#exampleModal").modal("hide");

                                this.getHousesUser();

                                this.clearFormHouse();
                            }
                        })
                        .catch((error) => {
                            console.error(error);
                        })
                        
                } else {
                    this.$http.put("/api/house/update", this.house)
                        .then((response) => {
                            if(response.status == 200) {
                                $("#exampleModal").modal("hide");

                                this.getHousesUser();

                                this.clearFormHouse();
                            }
                        })
                        .catch((error) => {
                            console.error(error);
                        })
                }
            },

            getHousesUser() {
                this.$http.get(`/api/house/getByUserId/${this.house.idUser}`)
                    .then((response) => {
                        if(response.status == 200) {
                            this.houses = response.data;
                        }
                    })
                    .catch((error) => {
                        console.error(error);
                    })
            },

            editHouse(idHouse) {
                this.$http.get(`/api/house/get/${idHouse}`)
                    .then((response) => {
                        if(response.status == 200) {
                            this.house = response.data;

                            $("#exampleModal").modal("show");
                        }
                    })
            },

            deleteHouse(house) {
                if(confirm(`Deseja excluir a casa com nome '${house.name}'?`)) {                    
                    this.$http.delete(`/api/house/delete/${house.id}`)
                        .then((response) => {
                            if(response.status == 200) {
                                this.getHousesUser();
                            }
                        })
                        .catch((error) => {
                            console.log(error);
                        })
                }
            },

            clearFormHouse() {
                this.house.id = 0;
                this.house.idUser = this.$store.state.user.id;
                this.house.name = "";
                this.house.typeAddress = "";
                this.house.nameAddress = "";
                this.house.numberAddress = "";
                this.house.numberAddress = 0;
                this.house.periodDaysReport = 1;
                this.house.valuePerKWH = 0.0;
            }
        },

        beforeCreate() {
            $(".modal-backdrop").remove();
        },

        created() {
            this.getHousesUser();
        },
    }
</script>

<style scoped></style>
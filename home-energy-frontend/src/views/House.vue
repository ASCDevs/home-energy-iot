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
                                            My Houses
                                        </h6>

                                        <div class="row">
                                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 text-right">
                                                <button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#exampleModal">
                                                    <i class="fas fa-plus"></i> Add house
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
                                                                <router-link to="/" title="Edit house">
                                                                    <i class="fas fa-pen"></i>
                                                                </router-link>

                                                                <router-link :to="{path: `/house/${house.id}/devices`}" class="ml-3" title="See all devices in this house">
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
                                                You don't own any registered house
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
                                    Register house
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
                                            Name: 
                                        </label>

                                        <input v-model="house.name" type="text" class="form-control form-control-sm" id="houseName" placeholder="Codername of house" required>
                                    </div>

                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12 mt-sm-3 mt-md-0 mt-lg-0 mt-xl-0">
                                        <label for="typeAddress"> 
                                            Type Address: 
                                        </label>

                                        <select v-model="house.typeAddress" class="form-control form-control-sm" id="typeAddress" required>
                                            <option value=""> Select Type Address </option>                                           
                                            <option value="Rua"> Rua </option>
                                            <option value="Avenida"> Avenida </option>
                                        </select>
                                    </div>
                                </div>

                                <div class="row mt-3">
                                    <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12">
                                        <label for="nameAddress"> 
                                            Address: 
                                        </label>

                                        <input v-model="house.nameAddress" type="text" class="form-control form-control-sm" id="nameAddress" placeholder="Rua/Avenida ..." required>
                                    </div>

                                    <div class="col-xl-3 col-lg-3 col-md-6 col-sm-12">
                                        <label for="numberAddress"> 
                                            Number: 
                                        </label>

                                        <input v-model="house.numberAddress" type="text" class="form-control form-control-sm" id="numberAddress" required>
                                    </div>
                                </div>

                                <div class="row mt-3">
                                    <div class="col-xl-5 col-lg-5 col-md-6 col-sm-12">
                                        <label for="periodDaysReport"> 
                                            Period Days Report: 
                                        </label>

                                        <select v-model="house.periodDaysReport" class="form-control form-control-sm" id="periodDaysReport" required>
                                            <option value="" disabled> Select day </option>
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
                    periodDaysReport: 1
                }
            }
        },

        methods: {
            register() {
                this.$http.post('/api/house/create', this.house)
                    .then((response) => {
                        if(response.status == 200) {
                            $('#exampleModal').modal('hide');
                            this.getHousesUser();
                            this.house.name = '';
                            this.house.typeAddress = '';
                            this.house.nameAddress = '';
                            this.house.numberAddress = 0;
                            this.house.periodDaysReport = 1;
                        }
                    })
                    .catch((error) => {
                        console.error(error);
                    })
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
            }
        },

        beforeCreate() {
            $(".modal-backdrop").remove();
        },

        created() {
            this.getHousesUser();
        }
    }
</script>

<style scoped></style>
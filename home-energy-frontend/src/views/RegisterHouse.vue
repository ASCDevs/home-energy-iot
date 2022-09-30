<template>
    <div>
        <NavBarOptions/>

        <div class="main-content">
            <NavBarUser />
            
            <div class="header bg-gradient-primary pb-8 pt-5 pt-md-8">
                <div class="container-fluid">
                    <div class="header-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="card shadow">
                                    <div class="card-header bg-transparent">
                                        <div class="row">
                                            <div class="col-xl-3 col-lg-3 col-md-6 col-sm-12">
                                                <h3 class="mb-0"> House(s) </h3>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 text-right">
                                                <button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#exampleModal">
                                                    <i class="ni ni-fat-add"></i> Add house
                                                </button>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="card-body">
                                        <div class="icon-examples">
                                            <table class="table table-hover table-bordered table-responsive-sm">
                                                <thead class="thead-light">
                                                    <tr>
                                                        <th scope="col"> # </th>
                                                        <th scope="col"> Codername </th>
                                                        <th scope="col"> Type </th>
                                                        <th scope="col"> Address </th>
                                                        <th scope="col"> Number </th>
                                                        <th scope="col"> Report each </th>
                                                    </tr>
                                                </thead>

                                                <tbody>
                                                    <tr v-for="house in houses" :key="house.id">
                                                        <td> {{house.id}} </td>
                                                        <td> {{house.name}} </td>
                                                        <td> {{house.typeAddress}} </td>
                                                        <td> {{house.nameAddress}} </td>
                                                        <td> {{house.numberAddress}} </td>
                                                        <td> {{house.periodDaysReport}} </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>

                                    <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                        <div class="modal-dialog" role="document">
                                            <div class="modal-content">
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

                                                            <input v-model="house.name" type="text" class="form-control form-control-sm" id="houseName">
                                                        </div>

                                                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12">
                                                            <label for="typeAddress"> 
                                                                Type Address: 
                                                            </label>

                                                            <select v-model="house.typeAddress" class="form-control form-control-sm" id="typeAddress">
                                                                <option value="Rua"> 
                                                                    Rua 
                                                                </option>

                                                                <option value="Avenida"> 
                                                                    Avenida 
                                                                </option>
                                                            </select>
                                                        </div>
                                                    </div>

                                                    <div class="row mt-3">
                                                        <div class="col-xl-6 col-lg-6 col-md-6 col-sm-12">
                                                            <label for="nameAddress"> 
                                                                Address: 
                                                            </label>

                                                            <input v-model="house.nameAddress" type="text" class="form-control form-control-sm" id="nameAddress">
                                                        </div>

                                                        <div class="col-xl-3 col-lg-3 col-md-6 col-sm-12">
                                                            <label for="numberAddress"> 
                                                                Number: 
                                                            </label>

                                                            <input v-model="house.numberAddress" type="text" class="form-control form-control-sm" id="numberAddress">
                                                        </div>
                                                    </div>

                                                    <div class="row mt-3">
                                                        <div class="col-xl-5 col-lg-5 col-md-6 col-sm-12">
                                                            <label for="periodDaysReport"> 
                                                                Period Days Report: 
                                                            </label>

                                                            <select v-model="house.periodDaysReport" class="form-control form-control-sm" id="periodDaysReport">
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
                                                    <button @click="register" type="button" class="btn btn-primary btn-sm"> 
                                                        Register 
                                                    </button>

                                                    <button type="button" class="btn btn-dark btn-sm" data-dismiss="modal">
                                                        Close
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
    import NavBarOptions from "../shared/NavBarOptions.vue";
    import NavBarUser from "../shared/NavBarUser.vue";

    export default {
        components: { 
            NavBarOptions, 
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
                            this.closeModal();

                            this.getHouses();

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

            getHouses() {
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

            closeModal() {
                $('#exampleModal').modal('hide');
            }
        },

        created() {
            this.closeModal();
            this.getHouses();
        }
    }
</script>

<style scoped>
    .teste {
        margin-right: 2px;
    }
</style>
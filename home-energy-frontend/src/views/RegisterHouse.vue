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
                                        <h3 class="mb-0"> Register House(s) </h3>
                                    </div>

                                    <div class="card-body px-5">
                                        <div class="icon-examples">
                                            <div>
                                                <div class="row">
                                                    <div class="col-6">
                                                        <label for="houseName"> Name: </label>

                                                        <input v-model="house.name" type="text" class="form-control form-control-sm" id="houseName">
                                                    </div>

                                                    <div class="col-6">
                                                        <label for="typeAddress"> Type Address: </label>

                                                        <select v-model="house.typeAddress" class="form-control form-control-sm" id="typeAddress">
                                                            <option value="Rua"> Rua </option>
                                                            <option value="Avenida"> Avenida </option>
                                                        </select>
                                                    </div>
                                                </div>

                                                <div class="row mt-3">
                                                    <div class="col-6">
                                                        <label for="nameAddress"> Address: </label>

                                                        <input v-model="house.nameAddress" type="text" class="form-control form-control-sm" id="nameAddress">
                                                    </div>

                                                    <div class="col-2">
                                                        <label for="numberAddress"> Number: </label>

                                                        <input v-model="house.numberAddress" type="text" class="form-control form-control-sm" id="numberAddress">
                                                    </div>
                                                </div>

                                                <div class="row mt-3">
                                                    <div class="col-3">
                                                        <label for="periodDaysReport"> Period Days Report: </label>

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

                                                <div class="row mt-5">
                                                    <div class="col-3">
                                                        <button @click="register" class="btn btn-primary btn-sm">
                                                            Register
                                                        </button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <span class="text-white"> {{ house }} </span>
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
                house: {
                    id: 0,
                    idUser: this.$store.state.user.id,
                    name: 'Casa da praia',
                    typeAddress: 'Rua',
                    nameAddress: 'Laranjeira',
                    numberAddress: 2000,
                    periodDaysReport: 2
                }
            }
        },

        methods: {
            register() {
                this.$http.post('/api/house/create', this.house)
                    .then((response) => {
                        console.log(response);
                    })
                    .catch((error) => {
                        console.error(error);
                    })
            }
        }
    }
</script>

<style scoped></style>
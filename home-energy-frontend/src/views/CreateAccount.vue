<template>
    <div id="create-account">
        <div class="bg-primary text-primary">
            teste
        </div>

        <div class="container border rounded bg-white mt-5">
            <div class="text-center">
                <h2> Create an Account </h2>
            </div>

            <div class="row align-items-center mt-4">             
                <div class="col-lg-6 col-md-12 mb-md-0">
                    <img src="../assets/image/banner.jpg" class="img-fluid mb-3 d-none d-lg-block">
                </div>

                <div class="col-lg-6 col-md-12 ml-auto">
                    <form @submit.prevent="register" class="row">
                        <div class="col-lg-6 col-md-6 mb-4">
                            <input v-model="user.name" id="name" type="text" name="name" placeholder="Name" class="form-control" required>
                        </div>

                        <div class="col-lg-6 col-md-6 mb-4">
                            <input v-model="user.username" id="username" type="text" name="username" placeholder="Username" class="form-control" required>
                        </div>

                        <div class="col-lg-8 col-md-4 mb-4">
                            <input v-model="user.cpf" id="cpf" type="text" name="cpf" placeholder="CPF" class="form-control" required>
                        </div>

                        <div class="col-lg-12 col-md-8 mb-4">
                            <input v-model="user.email" id="email" type="email" name="email" placeholder="Email Address" class="form-control" required>
                        </div>

                        <div class="col-lg-9 col-md-8 mb-4">
                            <input v-model="user.password" id="password" type="password" name="password" placeholder="Password" class="form-control" required>
                        </div>

                        <div v-if="status == 200" class="col-12 my-4">
                            <div class="alert alert-success alert-dismissible fade show" role="alert">
                                <div class="text-center"> 
                                    <b> Cadastro realizado com sucesso </b> 
                                </div>
                                
                                <div class="text-center">
                                    <small> 
                                        Você será redirecionado para a tela de login em alguns instantes 
                                    </small>
                                </div>
                                
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                    <span aria-hidden="true"> &times; </span>
                                </button>
                            </div>
                        </div>

                        <div class="form-group col-lg-12 mx-auto mb-0">
                            <button type="submit" class="btn btn-primary btn-block py-2">
                                Create your account
                            </button>
                        </div>

                        <div class="form-group col-lg-12 mx-auto d-flex align-items-center my-4">
                            <div class="border-bottom w-100 ml-5"></div>

                            <span class="px-2 small text-muted font-weight-bold text-muted">
                                OR
                            </span>

                            <div class="border-bottom w-100 mr-5"></div>
                        </div>

                        <div class="text-center w-100">
                            <p class="text-muted">
                                Already Registered? 
                                
                                <router-link to="/" class="text-primary ml-2">
                                    Login
                                </router-link>
                            </p>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "RegisterUser",

        data() {
            return {
                status: 0,

                user: {
                    id: 0,
                    name: '',
                    username: '',
                    password: '',
                    cpf: '',
                    email: ''
                }
            }
        },

        methods: {
            register() {
                this.$http.post("/api/user/create", this.user)
                    .then((response) => {
                        if(response.status == 200) {
                            setTimeout(() => {
                                this.$router.push({name: "login"});
                            }, 3500);

                            this.status = 200;
                        }
                    })
                    .catch((error) => {
                        console.error(error);
                    })
            }
        }
    }
</script>

<style scoped>
    a:hover {
        text-decoration: none
    }

    #register {
        padding-top: 500;
    }
</style>
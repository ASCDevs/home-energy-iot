<template>
   <div class="container-fluid bg-gradient-primary"> 
        <div class="py-5">
            <h1 class="text-white text-center"> Register User </h1>

            <div class="row justify-content-center">
                <div class="col-lg-6 col-md-8">
                    <div class="card bg-secondary shadow border-0">
                        <div class="card-body px-lg-5 ">
                            <form role="form">
                                <div class="form-group">
                                    <div class="input-group input-group-alternative mb-3">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text">
                                                <i class="ni ni-hat-3"></i>
                                            </span>
                                        </div>

                                        <input v-model="user.name" class="form-control" placeholder="Name" type="text">
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="input-group input-group-alternative mb-3">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text">
                                                <i class="ni ni-hat-3"></i>
                                            </span>
                                        </div>

                                        <input v-model="user.username" class="form-control" placeholder="Username" type="text">
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="input-group input-group-alternative">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text">
                                                <i class="ni ni-lock-circle-open"></i>
                                            </span>
                                        </div>

                                        <input v-model="user.password" type="password" class="form-control" placeholder="Password">
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="input-group input-group-alternative">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text">
                                                <i class="ni ni-lock-circle-open"></i>
                                            </span>
                                        </div>

                                        <input v-model="user.cpf" type="text" class="form-control" placeholder="CPF">
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="input-group input-group-alternative mb-3">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text">
                                                <i class="ni ni-email-83"></i>
                                            </span>
                                        </div>

                                        <input v-model="user.email" class="form-control" placeholder="Email" type="email">
                                    </div>
                                </div>

                                <div class="row my-4">
                                    <div class="col-12">
                                        <div class="custom-control custom-control-alternative custom-checkbox">
                                            <input class="custom-control-input" id="customCheckRegister" type="checkbox">
                                            
                                            <label class="custom-control-label" for="customCheckRegister">
                                                <span class="text-muted">
                                                    I agree with the Privacy Policy 
                                                </span>
                                            </label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row my-4">
                                    <div class="col-12 alert alert-success alert-dismissible fade show" role="alert">
                                        <strong> Cadastro realizado com sucesso </strong> 
                                        
                                        Você será redirecionado para a tela de login em alguns instantes :)
                                        
                                        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>
                                </div>

                                <div class="text-center">
                                    <button @click="register" type="button" class="btn btn-primary mt-4">
                                        Create account
                                    </button>
                                </div>
                            </form>
                        </div>
                    </div>
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
               this.$http.post('/api/user/create', this.user)
                    .then((response) => {
                        console.log(response);

                        if(response.status == 200) {
                            setTimeout(() => {

                            }, 2000);
                        }
                    })
                    .catch((error) => {
                        console.error(error);
                    })
            }
        }
    }
</script>

<style scoped></style>
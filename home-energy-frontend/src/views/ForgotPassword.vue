<template>
    <div class="bg-primary">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-xl-10 col-lg-12 col-md-9">
                    <div class="card o-hidden border-0 shadow-lg my-5">
                        <div class="card-body p-0">
                            <div class="row">
                                <div class="col-lg-6 d-none d-lg-block">
                                    <img src="../assets/image/forgot_password.jpg" class="img-fluid">
                                </div>

                                <div class="col-lg-6">
                                    <div class="p-5">
                                        <div class="text-center">
                                            <h2 class="h4 text-gray-900 mb-2">
                                                Esqueceu sua senha?
                                            </h2>

                                            <p class="mb-4">
                                                Basta digitar seu endereço de e-mail abaixo e enviaremos 
                                                um link para redefinir sua senha!
                                            </p>
                                        </div>

                                        <form @submit.prevent="forgotPassword" class="user">
                                            <div class="form-group">
                                                <input v-model="email" type="email" class="form-control" id="exampleInputEmail" aria-describedby="emailHelp" placeholder="Digite seu endereço de e-mail..." required>
                                            </div>

                                            <button type="submit" class="btn btn-primary btn-block">
                                                Redefinir senha
                                            </button>
                                        </form>
                                        
                                        <hr>

                                        <div class="text-center">
                                            <router-link to="/create/account" class="text-primary small ml-2">
                                                Crie uma conta!
                                            </router-link>
                                        </div>

                                        <div class="text-center">
                                            <router-link to="/" class="text-primary small ml-2">
                                                Já tem uma conta? Login!
                                            </router-link>
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
    export default {
        data() {
            return {
                email: ''
            }
        },

        methods: {
            forgotPassword() {
                alert(this.email);
            }
        }
    }
</script>

<style scoped>
    a:hover {
        text-decoration: none
    }
</style>
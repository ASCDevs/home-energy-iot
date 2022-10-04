<template>
    <div class="container border rounded mt-3">
        <router-link to="/" class="text-left">
            <i class="fas fa-arrow-left"></i> Back to Login
        </router-link>

        <div class="container text-center">
            <h1> Create an Account </h1>
        </div>

        <div class="row align-items-center">                
            <div class="col-lg-6 col-md-6 mb-md-0">
                <img src="../assets/image/banner.jpg" class="img-fluid mb-3 d-none d-md-block">
            </div>

            <div class="col-md-7 col-lg-6 ml-auto">
                <form @submit.prevent="register" class="row">
                    <div class="col-lg-6 mb-4">
                        <input v-model="user.name" id="name" type="text" name="name" placeholder="Name" class="form-control" required>
                    </div>

                    <div class="col-lg-6 mb-4">
                        <input v-model="user.username" id="username" type="text" name="username" placeholder="Username" class="form-control" required>
                    </div>

                    <div class="col-lg-8 mb-4">
                        <input v-model="user.cpf" id="cpf" type="text" name="cpf" placeholder="CPF" class="form-control" required>
                    </div>

                    <div class="col-lg-12 mb-4">
                        <input v-model="user.email" id="email" type="email" name="email" placeholder="Email Address" class="form-control" required>
                    </div>

                    <div class="col-lg-9 mb-4">
                        <input v-model="user.password" id="password" type="password" name="password" placeholder="Password" class="form-control" required>
                    </div>

                    <div v-if="status == 200" class="my-4">
                        <div class="col-12 alert alert-success alert-dismissible fade show" role="alert">
                            <strong> Cadastro realizado com sucesso </strong> <br/>
                            
                            Você será redirecionado para a tela de login em alguns instantes :)
                            
                            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
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
                        <p class="text-muted font-weight-bold">
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
                this.$http.post('/api/user/create', this.user)
                    .then((response) => {
                        if(response.status == 200) {
                            setTimeout(() => {
                                this.$router.push({name: 'login'});
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
    .register {
        background: -webkit-linear-gradient(left, #3931af, #00c6ff);
        margin-top: 3%;
        padding: 3%;
    }

    .register-left{
        text-align: center;
        color: #fff;
        margin-top: 4%;
    }

    .register-left input{
        border: none;
        border-radius: 1.5rem;
        padding: 2%;
        width: 60%;
        background: #f8f9fa;
        font-weight: bold;
        color: #383d41;
        margin-top: 30%;
        margin-bottom: 3%;
        cursor: pointer;
    }

    .register-right {
        background: #f8f9fa;
        border-top-left-radius: 10% 50%;
        border-bottom-left-radius: 10% 50%;
    }

    .register-left img {
        margin-top: 15%;
        margin-bottom: 5%;
        width: 25%;
        -webkit-animation: mover 2s infinite  alternate;
        animation: mover 1s infinite  alternate;
    }

    @-webkit-keyframes mover {
        0% { transform: translateY(0); }
        100% { transform: translateY(-20px); }
    }

    @keyframes mover {
        0% { transform: translateY(0); }
        100% { transform: translateY(-20px); }
    }

    .register-left p {
        font-weight: lighter;
        padding: 12%;
        margin-top: -9%;
    }

    .register .register-form {
        padding: 10%;
        margin-top: 10%;
    }

    .btnRegister {
        float: right;
        margin-top: 10%;
        border: none;
        border-radius: 1.5rem;
        padding: 2%;
        background: #0062cc;
        color: #fff;
        font-weight: 600;
        width: 50%;
        cursor: pointer;
    }

    .register .nav-tabs {
        margin-top: 3%;
        border: none;
        background: #0062cc;
        border-radius: 1.5rem;
        width: 28%;
        float: right;
    }

    .register .nav-tabs .nav-link {
        padding: 2%;
        height: 34px;
        font-weight: 600;
        color: #fff;
        border-top-right-radius: 1.5rem;
        border-bottom-right-radius: 1.5rem;
    }

    .register .nav-tabs .nav-link:hover{
        border: none;
    }

    .register .nav-tabs .nav-link.active {
        width: 100px;
        color: #0062cc;
        border: 2px solid #0062cc;
        border-top-left-radius: 1.5rem;
        border-bottom-left-radius: 1.5rem;
    }

    .register-heading {
        text-align: center;
        margin-top: 8%;
        margin-bottom: -15%;
        color: #495057;
    }
</style>
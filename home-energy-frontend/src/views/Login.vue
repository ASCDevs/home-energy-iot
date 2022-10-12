<template>
    <div class="container pt-5">
        <div class="overlay" id="overlay">
			<div class="overlay-content">
				<span class="spinner"></span>
				
          		<p class="mt-3"> Autenticando... </p>
			</div>
		</div>

        <div class="row">
            <div class="offset-md-2 col-lg-5 col-md-7 offset-lg-4 offset-md-3">
                <div class="panel border bg-white rounded">
                    <div class="text-center">
                        <h3 class="pt-3 font-weight-bold">
                            Login
                        </h3>
                    </div>

                    <div class="panel-body p-3">
                        <form @submit.prevent="auth">
                            <div class="form-group py-1">
                                <div class="input-field"> 
                                    <span class="far fa-user px-2"></span> 
                                    
                                    <input v-model="loginModel.username" class="form form-control" type="text" placeholder="Usuário" required> 
                                </div>
                            </div>
                            
                            <div class="form-group py-1">
                                <div class="input-field">
                                    <span class="fas fa-lock px-2"></span> 
                                    
                                    <input v-model="loginModel.password" class="form form-control" type="password" placeholder="Senha" required>
                                </div>
                            </div>

                            <div v-show="errorAuth" class="container">
                                <div class="alert alert-danger text-center" role="alert">
                                    <small> {{ errorAuth }} </small>
                                </div>
                            </div>
                            
                            <button type="submit" class="btn btn-primary btn-block mt-3">
                                Login
                            </button>
                            
                            <div class="text-center pt-4 text-muted">
                                Não tem uma conta?
                                
                                <router-link to="/create/account" class="text-primary ml-2">
                                    Cadastrar-se
                                </router-link>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Login",

        data() {
            return {
                errorAuth: "",

                loginModel: {
                    username: "",
                    password: ""
                }
            }
        },

        methods: {
            auth() {
                $("#overlay").css("display", "block");

                this.$store.dispatch("login", this.loginModel)
                    .then(() => {
                        this.$router.push({name: "registerHouse"});
                    })
                    .catch((error) => {
                        $("#overlay").css("display", "none");

                        if(error.response.status == 400) {
                            this.errorAuth = error.response.data;
                        }
                    })
            }
        },

        beforeCreate() {
            $("#overlay").css("display", "none");
            $(".modal-backdrop").remove();
        },

        beforeUnmount() {
            console.log("beforeUnmount");
            $("#overlay").css("display", "none");
        }
    }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    a:hover {
        text-decoration: none
    }

    .btn.btn-primary {
        margin-top: 20px;
        border-radius: 15px
    }

    .input-field {
        border-radius: 5px;
        padding: 5px;
        display: flex;
        align-items: center;
        border: 1px solid #ddd;
        color: #4e73df;
    }

    input[type='text'], input[type='password'] {
        border: none;
        outline: none;
        box-shadow: none;
        width: 100%
    }

    .fa-eye-slash.btn {
        border: none;
        outline: none;
        box-shadow: none
    }

    /* Overlay */
    p {
	    color: white;
    }
            
    .spinner {
        width: 75px;
        height: 75px;
        display: inline-block;
        border-width: 2px;
        border-color: rgba(255, 255, 255, 0.05);
        border-top-color: #fff;
        animation: spin 1s infinite linear;
        border-radius: 100%;
        border-style: solid;
    }

    @keyframes spin {
        100% {
            transform: rotate(360deg);
        }
    }
                
    .overlay {
        height: 100%;
        width: 100%;
        display: none;
        position: fixed;
        z-index: 1;
        top: 0;
        left: 0;
        background-color: rgb(0, 0, 0);
        background-color: rgba(0, 0, 0, 0.9);
    }

    .overlay-content {
        position: relative;
        top: 25%;
        width: 100%;
        text-align: center;
        margin-top: 30px;
    }

    .overlay a {
        padding: 8px;
        text-decoration: none;
        font-size: 36px;
        color: #818181;
        display: block;
        transition: 0.3s;
    }

    .overlay a:hover, .overlay a:focus {
        color: #f1f1f1;
    }

    .overlay .closebtn {
        position: absolute;
        top: 20px;
        right: 45px;
        font-size: 60px;
    }
</style>
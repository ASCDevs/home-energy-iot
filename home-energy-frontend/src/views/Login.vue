<template>
    <div class="bg-default main-content">
        <div class="overlay" id="overlay">
			<div class="overlay-content">
				<span class="spinner"></span>
				
          		<p class="mt-3"> Autenticando... </p>
			</div>
		</div>

        <div class="header bg-gradient-primary py-7 py-lg-7">
            <div class="container">
                <div class="header-body text-center">
                    <div class="row justify-content-center">
                        <div class="col-lg-5 col-md-6">
                            <img src="../assets/image/logo_site.png" class="img-fluid" alt="logo_site">
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="container mt--7 pb-7">
            <div class="row justify-content-center">
                <div class="col-lg-5 col-md-7">
                    <div class="card bg-secondary shadow border-0">
                        <div class="card-body px-lg-5 py-lg-5">
                            <form @submit.prevent="auth" role="form">
                                <div class="form-group mb-3">
                                    <div class="input-group input-group-alternative">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text">
                                                <i class="ni ni-email-83"></i>
                                            </span>
                                        </div>

                                        <input type="text" v-model="loginModel.username" class="form-control" name="username" id="username" placeholder="Enter username">
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="input-group input-group-alternative">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text">
                                                <i class="ni ni-lock-circle-open"></i>
                                            </span>
                                        </div>

                                        <input type="password" v-model="loginModel.password" class="form-control" name="password" id="password" placeholder="Enter password">
                                    </div>
                                </div>

                                <div class="text-center">
                                    <button type="submit" class="btn btn-primary btn-block my-4">
                                        Sign in
                                    </button>
                                </div>

                                <div v-if="errorAuth" class="alert alert-danger" role="alert">
                                    {{ errorAuth }}
                                </div>

                                <div class="row mt-3">
                                    <div class="col-6">
                                        <router-link to="/" class="text-muted">
                                            <small> Forgot password? </small>
                                        </router-link>
                                    </div>

                                    <div class="col-6 text-right">
                                        <router-link :to="{name: 'registerUser'}" class="text-muted">
                                            <small> Create new account </small>
                                        </router-link>
                                    </div>
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
        name: "Auth",

        data() {
            return {
                errorAuth: '',

                loginModel: {
                    username: '',
                    password: ''
                }
            }
        },

        methods: {
            auth() {
                $("#overlay").css("display", "block");

                this.$store.dispatch('login', this.loginModel)
                    .then(() => {
                        this.$router.push({name: 'about'});
                    })
                    .catch((error) => {
                        $("#overlay").css("display", "none");

                        if(error.request.status == 400) {
                            this.errorAuth = error.response.data;
                        }
                    })
            }
        },

        mounted() {
            $("#overlay").css("display", "none");
        },
    };
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .main-content {
        height: 100%;
        margin: 0;
    }

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
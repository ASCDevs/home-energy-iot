<template>
    <div class="container">
        <div class="overlay" id="overlay">
			<div class="overlay-content">
				<span class="spinner"></span>
				
          		<p class="mt-3"> Autenticando... </p>
			</div>
		</div>

        <div class="row">
            <div class="offset-md-2 col-lg-5 col-md-7 offset-lg-4 offset-md-3">
                <div class="panel border bg-white">
                    <div class="panel-heading">
                        <h3 class="pt-3 font-weight-bold">
                            Login
                        </h3>
                    </div>

                    <div class="panel-body p-3">
                        <form @submit.prevent="auth">
                            <div class="form-group py-1">
                                <div class="input-field"> 
                                    <span class="far fa-user px-2"></span> 
                                    
                                    <input v-model="loginModel.username" class="form form-control" type="text" placeholder="Username"> 
                                </div>
                            </div>
                            
                            <div class="form-group py-1">
                                <div class="input-field">
                                    <span class="fas fa-lock px-2"></span> 
                                    
                                    <input v-model="loginModel.password" class="form form-control" type="password" placeholder="Enter your Password">
                                </div>
                            </div>

                            <div v-if="errorAuth != ''" class="container">
                                <div class="alert alert-danger text-center" role="alert">
                                    {{ errorAuth }}
                                </div>
                            </div>
                            
                            <div class="form-inline"> 
                                <button type="button" class="btn btn-link btn-sm" id="forgot" data-toggle="modal" data-target="#exampleModal">
                                    Forgot password?
                                </button>
                            </div>
                            
                            <button type="submit" class="btn btn-primary btn-block mt-3">
                                Login
                            </button>
                            
                            <div class="text-center pt-4 text-muted">
                                Don't have an account?
                                
                                <router-link to="/register-user" class="text-primary ml-2">
                                    Sign up
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
                        this.$router.push({name: 'registerHouse'});
                    })
                    .catch((error) => {
                        $("#overlay").css("display", "none");

                        if(error.response.status == 400) {
                            this.errorAuth = error.response.data;
                        }
                    })
            }
        },

        created() {
            $("#overlay").css("display", "none");
        },
    };
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .container {
        margin: 50px auto
    }

    .panel-heading {
        text-align: center;
        margin-bottom: 10px
    }

    #forgot {
        min-width: 100px;
        margin-left: auto;
        text-decoration: none
    }

    a:hover {
        text-decoration: none
    }

    .form-inline label {
        padding-left: 10px;
        margin: 0;
    }

    .btn.btn-primary {
        margin-top: 20px;
        border-radius: 15px
    }

    .panel {
        min-height: 380px;
        box-shadow: 20px 20px 80px rgb(218, 218, 218);
        border-radius: 12px
    }

    .input-field {
        border-radius: 5px;
        padding: 5px;
        display: flex;
        align-items: center;
        cursor: pointer;
        border: 1px solid #ddd;
        color: #4343ff
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

    img {
        width: 40px;
        height: 40px;
        object-fit: cover;
        border-radius: 50%;
        position: relative
    }

    a[target='_blank'] {
        position: relative;
        transition: all 0.1s ease-in-out
    }

    .bordert {
        border-top: 1px solid #aaa;
        position: relative
    }

    .bordert:after {
        content: "or connect with";
        position: absolute;
        top: -13px;
        left: 33%;
        background-color: #fff;
        padding: 0px 8px
    }

    @media(max-width: 360px) {
        #forgot {
            margin-left: 0;
            padding-top: 10px
        }

        body {
            height: 100%
        }

        .container {
            margin: 30px 0
        }

        .bordert:after {
            left: 25%
        }
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
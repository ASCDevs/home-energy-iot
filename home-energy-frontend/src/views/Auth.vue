<template>
    <div class="container border mt-3">
        <div class="overlay" id="overlay">
			<div class="overlay-content">
				<span class="spinner"></span>
				
          		<p class="mt-3"> Autenticando... </p>
			</div>
		</div>

        <form class="p-3">
            <div class="form-group">
                <label for="username"> Login </label>
                
                <input type="text" v-model="loginModel.username" class="form-control" name="username" id="username" placeholder="Enter login">
                
                <small id="emailHelp" class="form-text text-muted">
                    We'll never share your email with anyone else.
                </small>
            </div>

            <div class="form-group">
                <label for="password"> Password </label>
                
                <input type="password" v-model="loginModel.password" class="form-control" name="password" id="password" placeholder="Enter password">
            </div>

            <div v-if="errorAuth" class="alert alert-danger" role="alert">
                {{ errorAuth }}
            </div>

            <button @click="auth" type="button" class="btn btn-primary"> Sign In </button>
        </form>
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
                let overlay = document.getElementById("overlay");

                overlay.style.display = "block";

                this.$store.dispatch('login', this.loginModel)
                    .then(() => {
                        this.$router.push({name: 'about'});
                    })
                    .catch((error) => {
                        overlay.style.display = "none";

                        if(error.request.status == 400) {
                            this.errorAuth = error.response.data;
                        }
                    })
            }
        }
    };
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
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
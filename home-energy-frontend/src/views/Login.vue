<template>
    <div>
        <h1> Login </h1>
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
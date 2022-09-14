<template>
    <div class="container border mt-3">
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
                this.$store.dispatch('login', this.loginModel)
                    .then((response) => {
                        this.$router.push({name: 'about'});
                    })
                    .catch((error) => {
                        if(error.request.status == 400) {
                            this.errorAuth = error.response.data;
                        }
                    })
            }
        }
    };
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped></style>
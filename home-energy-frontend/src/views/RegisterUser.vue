<template>
   <div> 
        
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
                            this.status = 200;

                            setTimeout(() => {

                            }, 2000);
                        }
                    })
                    .catch((error) => {
                        console.error(error);
                    })
            },
        }
    }
</script>

<style scoped></style>
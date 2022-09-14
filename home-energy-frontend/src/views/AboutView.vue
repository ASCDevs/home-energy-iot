<template>
    <div class="about p-4">
        <h1> This is an about page </h1>
        
        <button @click="logout" class="btn btn-danger btn-sm mt-5"> Sair </button>
    </div>
</template>

<script>
    export default {
        name: "Auth",
        
        methods: {
            logout() {
                this.$store.commit('LOGOUT_USER');
                this.$router.push({name: 'auth'});
            }
        }
    }
</script>
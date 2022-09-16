<template>
    <div class="about p-4">
        <button @click="logout" class="btn btn-danger btn-sm mt-5"> Sair </button>
    </div>
</template>

<script>
    // site para ajudar signal
    // https://github.com/alexandremalavasi/DemoYoutubeSignalRVueJS/tree/master/ClientApp/src/components

    // video
    // https://www.youtube.com/watch?v=YeO4udwi11w

    import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr';

    export default {
        name: "Auth",

        data() {
            return {
                signalRConnection: null
            }
        },
        
        methods: {
            configureSignalR() {
                this.signalRConnection = new HubConnectionBuilder()
                    .withUrl("https://servicehomeiotmonitoring.azurewebsites.net/costumerhub")
                    .configureLogging(LogLevel.Information)
                    .build();

                this.signalRConnection.start().then(() => {
                    console.log('Connection started');
                });
            },

            logout() {
                this.$store.commit('LOGOUT_USER');
                this.$router.push({name: 'auth'});
            }
        },

        mounted() {
            console.log('mounted');

            this.configureSignalR();
        }
    }
</script>
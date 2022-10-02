<template>
    <div>
        <h1> About </h1>
    </div>
</template>

<script>
    import { HubConnectionBuilder, LogLevel } from "@aspnet/signalr";

    import Sidebar from "../shared/Sidebar.vue";
    import NavBarUser from "../shared/NavBarUser.vue";

    export default {
        name: "AboutView",

        components: { 
            Sidebar, 
            NavBarUser 
        },

        data() {
            return {
                signalRConnection: null
            };
        },

        methods: {
            configureSignalR() {
                this.signalRConnection = new HubConnectionBuilder()
                    .withUrl("https://servicehomeiotmonitoring.azurewebsites.net/costumerhub")
                    .configureLogging(LogLevel.Information)
                    .build();
                    
                this.signalRConnection.start().then(() => {
                    console.log("Connection started");
                });
            }
        },

        mounted() {
            console.log("mounted");
            this.configureSignalR();
        }
}
</script>

<style scoped></style>
<template>
    <div>
        <NavBarOptions/>

        <div class="main-content">
            <NavBarUser />
	
            <div class="header bg-gradient-primary pb-8 pt-5 pt-md-8">
                <div class="container-fluid">
                    <div class="header-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="card shadow">
                                    <div class="card-header bg-transparent">
                                        <h3 class="mb-0"> Authenticated </h3>
                                    </div>

                                    <div class="card-body">
                                        <div class="row icon-examples"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import { HubConnectionBuilder, LogLevel } from "@aspnet/signalr";

    import NavBarOptions from "../shared/NavBarOptions.vue";
    import NavBarUser from "../shared/NavBarUser.vue";

    export default {
        name: "AboutView",

        components: { 
            NavBarOptions, 
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
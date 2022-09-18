<template>
    <div>
        <nav class="navbar navbar-vertical fixed-left navbar-expand-md navbar-light bg-white" id="sidenav-main">
            <div class="container-fluid">
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#sidenav-collapse-main" aria-controls="sidenav-main" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                
                <div class="d-none d-xl-block d-lg-block">
                    <img src="../../public/logo-site.jpg" class="img-thumbnail" alt="logo">
                </div>

                <ul class="nav align-items-center d-md-none">
                    <li class="nav-item dropdown">
                        <a class="nav-link nav-link-icon" href="#" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <i class="ni ni-bell-55"></i>
                        </a>

                        <div class="dropdown-menu dropdown-menu-arrow dropdown-menu-right" aria-labelledby="navbar-default_dropdown_1">
                            <a class="dropdown-item" href="#">Action</a>

                            <a class="dropdown-item" href="#">Another action</a>

                            <div class="dropdown-divider"></div>

                            <a class="dropdown-item" href="#">Something else here</a>
                        </div>
                    </li>

                    <li class="nav-item dropdown">
                        <a class="nav-link" href="#" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            <div class="media align-items-center">
                                <span class="avatar avatar-sm rounded-circle">
                                    <img alt="Image placeholder" src="../../public/user.png">
                                </span>
                            </div>
                        </a>

                        <div class="dropdown-menu dropdown-menu-arrow dropdown-menu-right">
                            <div class=" dropdown-header noti-title">
                                <h6 class="text-overflow m-0">Welcome!</h6>
                            </div>
            
                            <a href="./examples/profile.html" class="dropdown-item">
                                <i class="ni ni-single-02"></i>
                                <span>My profile</span>
                            </a>

                            <a href="./examples/profile.html" class="dropdown-item">
                                <i class="ni ni-settings-gear-65"></i>
                                <span>Settings</span>
                            </a>

                            <a href="./examples/profile.html" class="dropdown-item">
                                <i class="ni ni-calendar-grid-58"></i>
                                <span>Activity</span>
                            </a>

                            <a href="./examples/profile.html" class="dropdown-item">
                                <i class="ni ni-support-16"></i>
                                <span>Support</span>
                            </a>

                            <div class="dropdown-divider"></div>
                            
                            <a @click="logout" type="button" class="dropdown-item">
                                <i class="ni ni-user-run"></i>
                                <span>Logout</span>
                            </a>
                        </div>
                    </li>
                </ul>

                <div class="collapse navbar-collapse" id="sidenav-collapse-main">
                    <div class="navbar-collapse-header d-md-none">
                        <div class="row">
                            <div class="col-6 collapse-brand">
                                <a href="./index.html">
                                    <img src="../../public/user.png">
                                </a>
                            </div>

                            <div class="col-6 collapse-close">
                                <button type="button" class="navbar-toggler" data-toggle="collapse" data-target="#sidenav-collapse-main" aria-controls="sidenav-main" aria-expanded="false" aria-label="Toggle sidenav">
                                    <span></span>
                                    <span></span>
                                </button>
                            </div>
                        </div>
                    </div>

                    <form class="mt-4 mb-3 d-md-none">
                        <div class="input-group input-group-rounded input-group-merge">
                            <input type="search" class="form-control form-control-rounded form-control-prepended" placeholder="Search" aria-label="Search">
                            
                            <div class="input-group-prepend">
                                <div class="input-group-text">
                                    <span class="fa fa-search"></span>
                                </div>
                            </div>
                        </div>
                    </form>

                    <ul class="navbar-nav">
                        <li class="nav-item  active ">
                            <a class="nav-link  active " href="./index.html">
                                <i class="ni ni-tv-2 text-primary"></i> Dashboard
                            </a>
                        </li>

                        <li class="nav-item">
                            <a class="nav-link " href="./examples/icons.html">
                                <i class="ni ni-planet text-blue"></i> Icons
                            </a>
                        </li>

                        <li class="nav-item">
                            <a class="nav-link " href="./examples/maps.html">
                                <i class="ni ni-pin-3 text-orange"></i> Maps
                            </a>
                        </li>

                        <li class="nav-item">
                            <a class="nav-link " href="./examples/profile.html">
                                <i class="ni ni-single-02 text-yellow"></i> User profile
                            </a>
                        </li>

                        <li class="nav-item">
                            <a class="nav-link " href="./examples/tables.html">
                                <i class="ni ni-bullet-list-67 text-red"></i> Tables
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <div class="main-content">
            <nav class="navbar navbar-top navbar-expand-md navbar-dark" id="navbar-main">
                <div class="container-fluid">
                    <a class="h4 mb-0 text-white text-uppercase d-none d-lg-inline-block" href="./index.html">
                        Dashboard
                    </a>

                    <form class="navbar-search navbar-search-dark form-inline mr-3 d-none d-md-flex ml-lg-auto">
                        <div class="form-group mb-0">
                            <div class="input-group input-group-alternative">
                                <div class="input-group-prepend">
                                    <span class="input-group-text">
                                        <i class="fas fa-search"></i>
                                    </span>
                                </div>

                                <input class="form-control" placeholder="Search" type="text">
                            </div>
                        </div>
                    </form>

                    <ul class="navbar-nav align-items-center d-none d-md-flex">
                        <li class="nav-item dropdown">
                            <a class="nav-link pr-0" href="#" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                <div class="media align-items-center">
                                    <span class="avatar avatar-sm rounded-circle">
                                        <img alt="Image placeholder" src="../../public/user.png">
                                    </span>

                                    <div class="media-body ml-2 d-none d-lg-block">
                                        <span class="mb-0 text-sm  font-weight-bold">Jessica Jones</span>
                                    </div>
                                </div>
                            </a>

                            <div class="dropdown-menu dropdown-menu-arrow dropdown-menu-right">
                                <div class=" dropdown-header noti-title">
                                    <h6 class="text-overflow m-0">Welcome!</h6>
                                </div>

                                <a href="./examples/profile.html" class="dropdown-item">
                                    <i class="ni ni-single-02"></i>
                                    <span>My profile</span>
                                </a>

                                <a href="./examples/profile.html" class="dropdown-item">
                                    <i class="ni ni-settings-gear-65"></i>
                                    <span>Settings</span>
                                </a>

                                <a href="./examples/profile.html" class="dropdown-item">
                                    <i class="ni ni-calendar-grid-58"></i>
                                    <span>Activity</span>
                                </a>

                                <a href="./examples/profile.html" class="dropdown-item">
                                    <i class="ni ni-support-16"></i>
                                    <span>Support</span>
                                </a>

                                <div class="dropdown-divider"></div>

                                <a @click="logout" type="button" class="dropdown-item">
                                    <i class="ni ni-user-run"></i>
                                    <span>Logout</span>
                                </a>
                            </div>
                        </li>
                    </ul>
                </div>
            </nav>
	
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
                                        <div class="row icon-examples">

                                        </div>
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
    import "@/assets/vendor/js/plugins/bootstrap/dist/js/bootstrap.bundle.min.js";
    import "@/assets/vendor/js/argon-dashboard.min.js?v=1.1.2";

    // site para ajudar signal
    // https://github.com/alexandremalavasi/DemoYoutubeSignalRVueJS/tree/master/ClientApp/src/components

    // video
    // https://www.youtube.com/watch?v=YeO4udwi11w

    import { HubConnectionBuilder, LogLevel } from "@aspnet/signalr";

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
                    console.log("Connection started");
                });
            },

            logout() {
                this.$store.commit("LOGOUT_USER");
                this.$router.push({name: "auth"});
            }
        },

        mounted() {
            console.log("mounted");
            this.configureSignalR();
        }
    }
</script>
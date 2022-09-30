<template>
    <div id="page-top">
        <div id="wrapper">
            <Sidebar/>

            <div id="content-wrapper" class="d-flex flex-column">
                <div id="content">
                    <NavBarUser/>
                    
                    <div class="container-fluid">
                        <div class="d-sm-flex align-items-center justify-content-end mb-4">
                            <a href="#" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm">
                                <i class="fas fa-download fa-sm text-white-50"></i> Generate Report
                            </a>
                        </div>

                        <div class="row">
                            <div class="col-xl-12 col-lg-12">
                                <div class="card shadow mb-4">
                                    <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                                        <h6 class="m-0 font-weight-bold text-primary">
                                            Houses
                                        </h6>
                                        
                                        <div class="dropdown no-arrow">
                                            <a class="dropdown-toggle" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                <i class="fas fa-ellipsis-v fa-sm fa-fw text-gray-400"></i>
                                            </a>

                                            <div class="dropdown-menu dropdown-menu-right shadow animated--fade-in" aria-labelledby="dropdownMenuLink">
                                                <div class="dropdown-header">
                                                    Dropdown Header:
                                                </div>

                                                <a class="dropdown-item" href="#">Action</a>

                                                <a class="dropdown-item" href="#">Another action</a>

                                                <div class="dropdown-divider"></div>

                                                <a class="dropdown-item" href="#">Something else here</a>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="card-body">
                                        <div class="chart-area">
                                            <canvas id="myAreaChart"></canvas>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                
                <footer class="sticky-footer bg-white">
                    <div class="container my-auto">
                        <div class="copyright text-center my-auto">
                            <span> Copyright &copy; PowerMetrics {{new Date().getFullYear()}} </span>
                        </div>
                    </div>
                </footer>
            </div>
        </div>
           
        <a class="scroll-to-top rounded" href="#page-top">
            <i class="fas fa-angle-up"></i>
        </a>

        <div class="modal fade" id="logoutModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel">
                            Ready to Leave?
                        </h5>

                        <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">×</span>
                        </button>
                    </div>

                    <div class="modal-body">
                        Select "Logout" below if you are ready to end your current session.
                    </div>
                    
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary btn-sm">
                            Logout
                        </button>

                        <button class="btn btn-secondary btn-sm" type="button" data-dismiss="modal">
                            Cancel
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import "../assets/vendor/bootstrap/js/bootstrap.bundle.min.js";
    import "../assets/vendor/jquery-easing/jquery.easing.min.js";
    import "../assets/vendor/sb-admin/js/sb-admin-2.min.js";

    import Sidebar from "@/shared/Sidebar.vue";
    import NavBarUser from "../shared/NavBarUser.vue";

    export default {
        components: { 
            Sidebar, 
            NavBarUser 
        },

        methods: {
            logout() {
                this.$store.commit("LOGOUT_USER");
                this.$router.push({ name: "login" });
            },

            ampliarNavBar() {
                let accordion = document.querySelector("#accordionSidebar");
                let body = document.querySelector("#page-top");

                if(!accordion.classList.contains("toggled") && !body.classList.contains("sidebar-toggled")) {
                    accordion.classList.add("toggled");
                    body.classList.add("sidebar-toggled");
                } else {
                    accordion.classList.remove("toggled");
                    body.classList.remove("sidebar-toggled");
                }
            }
        },
    }
</script>

<style scoped>
    @import url('https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i');

    #accordionSidebar {
        height: 100%;
    }
</style>
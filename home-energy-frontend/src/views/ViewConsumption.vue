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
                                            Realtime Consumption
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
    </div>
</template>

<script>
    import Sidebar from "../shared/Sidebar.vue";
    import NavBarUser from "../shared/NavBarUser.vue";

    export default {
        components: { 
            Sidebar, 
            NavBarUser 
        }
    }
</script>

<style scoped></style>
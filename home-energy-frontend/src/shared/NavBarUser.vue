<template>
    <nav class="navbar navbar-expand navbar-light bg-white topbar mb-4 static-top shadow">
        <button @click="ampliarNavBar" id="sidebarToggleTop" class="btn btn-link d-md-none rounded-circle mr-3">
            <i class="fa fa-bars"></i>
        </button>

        <ul class="navbar-nav ml-auto">
            <li class="nav-item dropdown no-arrow d-sm-none">
                <a class="nav-link dropdown-toggle" href="#" id="searchDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i class="fas fa-search fa-fw"></i>
                </a>

                <div class="dropdown-menu dropdown-menu-right p-3 shadow animated--grow-in" aria-labelledby="searchDropdown">
                    <form class="form-inline mr-auto w-100 navbar-search">
                        <div class="input-group">
                            <input type="text" class="form-control bg-light border-0 small" placeholder="Search for..." aria-label="Search" aria-describedby="basic-addon2">
                            
                            <div class="input-group-append">
                                <button class="btn btn-primary" type="button">
                                    <i class="fas fa-search fa-sm"></i>
                                </button>
                            </div>
                        </div>
                    </form>
                </div>
            </li>

            <li class="nav-item dropdown no-arrow mx-1">
                <a class="nav-link dropdown-toggle" href="#" id="alertsDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i class="fas fa-bell fa-fw"></i>

                    <span class="badge badge-danger badge-counter">3+</span>
                </a>

                <div class="dropdown-list dropdown-menu dropdown-menu-right shadow animated--grow-in" aria-labelledby="alertsDropdown">
                    <h6 class="dropdown-header">
                        Alerts Center
                    </h6>

                    <a class="dropdown-item d-flex align-items-center" href="#">
                        <div class="mr-3">
                            <div class="icon-circle bg-primary">
                                <i class="fas fa-file-alt text-white"></i>
                            </div>
                        </div>
                        <div>
                            <div class="small text-gray-500">December 12, 2019</div>
                            <span class="font-weight-bold">A new monthly report is ready to download!</span>
                        </div>
                    </a>
                    <a class="dropdown-item d-flex align-items-center" href="#">
                        <div class="mr-3">
                            <div class="icon-circle bg-success">
                                <i class="fas fa-donate text-white"></i>
                            </div>
                        </div>
                        <div>
                            <div class="small text-gray-500">December 7, 2019</div>
                            $290.29 has been deposited into your account!
                        </div>
                    </a>
                    <a class="dropdown-item d-flex align-items-center" href="#">
                        <div class="mr-3">
                            <div class="icon-circle bg-warning">
                                <i class="fas fa-exclamation-triangle text-white"></i>
                            </div>
                        </div>
                        <div>
                            <div class="small text-gray-500">December 2, 2019</div>
                            Spending Alert: We've noticed unusually high spending for your account.
                        </div>
                    </a>
                    <a class="dropdown-item text-center small text-gray-500" href="#">Show All Alerts</a>
                </div>
            </li>

            <li class="nav-item dropdown no-arrow mx-1">
                <a class="nav-link dropdown-toggle" href="#" id="messagesDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i class="fas fa-envelope fa-fw"></i>
                    <span class="badge badge-danger badge-counter">7</span>
                </a>

                <div class="dropdown-list dropdown-menu dropdown-menu-right shadow animated--grow-in" aria-labelledby="messagesDropdown">
                    
                    <h6 class="dropdown-header">
                        Message Center
                    </h6>

                    <a class="dropdown-item d-flex align-items-center" href="#">
                        <div class="dropdown-list-image mr-3">
                            <img class="rounded-circle" alt="...">

                            <div class="status-indicator bg-success"></div>
                        </div>
                        
                        <div class="font-weight-bold">
                            <div class="text-truncate">
                                Hi there! I am wondering if you can help me with a
                                problem I've been having.
                            </div>

                            <div class="small text-gray-500">
                                Emily Fowler · 58m
                            </div>
                        </div>
                    </a>

                    <a class="dropdown-item d-flex align-items-center" href="#">
                        <div class="dropdown-list-image mr-3">
                            <img class="rounded-circle" alt="...">

                            <div class="status-indicator"></div>
                        </div>

                        <div>
                            <div class="text-truncate">
                                I have the photos that you ordered last month, how
                                would you like them sent to you?
                            </div>

                            <div class="small text-gray-500">
                                Jae Chun · 1d
                            </div>
                        </div>
                    </a>

                    <a class="dropdown-item d-flex align-items-center" href="#">
                        <div class="dropdown-list-image mr-3">
                            <img class="rounded-circle" alt="...">

                            <div class="status-indicator bg-warning"></div>
                        </div>

                        <div>
                            <div class="text-truncate">
                                Last month's report looks great, I am very happy with
                                the progress so far, keep up the good work!
                            </div>

                            <div class="small text-gray-500">Morgan Alvarez · 2d</div>
                        </div>
                    </a>

                    <a class="dropdown-item d-flex align-items-center" href="#">
                        <div class="dropdown-list-image mr-3">
                            <img class="rounded-circle" alt="...">

                            <div class="status-indicator bg-success"></div>
                        </div>

                        <div>
                            <div class="text-truncate">
                                Am I a good boy? The reason I ask is because someone
                                told me that people say this to all dogs, even if they aren't good...
                            </div>

                            <div class="small text-gray-500">
                                Chicken the Dog · 2w
                            </div>
                        </div>
                    </a>

                    <a class="dropdown-item text-center small text-gray-500" href="#">
                        Read More Messages
                    </a>
                </div>
            </li>

            <div class="topbar-divider d-none d-sm-block"></div>

            <li class="nav-item dropdown no-arrow">
                <a class="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i class="fas fa-user text-dark-200 "></i>

                    <span class="mr-2 d-none d-lg-inline text-gray-600 small ml-2 mt-1">
                        {{ this.$store.state.user.name }}
                    </span>
                </a>

                <div class="dropdown-menu dropdown-menu-right shadow animated--grow-in" aria-labelledby="userDropdown">
                    <a class="dropdown-item" href="#">
                        <i class="fas fa-user fa-sm fa-fw mr-2 text-gray-400"></i> Profile
                    </a>

                    <a class="dropdown-item" href="#">
                        <i class="fas fa-cogs fa-sm fa-fw mr-2 text-gray-400"></i> Settings
                    </a>

                    <a class="dropdown-item" href="#">
                        <i class="fas fa-list fa-sm fa-fw mr-2 text-gray-400"></i> Activity Log
                    </a>

                    <div class="dropdown-divider"></div>

                    <a class="dropdown-item" href="#" data-toggle="modal" data-target="#logoutModal">
                        <i class="fas fa-sign-out-alt fa-sm fa-fw mr-2 text-gray-400"></i> Logout
                    </a>
                </div>
            </li>
        </ul>

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
                        <button @click="logout" type="button" class="btn btn-danger btn-sm">
                            Logout
                        </button>

                        <button class="btn btn-secondary btn-sm" type="button" data-dismiss="modal">
                            Cancel
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </nav>
</template>

<script>
    export default {
        name: "NavBarUser",

        methods: {
            logout() {
                this.$store.commit("LOGOUT_USER");
                this.$router.push({name: "login"});
            },

            ampliarNavBar() {
                let accordion = document.querySelector('#accordionSidebar');
                let body = document.querySelector('#page-top');

                if(!accordion.classList.contains('toggled') && !body.classList.contains('sidebar-toggled')) {
                    accordion.classList.add('toggled');
                    body.classList.add('sidebar-toggled');
                } else {
                    accordion.classList.remove('toggled');
                    body.classList.remove('sidebar-toggled');
                }
            }
        }
    }
</script>

<style scoped></style>
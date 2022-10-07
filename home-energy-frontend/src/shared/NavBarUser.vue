<template>
    <nav class="navbar navbar-expand navbar-light bg-white topbar mb-4 static-top shadow">
        <button @click="ampliarNavBar" id="sidebarToggleTop" class="btn btn-link d-md-none rounded-circle mr-3">
            <i class="fa fa-bars"></i>
        </button>

        <ul class="navbar-nav ml-auto">
            <li class="nav-item dropdown no-arrow">
                <a class="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i class="fas fa-user text-dark-200 "></i>

                    <span class="mr-2 text-gray-600 small ml-2 mt-1">
                        {{ this.$store.state.user.name }}
                    </span>
                </a>

                <div class="dropdown-menu dropdown-menu-right shadow animated--grow-in" aria-labelledby="userDropdown">
                    <a type="button" class="dropdown-item" data-toggle="modal" data-target="#logoutModal">
                        <i class="fas fa-sign-out-alt fa-sm fa-fw mr-2 text-gray-400"></i> Sair
                    </a>
                </div>
            </li>
        </ul>

        <div class="modal fade" id="logoutModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel">
                            Deseja sair da aplicação?
                        </h5>

                        <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true"> × </span>
                        </button>
                    </div>

                    <div class="modal-footer">
                        <button @click="logout" type="button" class="btn btn-danger btn-sm">
                            Sair
                        </button>

                        <button class="btn btn-secondary btn-sm" type="button" data-dismiss="modal">
                            Cancelar
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
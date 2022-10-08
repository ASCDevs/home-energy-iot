<template>
    <div id="page-top">
        <div id="wrapper">
            <Sidebar/>

            <div id="content-wrapper" class="d-flex flex-column">
                <div id="content">
                    <NavBarUser/>
                    
                    <div class="container-fluid">
                        <div class="text-center">
                            <div class="error mx-auto" data-text="404">
                                404
                            </div>

                            <p class="lead text-gray-800 mb-5"> 
                                Página não encontrada
                            </p>
                            
                            <router-link to="/house/register">
                                &larr; Voltar para homepage
                            </router-link>
                        </div>
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
        }
    }
</script>

<style scoped></style>
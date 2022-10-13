<template>
    <nav id="sidebar-main">
        <ul class="navbar-nav bg-gradient-primary sidebar sidebar-dark accordion" id="accordionSidebar">
            <div class="sidebar-brand d-flex align-items-center justify-content-center">
                <div class="sidebar-brand-icon">
                    <img src="../assets/image/icon_navbar.png" width="60" height="60" alt="icon">
                </div>

                <div class="sidebar-brand-text mx-3"> 
                    Energy
                </div>
            </div>

            <hr class="sidebar-divider">

            <div class="sidebar-heading">
                Interface
            </div>

            <li class="nav-item">
                <router-link :to="{name: 'registerHouse'}" class="nav-link">
                    <i class="fas fa-laptop-house"></i> 
                    <span> Casas </span>
                </router-link>
            </li>

            <hr class="sidebar-divider d-none d-md-block">

            <div class="text-center d-none d-md-inline">
                <button @click="ampliarNavBar" class="rounded-circle border-0" id="sidebarToggle"></button>
            </div>
        </ul>
    </nav>
</template>

<script>
    export default {
        name: "Sidebar",

        methods: {
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

        mounted() {
            let accordion = document.querySelector("#accordionSidebar");

            let body = document.querySelector("#page-top");

            accordion.classList.add("toggled");

            body.classList.add("sidebar-toggled");
        }
    }
</script>

<style scoped>
    #accordionSidebar {
        height: 100%;
    }

    #accordionSidebar li:hover {
        background-color: rgb(56, 124, 250);
    }
</style>
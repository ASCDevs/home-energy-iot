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
                                        <h3 class="mb-0"> Consumption </h3>
                                    </div>

                                    <div class="card-body px-5">
                                        <div class="row icon-examples">
                                            <img src="https://i.pinimg.com/originals/f8/8a/ca/f88acab7ffd127b4465659500aa0538f.gif" class="img-fluid rounded mx-auto d-block" />
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
    import NavBarOptions from "../shared/NavBarOptions.vue";
    import NavBarUser from "../shared/NavBarUser.vue";

    export default {
        components: { 
            NavBarOptions, 
            NavBarUser 
        }
    }
</script>

<style scoped></style>
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
                                        <h3 class="mb-0"> Register House(s) </h3>
                                    </div>

                                    <div class="card-body px-5">
                                        <div class="row icon-examples">
                                            <div class="alert alert-success" role="alert">
                                                <h4 class="alert-heading">Well done!</h4>
                                                
                                                <p>Aww yeah, you successfully read this important alert message. This example text is going to run a bit longer so that you can see how spacing within an alert works with this kind of content.</p>
                                                
                                                <hr>
                                                
                                                <p class="mb-0">Whenever you need to, be sure to use margin utilities to keep things nice and tidy.</p>
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
import { createRouter, createWebHistory } from "vue-router";
import Login from "../views/Login.vue";
import store from '@/store/index';

const routes = [
    {
        path: "/",

        name: "login",

        meta: {
            public: true
        },

        component: Login
    },

    {
        path: "/house/register",

        name: "registerHouse",

        title: "House",

        meta: {
            public: false
        },

        component: () => import(/* webpackChunkName: "registerHouse" */ "../views/House.vue")
    },

    {
        path: "/house/:id/devices",

        name: "devicesHouse",

        meta: {
            public: false
        },

        component: () => import(/* webpackChunkName: "devicesHouse" */ "../views/Device.vue")
    },

    {
        path: "/create/account",

        name: "createAccount",

        meta: {
            public: true
        },

        component: () => import(/* webpackChunkName: "createAccount" */ "../views/CreateAccount.vue")
    },

    {
        path: "/device/register",

        name: "registerDevice",

        meta: {
            public: true
        },

        component: () => import(/* webpackChunkName: "registerDevice" */ "../views/Device.vue")
    },

    {
        path: "/device/:id/consumption",

        name: "consumptionDevice",

        meta: {
            public: false
        },

        component: () => import(/* webpackChunkName: "consumptionDevice" */ "../views/ConsumptionDevice.vue")
    },

    {
        path: "/report/device/:id",

        name: "reportDevice",

        meta: {
            public: false
        },

        component: () => import(/* webpackChunkName: "reportDevice" */ "../views/ReportDevice.vue")
    },

    {
        path: "/:pathMatch(.*)*",

        name: "notFound",

        meta: {
            publico: false
        },

        component: () => import(/* webpackChunkName: "notFound" */ "../views/NotFound.vue")
    },
];

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes
});

router.beforeEach((routeTo, routeFrom, next) => {    
    if(store.state.token == null && !routeTo.meta.public) {
        return next({name: "login"});
    }

    next();
});

export default router;
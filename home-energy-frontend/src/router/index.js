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

        component: Login,
    },
    
    {
        path: "/about",

        name: "about",

        meta: {
            public: false
        },

        component: () => import(/* webpackChunkName: "about" */ "../views/AboutView.vue"),
    },

    {
        path: "/register-house",

        name: "registerHouse",

        meta: {
            public: false
        },

        component: () => import(/* webpackChunkName: "registerHouse" */ "../views/RegisterHouse.vue"),
    },

    {
        path: "/view-consumption",

        name: "viewConsumption",

        meta: {
            public: false
        },

        component: () => import(/* webpackChunkName: "viewConsumption" */ "../views/ViewConsumption.vue"),
    },

    {
        path: "/register-user",

        name: "registerUser",

        meta: {
            public: true
        },

        component: () => import(/* webpackChunkName: "viewConsumption" */ "../views/RegisterUser.vue"),
    }
];

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes,
});

router.beforeEach((routeTo, routeFrom, next) => {    
    if(store.state.token == null && !routeTo.meta.public) {
        return next({name: 'login'});
    }

    next();
});

export default router;
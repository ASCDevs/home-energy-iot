import { createRouter, createWebHistory } from "vue-router";
import Login from "../views/Login.vue";
import store from '@/store/index';

const routes = [
    {
        path: "/",

        name: "login",

        component: Login,
    },
    
    {
        path: "/about",

        name: "about",

        component: () => import(/* webpackChunkName: "about" */ "../views/AboutView.vue"),
    },

    {
        path: "/register-house",

        name: "registerHouse",

        component: () => import(/* webpackChunkName: "registerHouse" */ "../views/RegisterHouse.vue"),
    },

    {
        path: "/view-consumption",

        name: "viewConsumption",

        component: () => import(/* webpackChunkName: "viewConsumption" */ "../views/ViewConsumption.vue"),
    }
];

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes,
});

router.beforeEach((routeTo, routeFrom, next) => {    
    if(routeTo.path != '/') {
        if(store.state.token == null) {
            return next({path: '/'});
        }
    }

    next();
});

export default router;
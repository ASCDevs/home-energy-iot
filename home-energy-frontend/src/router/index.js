import { createRouter, createWebHistory } from "vue-router";
import Auth from "../views/Auth.vue";
import store from '@/store/index';

const routes = [
    {
        path: "/",

        name: "auth",

        component: Auth,
    },
    
    {
        path: "/about",

        name: "about",

        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        
        component: () => import(/* webpackChunkName: "about" */ "../views/AboutView.vue"),
    },
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
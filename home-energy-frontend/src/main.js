import { createApp } from "vue";

import jQuery from "jquery";
global.$ = jQuery

import App from "./App.vue";
import router from "./router";
import store from "./store";
import http from "@/axios/http";

import "bootstrap/dist/css/bootstrap.min.css";
import "./assets/vendor/fontawesome-free/css/all.min.css";
import "./assets/vendor/sb-admin/css/sb-admin-2.min.css";

const app = createApp(App);

app.config.globalProperties.$http = http;
app.use(store);
app.use(router);

app.mount("#app");
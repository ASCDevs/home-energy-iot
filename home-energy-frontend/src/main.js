import { createApp } from "vue";

import jQuery from 'jquery';
global.$ = jQuery

import App from "./App.vue";
import router from "./router";
import store from "./store";
import http from "@/axios/http";

import "bootstrap/dist/css/bootstrap.min.css";
import "@/assets/vendor/js/plugins/nucleo/css/nucleo.css";
import "@/assets/vendor/css/argon-dashboard.min.css?v=1.1.2";

const app = createApp(App);

app.config.globalProperties.$http = http;
app.use(store);
app.use(router);

app.mount("#app");
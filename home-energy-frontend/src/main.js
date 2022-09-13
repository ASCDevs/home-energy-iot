import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import http from "@/axios/http";

import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap-vue/dist/bootstrap-vue.css';

const app = createApp(App);

app.config.globalProperties.$http = http;
app.use(store);
app.use(router);
app.mount('#app');
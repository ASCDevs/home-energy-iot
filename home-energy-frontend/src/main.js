import { createApp } from "vue";

import App from "./App.vue";
import router from "./router";
import store from "./store";
import http from "@/axios/http";

const app = createApp(App);

app.config.globalProperties.$http = http;
app.use(store);
app.use(router);

app.mount("#app");
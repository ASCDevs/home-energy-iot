import axios from "axios";
import store from "@/store/index";

const http = axios.create({
    baseURL: "https://home-energy-iot-api.azurewebsites.net"
});

http.interceptors.request.use(function(config) {
    const token = store.state.token;

    if(token) {
        config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
    
}, function(error) {
    return Promise.reject(error);
});

export default http;
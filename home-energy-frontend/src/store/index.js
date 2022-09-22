import { createStore } from "vuex";
import createPersistedState from 'vuex-persistedstate';
import http from '@/axios/http';

export default createStore({
    state: {
        token: null,
        user: {}
    },

    getters: {
        isAuthenticatedUser: state => Boolean(state.token)
    },

    mutations: {
        AUTHENTICATE_USER(state, {token, user}) {
            state.token = token;
            state.user = user;
        },

        LOGOUT_USER(state) {
            state.token = null;
            state.user = {};
        }
    },

    actions: {
        login({commit}, loginModel) {
            return new Promise((resolve, reject) => {
                http.post('/api/login/login', loginModel)
                    .then((response) => {
                        commit('AUTHENTICATE_USER', {
                            token: response.data.userToken,
                            user: response.data.user
                        });

                        resolve(response.data);

                    }).catch(error => {
                        reject(error);
                    })
            })
        },

        logout({commit}) {
            return new Promise(() => {
                commit('LOGOUT_USER');
                localStorage.removeItem('vuex');
            })
        } 
    },

    plugins: [createPersistedState()]
});
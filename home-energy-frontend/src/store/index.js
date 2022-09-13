import { createStore } from "vuex";
import createPersistedState from 'vuex-persistedstate';
import http from '@/axios/http';

export default createStore({
  state: {
    token: null,
    user: {}
  },

  getters: {
    usuarioLogado: state => Boolean(state.token)
  },

  mutations: {
    USUARIO_LOGADO(state, {token, user}) {
      state.token = token;
      state.user = user;
    },

    DESLOGAR_USUARIO(state) {
        state.token = null;
        state.user = {};
    }
  },

  actions: {
    logar({commit}, user) {
        return new Promise((resolve, reject) => {
            http.post('/auth', user)
                .then((response) => {
                    commit('USUARIO_LOGADO', {
                        token: response.data.token,
                        user: response.data.user
                    });

                    resolve(response.data);

                }).catch(error => {
                    reject(error);
                })
        })
    },

    deslogar({commit}) {
        return new Promise(() => {
          commit('DESLOGAR_USUARIO');
        })
    } 
  },

  plugins: [createPersistedState()]

  // modules: {},
});

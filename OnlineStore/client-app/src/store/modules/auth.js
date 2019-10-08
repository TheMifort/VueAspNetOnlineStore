const axios = require('axios');

const state = {
    role: localStorage.getItem("role") || "",
    status: "",
    isAuthenticated: localStorage.getItem("isAuthenticated") || false
};

const getters = {
    isAuthenticated: state => state.isAuthenticated,
    authStatus: state => state.status
};

const actions = {
    AUTH_REQUEST: ({ commit }, user) => {
        return new Promise((resolve, reject) => {
            commit("AUTH_REQUEST");

            axios
                .post('api/Account/Login', { "username": user.username, "password": user.password })
                .then(resp => {
                    localStorage.setItem("role", resp.role);
                    commit("AUTH_SUCCESS", resp);
                    //dispatch("USER_REQUEST");
                    resolve(resp);
                }).catch(err => {
                    commit("AUTH_ERROR", err);
                    localStorage.removeItem("isAuthenticated");
                    localStorage.removeItem("role");
                    reject(err);
                });
        });
    },
    AUTH_LOGOUT: ({ commit }) => {
        return new Promise((resolve) => {
            axios
                .get('api/Account/LogOff')
                .then(resp => {
                    commit("AUTH_LOGOUT");
                    localStorage.removeItem("isAuthenticated");
                    localStorage.removeItem("role");
                    resolve(resp);
                });
        });
    }
};

const mutations = {
    AUTH_REQUEST: (state) => {
        state.status = "loading";
    },
    AUTH_SUCCESS: (state, resp) => {
        state.status = "success";
        state.isAuthenticated = true;
        state.role = resp.role;
    },
    AUTH_ERROR: (state) => {
        state.status = "error";
    },
    AUTH_LOGOUT: (state) => {
        state.role = "";
        state.isAuthenticated = false;
    }
};

export default {
    state,
    getters,
    actions,
    mutations
}
const axios = require('axios');

const state = {
    token: localStorage.getItem("token") || "",
    refreshToken: localStorage.getItem("refreshToken") || "",
    username: localStorage.getItem("username") || "",
    role: localStorage.getItem("role") || "",
    status: ""
};

const getters = {
    isAuthenticated: state => !!state.token,
    authStatus: state => state.status
};

const actions = {
    AUTH_REQUEST: ({ commit }, user) => {
        return new Promise((resolve, reject) => {
            commit("AUTH_REQUEST");

            axios
                .post('api/Account/Auth/Login', { "username": user.username, "password": user.password })
                .then(resp => {
                    localStorage.setItem("role", resp.data.Role);
                    localStorage.setItem("token", resp.data.AccessToken);
                    localStorage.setItem("username", resp.data.Name);
                    localStorage.setItem("refreshToken", resp.data.RefreshToken);
                    axios.defaults.headers.common['Authorization'] = "Bearer " + resp.data.AccessToken;
                    commit("AUTH_SUCCESS", resp);
                    resolve(resp);
                }).catch(err => {
                    commit("AUTH_ERROR", err);
                    localStorage.removeItem("token");
                    localStorage.removeItem("role");
                    localStorage.removeItem("refreshToken");
                    localStorage.removeItem("username");
                    reject(err);
                });
        });
    },
    AUTH_REFRESH: ({ commit }) => {
        return new Promise((resolve, reject) => {
            commit("AUTH_REQUEST");

            axios
                .post('api/Account/Auth/Token', { "refreshToken": localStorage.getItem("refreshToken") })
                .then(resp => {
                    localStorage.setItem("role", resp.data.Role);
                    localStorage.setItem("token", resp.data.AccessToken);
                    localStorage.setItem("username", resp.data.Name);
                    localStorage.setItem("refreshToken", resp.data.RefreshToken);
                    axios.defaults.headers.common['Authorization'] = "Bearer " + resp.data.AccessToken;
                    commit("AUTH_SUCCESS", resp);
                    resolve(resp);
                }).catch(err => {
                    commit("AUTH_ERROR", err);
                    localStorage.removeItem("token");
                    localStorage.removeItem("role");
                    localStorage.removeItem("refreshToken");
                    localStorage.removeItem("username");
                    reject(err);
                });
        });
    },
    AUTH_LOGOUT: ({ commit }) => {
        return new Promise((resolve) => {
            axios
                .post('api/Account/SignOut')
                .then(resp => {
                    commit("AUTH_LOGOUT");
                    localStorage.removeItem("token");
                    localStorage.removeItem("username");
                    localStorage.removeItem("role");
                    localStorage.removeItem("refreshToken");
                    resolve(resp);
                })
                .catch(err => {
                    alert(err);
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
    },
    AUTH_ERROR: (state) => {
        state.status = "error";
    },
    AUTH_LOGOUT: (state) => {
        state.role = "";
    }
};

export default {
    state,
    getters,
    actions,
    mutations
}
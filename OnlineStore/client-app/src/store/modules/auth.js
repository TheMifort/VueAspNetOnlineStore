const axios = require('axios');

const state = {
    token: localStorage.getItem("token") || "",
    refreshToken: localStorage.getItem("refreshToken") || "",
    username: localStorage.getItem("username") || "",
    role: localStorage.getItem("role") || "",
    customerName: localStorage.getItem("customerName") || "",
    customerCode: localStorage.getItem("customerCode") || "",
    status: ""
};

const getters = {
    isAuthenticated: state => !!state.token,
    isManager: state => state.role === "Manager",
    hasCustomer: state => !!state.customer.Name,
    customer: state => state.customerName + '(' + state.customerCode + ')',
    authStatus: state => state.status
};

const actions = {
    AUTH_REQUEST: ({ commit }, user) => {
        return new Promise((resolve, reject) => {
            commit("AUTH_REQUEST");

            axios
                .post('api/Account/Auth/Login', { "username": user.username, "password": user.password })
                .then(resp => {

                    commit("AUTH_SUCCESS", resp);
                    resolve(resp);
                }).catch(err => {
                    commit("AUTH_ERROR", err);
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
                    commit("AUTH_SUCCESS", resp);
                    resolve(resp);
                }).catch(err => {
                    commit("AUTH_ERROR", err);
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
        localStorage.setItem("role", resp.data.Role);
        localStorage.setItem("token", resp.data.AccessToken);
        localStorage.setItem("username", resp.data.Name);
        localStorage.setItem("refreshToken", resp.data.RefreshToken);
        localStorage.setItem("customerName", resp.data.CustomerName);
        localStorage.setItem("customerCode", resp.data.CustomerCode);
        axios.defaults.headers.common['Authorization'] = "Bearer " + resp.data.AccessToken;
        state.status = "success";
    },
    AUTH_ERROR: (state) => {
        localStorage.removeItem("token");
        localStorage.removeItem("role");
        localStorage.removeItem("refreshToken");
        localStorage.removeItem("username");
        localStorage.removeItem("customerCode");
        localStorage.removeItem("customerName");
        state.status = "error";
    },
    AUTH_LOGOUT: (state) => {
        localStorage.removeItem("token");
        localStorage.removeItem("role");
        localStorage.removeItem("refreshToken");
        localStorage.removeItem("username");
        localStorage.removeItem("customerCode");
        localStorage.removeItem("customerName");
        state.role = "";
    }
};

export default {
    state,
    getters,
    actions,
    mutations
}
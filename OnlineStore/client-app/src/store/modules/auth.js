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
    accessToken: state => state.token,
    isManager: state => state.role === "Manager",
    isUser: state =>  state.role === "User",
    hasCustomer: state => !!state.customerName,
    customer: state => state.customerName + '(' + state.customerCode + ')',
    authStatus: state => state.status,
    userName: state => state.username,
};

const actions = {
    AUTH_REQUEST: ({commit}, user) => {
        return new Promise((resolve, reject) => {
            commit("AUTH_REQUEST");

            axios
                .post('api/Account/Auth/Login', {"username": user.username, "password": user.password})
                .then(resp => {

                    commit("AUTH_SUCCESS", resp);
                    resolve(resp);
                }).catch(err => {
                commit("AUTH_ERROR", err);
                reject(err);
            });
        });
    },
    AUTH_REFRESH: ({commit}) => {
        return new Promise((resolve, reject) => {
            commit("AUTH_REQUEST");

            axios
                .post('api/Account/Auth/Token', {"refreshToken": localStorage.getItem("refreshToken")})
                .then(resp => {
                    commit("AUTH_SUCCESS", resp);
                    resolve(resp);
                }).catch(err => {
                commit("AUTH_ERROR", err);
                commit("AUTH_LOGOUT", err);
                reject(err);
            });
        });
    },
    AUTH_LOGOUT: ({commit}) => {
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
                });
        });
    }
};

const mutations = {
    AUTH_REQUEST: (state) => {
        state.status = "loading";
    },
    AUTH_SUCCESS: (state, resp) => {
        state.role = resp.data.Role;
        state.token = resp.data.AccessToken;
        state.username = resp.data.Name;
        state.refreshToken = resp.data.RefreshToken;
        state.customerName = resp.data.CustomerName;
        state.customerCode = resp.data.CustomerCode;

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
        state.status = "error";
    },
    AUTH_LOGOUT: (state) => {
        state.role = "";
        state.token = "";
        state.userName = "";
        state.refreshToken ="";
        state.customerName = "";
        state.customerCode = "";

        localStorage.removeItem("token");
        localStorage.removeItem("role");
        localStorage.removeItem("refreshToken");
        localStorage.removeItem("username");
        localStorage.removeItem("customerCode");
        localStorage.removeItem("customerName");

        axios.defaults.headers.common['Authorization'] = "";
        state.role = "";
    }
};

export default {
    state,
    getters,
    actions,
    mutations
}
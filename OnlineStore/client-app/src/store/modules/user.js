const axios = require('axios');

const state = {
    users: [],
    allRoles: [],
    allCustomers: [],
    status: ""
};

const getters = {
    users: state => state.users,
    allRoles: state => state.allRoles,
    allCustomers: state => state.allCustomers
};

const actions = {
    USERS_REQUEST: ({ commit }) => {
        return new Promise((resolve, reject) => {
            commit("USERS_REQUEST");

            axios
                .get('api/Admin/User')
                .then(resp => {
                    commit("USER_SUCCESS", resp);
                    resolve(resp);
                }).catch(err => {
                    commit("USER_ERROR", err);
                    reject(err);
                });
        });
    },
    USER_CHANGE: ({ commit }, user) => {
        return new Promise((resolve, reject) => {
            commit("USERS_REQUEST");
            axios
                .put(`api/Admin/User/${user.id}`, { roles: user.roles, customerId: user.customer == null ? null : user.customer.id })
                .then(resp => {
                    //commit("USER_SUCCESS", resp);
                    resolve(resp);
                }).catch(err => {
                    commit("USER_ERROR", err);
                    reject(err);
                });
        });
    },
    USER_CREATE: ({ commit }, user) => {
        return new Promise((resolve, reject) => {
            commit("USERS_REQUEST");
            axios
                .post("api/Admin/User/", { userName: user.userName, password: user.password })
                .then(resp => {
                    //commit("USER_SUCCESS", resp);
                    resolve(resp);
                }).catch(err => {
                    commit("USER_ERROR", err);
                    reject(err);
                });
        });
    }
};

const mutations = {
    USERS_REQUEST: (state) => {
        state.status = "loading";
    },
    USER_SUCCESS: (state, resp) => {
        state.status = "success";
        state.users = resp.data.users;
        state.allRoles = resp.data.allRoles;
        state.allCustomers = resp.data.allCustomers;
    },
    USER_ERROR: (state) => {
        state.status = "error";
    }
};

export default {
    state,
    getters,
    actions,
    mutations
}
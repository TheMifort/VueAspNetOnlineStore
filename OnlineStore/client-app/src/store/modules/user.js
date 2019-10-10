const axios = require('axios');

const state = {
    users: [],
    status: ""
};

const getters = {
    users: state => state.users
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
    }
};

const mutations = {
    USERS_REQUEST: (state) => {
        state.status = "loading";
    },
    USER_SUCCESS: (state, resp) => {
        state.status = "success";
        state.users = resp.data;
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
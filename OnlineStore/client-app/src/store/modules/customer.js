const axios = require('axios');

const state = {
    customers: [],
    status: ""
};

const getters = {
    customers: state => state.customers
};

const actions = {
    CUSTOMERS_REQUEST: ({ commit }) => {
        return new Promise((resolve, reject) => {
            commit("CUSTOMERS_REQUEST");

            axios
                .get('api/Admin/Customer')
                .then(resp => {
                    commit("CUSTOMER_SUCCESS", resp);
                    resolve(resp);
                }).catch(err => {
                    commit("CUSTOMER_ERROR", err);
                    reject(err);
                });
        });
    },
    CUSTOMER_CHANGE: ({ commit }, customer) => {
        return new Promise((resolve, reject) => {
            commit("CUSTOMERS_REQUEST");
            axios
                .put(`api/Admin/Customer/${customer.id}`, { name: customer.name, code: customer.code, discount: customer.discount > 0 ? customer.discount : 0 })
                .then(resp => {
                    resolve(resp);
                }).catch(err => {
                    commit("CUSTOMER_ERROR", err);
                    reject(err);
                });
        });
    },
    CUSTOMER_CREATE: ({ commit }, customer) => {
        return new Promise((resolve, reject) => {
            commit("CUSTOMERS_REQUEST");
            axios
                .post("api/Admin/Customer/", { name: customer.name, code: customer.code, discount: 0 })
                .then(resp => {
                    resolve(resp);
                }).catch(err => {
                    commit("CUSTOMER_ERROR", err);
                    reject(err);
                });
        });
    }
};

const mutations = {
    CUSTOMERS_REQUEST: (state) => {
        state.status = "loading";
    },
    CUSTOMER_SUCCESS: (state, resp) => {
        state.status = "success";
        state.customers = resp.data;
    },
    CUSTOMER_ERROR: (state) => {
        state.status = "error";
    }
};

export default {
    state,
    getters,
    actions,
    mutations
}
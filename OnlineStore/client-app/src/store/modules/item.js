const axios = require('axios');

const state = {
    items: []
};

const getters = {
    items: state => state.items
};

const actions = {
    ITEMS_REQUEST: ({ commit }) => {
        return new Promise((resolve, reject) => {
            commit("ITEMS_REQUEST");

            axios
                .get('api/Items')
                .then(resp => {
                    commit("ITEM_SUCCESS", resp);
                    resolve(resp);
                }).catch(err => {
                    commit("ITEM_ERROR", err);
                    reject(err);
                });
        });
    },
    ITEM_CHANGE: ({ commit }, item) => {
        return new Promise((resolve, reject) => {
            commit("ITEMS_REQUEST");
            axios
                .put(`api/Items/${item.id}`, { name: item.name, code: item.code, price: item.price, category: item.category })
                .then(resp => {
                    //commit("ITEM_SUCCESS", resp);
                    resolve(resp);
                }).catch(err => {
                    commit("ITEM_ERROR", err);
                    reject(err);
                });
        });
    },
    ITEM_CREATE: ({ commit }, item) => {
        return new Promise((resolve, reject) => {
            commit("ITEMS_REQUEST");
            axios
                .post("api/Items/", { name: item.name, code: item.code, price: item.price, category: item.category })//TODO send just item?
                .then(resp => {
                    //commit("ITEM_SUCCESS", resp);
                    resolve(resp);
                }).catch(err => {
                    commit("ITEM_ERROR", err);
                    reject(err);
                });
        });
    },
    ITEM_DELETE: ({ commit }, itemId) => {
        return new Promise((resolve, reject) => {
            commit("ITEMS_REQUEST");
            axios
                .delete(`api/Items/${itemId}`)
                .then(resp => {
                    resolve(resp);
                }).catch(err => {
                    commit("ITEM_ERROR", err);
                    reject(err);
                });
        });
    }
};

const mutations = {
    ITEMS_REQUEST: (state) => {
        state.status = "loading";
    },
    ITEM_SUCCESS: (state, resp) => {
        state.status = "success";
        state.items = resp.data;
    },
    ITEM_ERROR: (state) => {
        state.status = "error";
    }
};

export default {
    state,
    getters,
    actions,
    mutations
}
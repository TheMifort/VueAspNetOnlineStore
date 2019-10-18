const axios = require('axios');
import Vue from 'vue'

const state = {
    items: [],
    cartItems: JSON.parse(localStorage.getItem('cartItems')) || [],
    orders: [],
};

const getters = {
    items: state => state.items,
    cartItems: state => state.cartItems,
    orders: state => state.orders
};

const actions = {
    ITEMS_REQUEST: ({commit}) => {
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
    ITEM_CHANGE: ({commit}, item) => {
        return new Promise((resolve, reject) => {
            commit("ITEMS_REQUEST");
            axios
                .put(`api/Items/${item.id}`, {
                    name: item.name,
                    code: item.code,
                    price: item.price,
                    category: item.category
                })
                .then(resp => {
                    //commit("ITEM_SUCCESS", resp);
                    resolve(resp);
                }).catch(err => {
                commit("ITEM_ERROR", err);
                reject(err);
            });
        });
    },
    ITEM_CREATE: ({commit}, item) => {
        return new Promise((resolve, reject) => {
            commit("ITEMS_REQUEST");
            axios
                .post("api/Items/", {name: item.name, code: item.code, price: item.price, category: item.category})//TODO send just item?
                .then(resp => {
                    //commit("ITEM_SUCCESS", resp);
                    resolve(resp);
                }).catch(err => {
                commit("ITEM_ERROR", err);
                reject(err);
            });
        });
    },
    ITEM_DELETE: ({commit}, itemId) => {
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
    },
    CART_ADD: ({commit}, item) => {
        return new Promise((resolve) => {
            let same = state.cartItems.filter(e => e.id === item.id);
            if (same == null || same.length === 0) {
                item.count = 1;
                commit("CART_ADD", item);
                commit("CART_SAVE", item);
            } else {
                commit("CART_INC", same[0]);
                commit("CART_SAVE", item);
            }
            resolve(item);
        });
    },
    CART_DELETE: ({commit}, item) => {
        return new Promise((resolve) => {
            commit("CART_DELETE", item);
            commit("CART_SAVE", item);
            resolve(item);
        });
    },
    CART_EDIT: ({commit}, item) => {
        return new Promise((resolve) => {
            commit("CART_EDIT", item);
            commit("CART_SAVE", item);
            resolve(item);
        });
    },
    ORDERS_REQUEST: ({commit}) => {
        return new Promise((resolve, reject) => {
            axios
                .get('api/Items/Order')
                .then(resp => {
                    commit('ORDER_SUCCESS', resp);
                    resolve(resp);
                }).catch(err => {
                reject(err);
            });
        });
    },
    ORDER: ({commit, getters}) => {
        return new Promise((resolve, reject) => {

            let order = {
                orderItems: []
            };
            for (let item of getters.cartItems) {
                order.orderItems.push({itemId: item.id, itemsCount: item.count})
            }
            axios
                .post('api/Items/Order', order)
                .then(resp => {
                    resolve(resp);
                }).catch(err => {
                reject(err);
            });
        });
    },
    ORDER_CONFIRM: ({commit}, order) => {
        return new Promise((resolve, reject) => {
            axios
                .put(`api/Items/Order/${order.id}`, {shipmentDate: order.shipmentDate})
                .then(resp => {
                    resolve(resp);
                }).catch(err => {
                reject(err);
            });
        });
    },
    ORDER_COMPLETE: ({commit}, order) => {
        return new Promise((resolve, reject) => {
            axios
                .put(`api/Items/Order/${order.id}`, {complete: true})
                .then(resp => {
                    resolve(resp);
                }).catch(err => {
                reject(err);
            });
        });
    },
    ORDER_DELETE: ({commit}, order) => {
        return new Promise((resolve, reject) => {
            axios
                .delete(`api/Items/Order/${order.id}`)
                .then(resp => {
                    resolve(resp);
                }).catch(err => {
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
        for (let item of resp.data) {
            if (state.cartItems.some(e => e.id === item.id))
                Vue.set(item, '_rowVariant', 'info');
        }
    },
    ITEM_ERROR: (state) => {
        state.status = "error";
    },
    CART_ADD: (state, item) => {
        state.cartItems.push(item);
        Vue.set(item, '_rowVariant', 'info');
    },
    CART_DELETE: (state, item) => {
        Vue.delete(item, '_rowVariant', 'info');
        state.cartItems.splice(state.cartItems.indexOf(item), 1);
    },
    CART_EDIT: (state, item) => {

    },
    CART_INC: (state, item) => {
        item.count++;
    },
    CART_DEC: (state, item) => {
        if (item.count <= 1) {
            Vue.delete(item, '_rowVariant', 'info');
            state.cartItems.splice(state.cartItems.indexOf(item), 1);
        }
        item.count--;
    },
    CART_SAVE: (state) => {
        localStorage.setItem('cartItems', JSON.stringify(state.cartItems));
    },
    CART_CLEAR: (state) => {
        localStorage.removeItem('cartItems');
        state.cartItems = [];
    },
    ORDER_SUCCESS: (state, resp) => {
        state.orders = resp.data;
        for (let order of state.orders) {
            if (order.status === 1)
                Vue.set(order, '_rowVariant', 'info');
            else if (order.status === 2)
                Vue.set(order, '_rowVariant', 'success');
        }

    }
};

export default {
    state,
    getters,
    actions,
    mutations
}
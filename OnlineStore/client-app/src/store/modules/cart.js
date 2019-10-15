const state = {
    cartItems: []
};

const getters = {
    cartItems: state => state.cartItems
};

const actions = {
    CART_ADD: ({ commit }, item) => {
        return new Promise((resolve) => {
            let same = state.cartItems.filter(e => e.id === item.id);
            if (same == null || same.length === 0) {
                item.count = 1;
                commit("CART_ADD", item);
            } else {
                commit("CART_INC", same[0]);
            }
            resolve(item);
        });
    },
    CART_DELETE: ({ commit }, item) => {
        return new Promise((resolve) => {
            commit("CART_DELETE", item);
            resolve(item);
        });
    },
    CART_EDIT: ({ commit }, item) => {
        return new Promise((resolve) => {
            commit("CART_EDIT", item);
            resolve(item);
        });
    }
};

const mutations = {
    CART_ADD: (state, item) => {
        state.cartItems.push(item);
    },
    CART_DELETE: (state, item) => {
        state.cartItems = state.cartItems.filter(e => e.id !== item.id);
    },
    CART_EDIT: (state, item) => {
        
    },
    CART_INC: (state, item) => {
        item.count++;
    },
    CART_DEC: (state, item) => {
        item.count--;
    }
};

export default {
    state,
    getters,
    actions,
    mutations
}
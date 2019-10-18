import Vue from 'vue'
import Router from 'vue-router'
import store from '../store/store.js'

import Login from '../components/Login.vue'
import HelloWorld from '../components/HelloWorld.vue'
import Users from '../components/Users.vue'
import Customers from '../components/Customers.vue'
import Items from '../components/Items.vue'
import Cart from '../components/Cart.vue'
import Orders from '../components/Orders.vue'

Vue.use(Router);

const ifNotAuthenticated = (to, from, next) => {
    if (!store.getters.isAuthenticated) {
        next();
        return;
    }
    next('/');
};

const ifAuthenticated = (to, from, next) => {
    alert(store.getters.isAuthenticated);
    if (store.getters.isAuthenticated) {
        next();
        return;
    }
    next('/login');
};

const ifUser = (to, from, next) => {
    if (store.getters.isAuthenticated && store.getters.isUser) {
        next();
        return;
    }
    next('/');
};

const ifManager = (to, from, next) => {
    if (store.getters.isAuthenticated && store.getters.isManager) {
        next();
        return;
    }
    next('/');
};

const hasCustomer = (to, from, next) => {
    if (store.getters.isAuthenticated && store.getters.isUser && store.getters.hasCustomer) {
        next();
        return;
    }
    next('/');
};

export default new Router({
    mode: 'history',
    routes: [
        {
            path: '/',
            name: 'HelloWorld',
            component: HelloWorld
        },
        {
            path: '/login',
            name: 'Login',
            component: Login,
            beforeEnter: ifNotAuthenticated,
        },
        {
            path: '/users',
            name: 'Users',
            component: Users,
            beforeEnter: ifAuthenticated,
        },
        {
            path: '/customers',
            name: 'Customers',
            component: Customers,
            beforeEnter: ifManager,
        },
        {
            path: '/items',
            name: 'Items',
            component: Items,
            beforeEnter: hasCustomer,
        },
        {
            path: '/cart',
            name: 'Cart',
            component: Cart,
            beforeEnter: ifUser,
        },
        {
            path: '/orders',
            name: 'Orders',
            component: Orders,
            beforeEnter: ifAuthenticated,
        },
    ],
})
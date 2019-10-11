﻿import Vue from 'vue'
import Router from 'vue-router'
import store from '../store/store.js'

import Login from '../components/Login.vue'
import HelloWorld from '../components/HelloWorld.vue'
import Users from '../components/Users.vue'

Vue.use(Router)

const ifNotAuthenticated = (to, from, next) => {
    if (!store.getters.isAuthenticated) {
        next();
        return;
    }
    next('/');
}

const ifAuthenticated = (to, from, next) => {
    if (store.getters.isAuthenticated) {
        next();
        return;
    }
    next('/login');
}

export default new Router({
    mode: 'history',
    routes: [
        {
            path: '/',
            name: 'HelloWorld',
            component: HelloWorld
        },
        //{
        //    path: '/account',
        //    name: 'Account',
        //    component: Account,
        //    beforeEnter: ifAuthenticated,
        //},
        {
            path: '/login',
            name: 'Login',
            component: Login,
            //beforeEnter: ifNotAuthenticated,
        },
        {
            path: '/users',
            name: 'Users',
            component: Users,
            //beforeEnter: ifNotAuthenticated,
        },
    ],
})
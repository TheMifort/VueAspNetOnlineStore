import Vue from 'vue'
import App from './App.vue'
import store from './store/store.js'
import axios from 'axios'
import router from './router/router.js'

import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'

import '@fortawesome/fontawesome-free/css/all.css'
import '@fortawesome/fontawesome-free/js/all.js'

import BootstrapVue from 'bootstrap-vue'

Vue.use(BootstrapVue);
Vue.config.productionTip = false;

const token = localStorage.getItem('token');
if (token) {
    axios.defaults.headers.common['Authorization'] = "Bearer " + token;
}

axios.interceptors.response.use((response) => {
    return response;
}, function (error) {
    const originalRequest = error.config;

    if (error.response.status === 401 && !originalRequest._retry) {
        originalRequest._retry = true;
        let ret = store.dispatch("AUTH_REFRESH").then(() => {
            return axios(originalRequest);
        });
        return ret;
    }
    return Promise.reject(error);
});

new Vue({
    store,
    router,
    render: h => h(App),
}).$mount('#app');

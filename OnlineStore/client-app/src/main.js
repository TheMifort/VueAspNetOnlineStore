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
document.title = "OnlineStore";
Vue.config.devtools = process.env.NODE_ENV === 'development';

const token = localStorage.getItem('token');
if (token) {
    axios.defaults.headers.common['Authorization'] = "Bearer " + token;
}
axios.interceptors.response.use((response) => {
    return response;
}, function (error) {
    const originalRequest = error.config;
    if (error.response.status === 401) {
        if(typeof originalRequest.headers["retry"] == 'undefined') {
            originalRequest.headers['retry'] = true;
            let ret = store.dispatch("AUTH_REFRESH").then(() => {
                originalRequest.headers['Authorization'] = "Bearer " + store.getters.accessToken;
                return axios(originalRequest);
            });
            return ret;
        }
        else{
            store.commit("AUTH_LOGOUT");
            router.push('/login');
        }
    }
    return Promise.reject(error);
});

let app = new Vue({
    store,
    router,
    render: h => h(App),
}).$mount('#app');

window.__VUE_DEVTOOLS_GLOBAL_HOOK__.Vue = app.constructor;
import Vue from 'vue'
import App from './App.vue'
import store from './store/store.js'
import axios from 'axios'
import router from './router/router.js'

import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'

import BootstrapVue from 'bootstrap-vue'

Vue.use(BootstrapVue);

Vue.config.productionTip = false;

const token = localStorage.getItem('token');
if (token) {
    axios.defaults.headers.common['Authorization'] = "Bearer " + token;
}
new Vue({
    store,
    router,
    render: h => h(App),
}).$mount('#app');

<template>
    <div id="app">
        <Navbar/>
        <div class="main-container">
            <router-view />
        </div>
    </div>
</template>

<script>
    import HelloWorld from './components/HelloWorld.vue'
    import Login from "./components/Login.vue"
    import Navbar from "./components/Navbar.vue"
    import Users from "./components/Users.vue"
    export default {
        name: 'app',
        components: {
            Navbar,
            HelloWorld,
            Login,
            Users
        }
    }
</script>

<style>

    #app {
        font-family: 'Avenir', Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
    }
</style>

<template>
    <div>
        <b-navbar toggleable="lg" type="dark" variant="info">
            <b-navbar-brand>OnlineStore</b-navbar-brand>

            <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

            <b-collapse id="nav-collapse" is-nav>
                <b-navbar-nav>
                    <b-nav-item to="/items">Items</b-nav-item>
                </b-navbar-nav>
                <!-- Right aligned nav items -->
                <b-navbar-nav class="ml-auto">
                    <b-navbar-nav>
                        <b-nav-item v-if="$store.getters.isUser" to="/cart">Cart</b-nav-item>
                        <b-nav-item to="/orders">Orders</b-nav-item>
                    </b-navbar-nav>

                    <b-nav-item-dropdown text="AdminPanel" right v-if="$store.getters.isManager">
                        <b-dropdown-item @click="$router.push('/customers')">Customers</b-dropdown-item>
                        <b-dropdown-item @click="$router.push('/users')">Users</b-dropdown-item>
                    </b-nav-item-dropdown>

                    <b-nav-item-dropdown right v-if="$store.getters.isAuthenticated">
                        <!-- Using 'button-content' slot -->
                        <template v-slot:button-content>
                            <em>{{userName}}</em>
                        </template>
                        <b-dropdown-item @click="signOut">Sign Out</b-dropdown-item>
                    </b-nav-item-dropdown>
                    <b-navbar-nav v-else>
                        <b-nav-item to="/login">Login</b-nav-item>
                    </b-navbar-nav>
                </b-navbar-nav>
            </b-collapse>
        </b-navbar>
    </div>
</template>

<script>
    export default {
        name: 'Login',
        computed: {
            userName: {
                get() {
                    return this.$store.getters.userName;
                }
            }
        },
        methods: {
            signOut() {
                this.$store.dispatch('AUTH_LOGOUT');
                this.$router.push("/");
            }
        }
    }
</script>

<style scoped>
</style>
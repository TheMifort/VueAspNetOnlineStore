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
                        <b-nav-item to="/cart">Cart</b-nav-item>
                        <b-nav-item to="/orders">Orders</b-nav-item>
                    </b-navbar-nav>

                    <b-nav-item-dropdown text="AdminPanel" right>
                        <b-dropdown-item @click="$router.push('/customers')">Customers</b-dropdown-item>
                        <b-dropdown-item @click="$router.push('/users')">Users</b-dropdown-item>
                    </b-nav-item-dropdown>

                    <b-nav-item-dropdown right>
                        <!-- Using 'button-content' slot -->
                        <template v-slot:button-content>
                            <em>{{userName}}</em>
                        </template>
                        <b-dropdown-item @click="$router.push('/login')">Profile</b-dropdown-item>
                        <b-dropdown-item @click="signOut">Sign Out</b-dropdown-item>
                    </b-nav-item-dropdown>
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
            }
        }
    }
</script>

<style scoped>
</style>
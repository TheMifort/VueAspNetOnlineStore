<template>
    <div style="margin-top: 5px;">
        <b-alert v-if="error !== ''" show variant="danger">{{error}}</b-alert>
        <b-container>
            <b-row class="my-1">
                <b-col sm="3">
                    <label>Login: </label>
                </b-col>
                <b-col sm="8">
                    <b-form-input id="input-login" v-model="username" type="text"></b-form-input>
                </b-col>
            </b-row>
            <b-row class="my-1">
                <b-col sm="3">
                    <label>Password: </label>
                </b-col>
                <b-col sm="8">
                    <b-form-input id="input-password" v-model="password" type="password"></b-form-input>
                </b-col>
            </b-row>
            <b-button @click="login">Login</b-button>
        </b-container>

    </div>
</template>

<script>
    export default {
        name: 'Login',
        props: {
            msg: String
        },
        data() {
            return {
                username: "Admin",
                password: "+",
                error: ""
            };
        },
        methods: {
            login() {
                const {username, password} = this;
                this.$store.dispatch("AUTH_REQUEST", {username, password}).then(() => {
                    this.$router.push("/");
                }).catch(err => {
                   this.error = Object.values(err.response.data)[0][0];
                });
            }
        },
        created() {
            if (this.$store.getters.isAuthenticated) {
                this.$router.push("/");
            }
        }
    }</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>
<template>
    <div>
        <b-table selectable
                 select-mode="single"
                 selected-variant="primary"
                 striped hover
                 :items="users"
                 :fields="fields"
                 :busy="isBusy"
                 @row-selected="onRowSelected"
                 @row-dblclicked="onRowDoubleClicked">
            <template v-slot:table-busy>
                <div class="text-center text-danger my-2">
                    <b-spinner class="align-middle"></b-spinner>
                    <strong>Loading...</strong>
                </div>
            </template>
        </b-table>

        <b-button v-b-modal.modal-user-add>Add user</b-button>
        <b-modal ref="modal-user-add" id="modal-user-add" title="Create new user" @ok="okAdd" @cancel="cancelAdd">
            <b-form-group id="fieldset-1"
                          label="UserName"
                          label-for="input-userName">
                <b-form-input id="input-userName" v-model="newUser.userName" trim/>
            </b-form-group>

            <b-form-group id="fieldset-1"
                          label="Password"
                          label-for="input-new-password">
                <b-form-input id="input-new-password" type="password" v-model="newUser.password" trim/>
            </b-form-group>

        </b-modal>

        <b-modal ref="modal-user" id="modal-user" :title="user.userName" @ok="ok" @cancel="cancel">
            <b-form-group label="Roles:">
                <b-form-checkbox-group id="checkbox-group-2" v-model="user.roles" name="flavour-2">
                    <b-form-checkbox v-for="role in roles"
                                     v-model="user.roles"
                                     :key="role"
                                     :value="role"
                                     name="flavour-3a">
                        {{ role }}
                    </b-form-checkbox>
                </b-form-checkbox-group>
            </b-form-group>

            <b-form-group label="Customer:">
                <b-form-select v-model="user.customer" class="mb-3">
                    <template v-slot:first>
                        <option :value="null"></option>
                    </template>
                    <option v-for="customer in customers"
                            :key="customer.id"
                            :value="customer"
                            name="flavour-3a">
                        {{ customer.name }}
                    </option>
                </b-form-select>
            </b-form-group>
            <b-form-group id="fieldset-1"
                          label="Password"
                          label-for="input-password">
                <b-form-input id="input-password" type="password" v-model="user.password" trim/>
            </b-form-group>
        </b-modal>
    </div>
</template>

<script>


    export default {
        name: 'Users',
        data() {
            return {
                isBusy: false,
                fields: ['id', 'userName', {key: 'customer.name', label: "Customer"}, 'roles'],
                newUser: {
                    id: {},
                    userName: "",
                    roles: [],
                    customer: {}
                },
                user: {
                    id: {},
                    userName: "",
                    roles: [],
                    customer: {}
                }
            }
        },
        computed: {
            users: {
                get() {
                    return this.$store.getters.users;
                }
            },
            roles: {
                get() {
                    return this.$store.getters.allRoles;
                }
            },
            customers: {
                get() {
                    return this.$store.getters.allCustomers;
                }
            },
        },

        methods: {
            onRowSelected(items) {

            },
            onRowDoubleClicked(item) {
                this.user.id = item.id;
                this.user.userName = item.userName;
                this.user.roles = item.roles;
                this.user.customer = item.customer;
                this.$refs['modal-user'].show();
            },
            async fetchData() {
                this.isBusy = true;
                await this.$store.dispatch('USERS_REQUEST');
                this.isBusy = false;
            },
            async ok() {
                await this.$store.dispatch('USER_CHANGE', this.user);
                await this.fetchData();
                this.user = {
                    id: {},
                    userName: "",
                    roles: {},
                    customer: {}
                }
            },
            async okAdd() {
                await this.$store.dispatch('USER_CREATE', this.newUser);
                await this.fetchData();
                this.newUser = {
                    id: {},
                    userName: "",
                    roles: {},
                    customer: {}
                }
            },
            cancel() {
                this.user = {
                    id: {},
                    userName: "",
                    roles: {},
                    customer: {}
                }
            },
            cancelAdd() {
                this.newUser = {
                    id: {},
                    userName: "",
                    roles: {},
                    customer: {}
                }
            }
        },


        created: async function () {
            if (!this.$store.getters.isManager) {
                this.$router.push("/");
                return;
            }
            await this.fetchData();
        },
    }
</script>

<style scoped>
</style>
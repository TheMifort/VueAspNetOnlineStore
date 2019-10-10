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
        <b-button v-b-modal.modal-1>Launch demo modal</b-button>

        <b-modal id="modal-1" :title="user.Id">
            <b-form-input v-model="user.userName" type="text" placeholder="Enter your name"/>
            <b-form-input v-model="user.userName" type="text" placeholder="Enter your name"/>
            <p class="my-4">Hello from modal!</p>
        </b-modal>
    </div>
</template>

<script>


    export default {
        name: 'Users',
        data() {
            return {
                isBusy: false,
                fields: ['id', 'userName', 'customer', 'roles'],
                users: [],
                user: {}
            }
        },


        methods: {
            onRowSelected(items) {

            },
            onRowDoubleClicked(item) {
                this.user = item;
                alert(JSON.stringify(this.user));
            },
            async fetchData() {
                this.isBusy = true;
                await this.$store.dispatch('USERS_REQUEST');
                this.users = this.$store.getters.users;
                this.isBusy = false;
            }

        },
        created: async function () {
            await this.fetchData();
        },
    }
</script>

<style scoped>
</style>
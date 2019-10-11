<template>
    <div>
        <b-table selectable
                 select-mode="single"
                 selected-variant="primary"
                 striped hover
                 :items="customers"
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

        <b-button v-b-modal.modal-customer-add>Add customer</b-button>
        <b-modal ref="modal-customer-add" id="modal-customer-add" title="Create new customer" @ok="okAdd" @cancel="cancelAdd">
            <b-form-group id="fieldset-1"
                          label="Name"
                          label-for="input-name">
                <b-form-input id="input-name" v-model="newCustomer.name" trim />
            </b-form-group>

            <b-form-group id="fieldset-1"
                          label="Code"
                          label-for="input-new-code">
                <b-form-input id="input-new-code" v-model="newCustomer.code" trim />
            </b-form-group>

        </b-modal>

        <b-modal ref="modal-customer" id="modal-customer" :title="customer.name" @ok="ok" @cancel="cancel">
            <b-form-group id="fieldset-1"
                          label="Code"
                          label-for="input-code">
                <b-form-input id="input-code" v-model="customer.code" trim />
            </b-form-group>
            <b-form-group id="fieldset-1"
                          label="Discount"
                          label-for="input-discount">
                <b-form-input id="input-discount" type="number" v-model.number="customer.discount" trim />
            </b-form-group>
        </b-modal>
    </div>
</template>

<script>


    export default {
        name: 'Customers',
        data() {
            return {
                isBusy: false,
                fields: ['id', 'name', 'code', 'discount'],
                newCustomer: {
                    id: {},
                    name: "",
                    code: "",
                    discount: ""
                },
                customer: {
                    id: {},
                    name: "",
                    code: "",
                    discount: ""
                }
            }
        },
        computed: {
            customers: {
                get() {
                    return this.$store.getters.customers;
                }
            }
        },

        methods: {
            onRowSelected(items) {

            },
            onRowDoubleClicked(item) {
                this.customer.id = item.id;
                this.customer.name = item.name;
                this.customer.code = item.code;
                this.customer.discount = item.discount;
                this.$refs['modal-customer'].show();
            },
            async fetchData() {
                this.isBusy = true;
                await this.$store.dispatch('CUSTOMERS_REQUEST');
                this.isBusy = false;
            },
            async ok() {
                await this.$store.dispatch('CUSTOMER_CHANGE', this.customer);
                await this.fetchData();
                this.customer = {
                    id: {},
                    name: "",
                    code: "",
                    discount: ""
                }
            },
            async okAdd() {
                await this.$store.dispatch('CUSTOMER_CREATE', this.newCustomer);
                await this.fetchData();
                this.newCustomer = {
                    id: {},
                    name: "",
                    code: "",
                    discount: ""
                }
            },
            cancel() {
                this.customer = {
                    id: {},
                    name: "",
                    code: "",
                    discount: ""
                }
            },
            cancelAdd() {
                this.newCustomer = {
                    id: {},
                    name: "",
                    code: "",
                    discount: ""
                }
            }
        },


        created: async function () {
            await this.fetchData();
        },
    }
</script>

<style scoped>
</style>
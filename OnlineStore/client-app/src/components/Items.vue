<template>
    <div>
        <b-table selectable
                 select-mode="single"
                 selected-variant="primary"
                 striped hover
                 :items="items"
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

        <b-button v-b-modal.modal-item-add>Add item</b-button>
        <b-modal ref="modal-item-add" id="modal-item-add" title="Create new item" @ok="okAdd" @cancel="cancelAdd">
            <b-form-group id="fieldset-1"
                          label="Name"
                          label-for="input-new-itemName">
                <b-form-input id="input-new-itemName" v-model="newItem.name" trim />
            </b-form-group>

            <b-form-group id="fieldset-1"
                          label="Code"
                          label-for="input-new-code">
                <b-form-input id="input-new-code" v-model="newItem.code" trim />
            </b-form-group>

            <b-form-group id="fieldset-1"
                          label="Price"
                          label-for="input-new-price">
                <b-form-input id="input-new-price" type="number" v-model.number="newItem.price" trim />
            </b-form-group>

            <b-form-group id="fieldset-1"
                          label="Category"
                          label-for="input-new-category">
                <b-form-input id="input-new-category" v-model="newItem.category" trim />
            </b-form-group>

        </b-modal>

        <b-modal ref="modal-item" id="modal-user" title="item.id" @ok="ok" @cancel="cancel">
            <b-form-group id="fieldset-1"
                          label="Name"
                          label-for="input-itemName">
                <b-form-input id="input-itemName" v-model="item.name" trim />
            </b-form-group>

            <b-form-group id="fieldset-1"
                          label="Code"
                          label-for="input-code">
                <b-form-input id="input-code" v-model="item.code" trim />
            </b-form-group>

            <b-form-group id="fieldset-1"
                          label="Price"
                          label-for="input-price">
                <b-form-input id="input-price" type="number" v-model.number="item.price" trim />
            </b-form-group>

            <b-form-group id="fieldset-1"
                          label="Catefory"
                          label-for="input-category">
                <b-form-input id="input-category" v-model="item.category" trim />
            </b-form-group>
        </b-modal>
    </div>
</template>

<script>


    export default {
        name: 'Items',
        data() {
            return {
                isBusy: false,
                fields: ['id', 'name', 'code', 'price','category'],
                newItem: {
                    name: "",
                    code: "",
                    price: "",
                    category: ""
                },
                item: {
                    id: {},
                    name: "",
                    code: "",
                    price: "",
                    category: ""
                }
            }
        },
        computed: {
            items: {
                get() {
                    return this.$store.getters.items;
                }
            }
        },

        methods: {
            onRowSelected(items) {

            },
            onRowDoubleClicked(item) {
                this.item.id = item.id;
                this.item.name = item.name;
                this.item.code = item.code;
                this.item.price = item.price;
                this.item.category = item.category;
                this.$refs['modal-item'].show();
            },
            async fetchData() {
                this.isBusy = true;
                await this.$store.dispatch('ITEMS_REQUEST');
                this.isBusy = false;
            },
            async ok() {
                await this.$store.dispatch('ITEM_CHANGE', this.item);
                await this.fetchData();
            },
            async okAdd() {
                await this.$store.dispatch('ITEM_CREATE', this.newItem);
                await this.fetchData();
                this.newItem = {
                    name: "",
                    code: "",
                    price: "",
                    category: ""
                }
            },
            cancel() {

            },
            cancelAdd() {
                this.newItem = {
                    name: "",
                    code: "",
                    price: "",
                    category: ""
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
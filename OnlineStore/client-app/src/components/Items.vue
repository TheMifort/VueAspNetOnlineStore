﻿<template>
    <div class="container-fluid row">
        <div class="col-3" style="background: #00ffff">
            <h2>Filters</h2>
            <b-form-group id="fieldset-1"
                          label="Name"
                          label-for="input-filter-name">
                <b-form-input id="input-filter-name" v-model="filter.name" trim/>
            </b-form-group>
            <b-form-group id="fieldset-1"
                          label="Code"
                          label-for="input-filter-code">
                <b-form-input id="input-filter-code" v-model="filter.code" trim/>
            </b-form-group>
            <b-form-group id="fieldset-1"
                          label="Category"
                          label-for="input-filter-category">
                <b-form-input id="input-filter-category" v-model="filter.category" trim/>
            </b-form-group>
            <b-form-group id="fieldset-1"
                          label="Price from"
                          label-for="input-filter-priceFrom">
                <b-form-input id="input-filter-priceFrom" type="number" v-model.number="filter.priceFrom" trim/>
            </b-form-group>
            <b-form-group id="fieldset-1"
                          label="Price to"
                          label-for="input-filter-priceTo">
                <b-form-input id="input-filter-priceTo" type="number" v-model.number="filter.priceTo" trim/>
            </b-form-group>
        </div>

        <div class="col-9">
            <b-table striped hover
                     :items="items"
                     :fields="fields"
                     :busy="isBusy"
                     @row-dblclicked="onRowDoubleClicked">

                <template v-slot:cell(cart)="row">
                    <b-button size="sm" @click="cart(row)" class="mr-2">
                        <i class="fas fa-sm fa-cart-plus"></i>
                    </b-button>
                </template>
                <template v-slot:cell(edit)="row">
                    <b-button size="sm" @click="edit(row)" class="mr-2">
                        <i class="fas fa-sm fa-edit"></i>
                    </b-button>
                </template>
                <template v-slot:cell(delete)="row">
                    <b-button size="sm" @click="deleteItem(row)" class="mr-2">
                        <i class="fas fa-sm fa-trash-alt"></i>
                    </b-button>
                </template>

                <template v-slot:table-busy>
                    <div class="text-center text-danger my-2">
                        <b-spinner class="align-middle"></b-spinner>
                        <strong>Loading...</strong>
                    </div>
                </template>
            </b-table>

            <b-button v-if="$store.getters.isManager" v-b-modal.modal-item-add>Add item</b-button>
            <b-modal ref="modal-item-add" id="modal-item-add" title="Create new item" @ok="okAdd" @cancel="cancelAdd">
                <b-form-group id="fieldset-1"
                              label="Name"
                              label-for="input-new-itemName">
                    <b-form-input id="input-new-itemName" v-model="newItem.name" trim/>
                </b-form-group>

                <b-form-group id="fieldset-1"
                              label="Code"
                              label-for="input-new-code">
                    <b-form-input id="input-new-code" v-model="newItem.code" trim/>
                </b-form-group>

                <b-form-group id="fieldset-1"
                              label="Price"
                              label-for="input-new-price">
                    <b-form-input id="input-new-price" type="number" v-model.number="newItem.price" trim/>
                </b-form-group>

                <b-form-group id="fieldset-1"
                              label="Category"
                              label-for="input-new-category">
                    <b-form-input id="input-new-category" v-model="newItem.category" trim/>
                </b-form-group>

            </b-modal>

            <b-modal ref="modal-item" id="modal-user" :title="item.id" @ok="ok" @cancel="cancel">
                <b-form-group id="fieldset-1"
                              label="Name"
                              label-for="input-itemName">
                    <b-form-input id="input-itemName" v-model="item.name" trim/>
                </b-form-group>

                <b-form-group id="fieldset-1"
                              label="Code"
                              label-for="input-code">
                    <b-form-input id="input-code" v-model="item.code" trim/>
                </b-form-group>

                <b-form-group id="fieldset-1"
                              label="Price"
                              label-for="input-price">
                    <b-form-input id="input-price" type="number" v-model.number="item.price" trim/>
                </b-form-group>

                <b-form-group id="fieldset-1"
                              label="Catefory"
                              label-for="input-category">
                    <b-form-input id="input-category" v-model="item.category" trim/>
                </b-form-group>
            </b-modal>
        </div>
    </div>
</template>

<script>


    export default {
        name: 'Items',
        data() {
            return {
                isBusy: false,
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
                },
                filter: {
                    name: "",
                    code: "",
                    priceFrom: "",
                    priceTo: "",
                    category: ""
                }
            }
        },
        computed: {
            allItems: {
                get() {
                    return this.$store.getters.items;
                }
            },
            items: {
                get() {
                    return this.allItems.filter(e => e.name.startsWith(this.filter.name)
                        && e.code.startsWith(this.filter.code)
                        && e.category.startsWith(this.filter.category)
                        && (this.filter.priceFrom <= 0 || e.price >= this.filter.priceFrom)
                        && (this.filter.priceTo <= 0 || e.price <= this.filter.priceTo)
                    )
                }
            },
            fields: {
                get() {
                    if (this.$store.getters.isManager) return ['name', 'code', 'price', 'category', 'edit', 'delete'];
                    return ['name', 'code', 'price', 'category', 'cart'];
                }
            }
        },

        methods: {
            cart(row) {
                this.$store.dispatch('CART_ADD', row.item);
            },
            edit(row) {
                this.item.id = row.item.id;
                this.item.name = row.item.name;
                this.item.code = row.item.code;
                this.item.price = row.item.price;
                this.item.category = row.item.category;
                this.$refs['modal-item'].show();
            },
            async deleteItem(row) {
                await this.$store.dispatch('ITEM_DELETE', row.item.id);
                await this.fetchData();
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
            if (!this.$store.getters.isAuthenticated) {
                this.$router.push("/");
                return;
            }
            await this.fetchData();
        },

    }
</script>

<style scoped>
</style>
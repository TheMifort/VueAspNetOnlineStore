<template>
    <div class="container-fluid row">
        <div class="col-3" style="background: #00ffff">
            <h2>Filters</h2>
            <b-form-group id="fieldset-1"
                          label="Name"
                          label-for="input-filter-name">
                <b-form-input id="input-filter-name" v-model="filter.name" @input="filterItems" trim />
            </b-form-group>
            <b-form-group id="fieldset-1"
                          label="Code"
                          label-for="input-filter-code">
                <b-form-input id="input-filter-code" v-model="filter.code" @input="filterItems" trim />
            </b-form-group>
            <b-form-group id="fieldset-1"
                          label="Category"
                          label-for="input-filter-category">
                <b-form-input id="input-filter-category" v-model="filter.category" @input="filterItems" trim />
            </b-form-group>
            <b-form-group id="fieldset-1"
                          label="Price from"
                          label-for="input-filter-priceFrom">
                <b-form-input id="input-filter-priceFrom" type="number" v-model.number="filter.priceFrom" @input="filterItems" trim />
            </b-form-group>
            <b-form-group id="fieldset-1"
                          label="Price to"
                          label-for="input-filter-priceTo">
                <b-form-input id="input-filter-priceTo" type="number" v-model.number="filter.priceTo" @input="filterItems" trim />
            </b-form-group>
        </div>

        <div class="col-9">
            <b-table selectable
                     select-mode="single"
                     selected-variant="primary"
                     striped hover
                     :items="items"
                     :fields="fields"
                     :busy="isBusy"
                     @row-selected="onRowSelected"
                     @row-dblclicked="onRowDoubleClicked">

                <template v-slot:cell(cart)="row">
                    <b-button size="sm" @click="cart(row)" class="mr-2">
                        <i class="fas fa-sm fa-cart-plus"></i>
                    </b-button>
                </template>

                <template v-slot:table-busy>
                    <div class="text-center text-danger my-2">
                        <b-spinner class="align-middle"></b-spinner>
                        <strong>Loading...</strong>
                    </div>
                </template>
            </b-table>

            <b-button v-if="true" v-b-modal.modal-item-add>Add item</b-button>
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
    </div>
</template>

<script>


    export default {
        name: 'Items',
        data() {
            return {
                isBusy: false,
                fields: ['name', 'code', 'price', 'category', 'cart'],
                //items: [],
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
                }//,
                //cartItems: []
            }
        },
        computed: {
            allItems: {
                get() {
                    return this.$store.getters.items;
                }
            },
            //cartItems: {
            //    get() {
            //        return this.$store.getters.cartItems
            //    }
            //},
            items: {
                get() {
                    //for (let currentItem of this.allItems) {
                    //    if (this.cartItems.some(e => e.id == currentItem.id)) {
                    //        this.$set(currentItem, '_rowVariant', 'info');
                    //    }
                    //}
                    return this.allItems.filter(e => e.name.startsWith(this.filter.name)
                        && e.code.startsWith(this.filter.code)
                        && e.category.startsWith(this.filter.category)
                        && (this.filter.priceFrom <= 0 || e.price >= this.filter.priceFrom)
                        && (this.filter.priceTo <= 0 || e.price <= this.filter.priceTo)
                    )
                }
            }
        },

        methods: {
            cart(row) {
                this.$store.dispatch('CART_ADD', row.item);
            },
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
            },
            filterItems() {
                //this.items = this.allItems.filter(e => e.name.startsWith(this.filter.name)
                //    && e.code.startsWith(this.filter.code)
                //    && e.category.startsWith(this.filter.category)
                //    && (this.filter.priceFrom <= 0 || e.price >= this.filter.priceFrom)
                //    && (this.filter.priceTo <= 0 || e.price <= this.filter.priceTo)
                //);
            }
        },


        created: async function () {
            await this.fetchData();
        },
    }
</script>

<style scoped>
</style>
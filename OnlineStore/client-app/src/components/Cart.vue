<template>
    <div>
        <b-table selectable
                 select-mode="single"
                 selected-variant="primary"
                 striped hover
                 :items="cartItems"
                 :fields="fields"
                 :busy="isBusy">

            <template v-slot:cell(-)="row">
                <b-button size="sm" @click="minus(row)" class="mr-2">
                    <i class="fas fa-sm fa-minus"></i>
                </b-button>
            </template>
            <template v-slot:cell(+)="row">
                <b-button size="sm" @click="plus(row)" class="mr-2">
                    <i class="fas fa-sm fa-plus"></i>
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
        <b-button v-if="$store.getters.hasCustomer" @click="order">Order</b-button>
    </div>
</template>

<script>


    export default {
        name: 'Items',
        data() {
            return {
                isBusy: false,
                fields: ['name', 'code', 'price', 'category', '-', 'count', '+', 'delete'],
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
            cartItems: {
                get() {
                    return this.$store.getters.cartItems;
                }
            }
        },

        methods: {
            plus(row) {
                this.$store.commit('CART_INC', row.item);
                this.$store.commit('CART_SAVE');
            },
            minus(row) {
                this.$store.commit('CART_DEC', row.item);
                this.$store.commit('CART_SAVE');
            },
            async deleteItem(row) {
                await this.$store.dispatch('CART_DELETE', row.item.id);
            },
            async order() {
                await this.$store.dispatch('ORDER');
                this.$store.commit('CART_CLEAR');
            }
        },
        created() {
            if (!this.$store.getters.isUser)
                this.$router.push("/");
        }
    }
</script>

<style scoped>
</style>
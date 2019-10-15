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
                <b-button size="sm" @click="row.toggleDetails" class="mr-2">
                    {{ row.detailsShowing ? 'Hide' : 'Show'}} Details
                </b-button>
            </template>

            <template v-slot:table-busy>
                <div class="text-center text-danger my-2">
                    <b-spinner class="align-middle"></b-spinner>
                    <strong>Loading...</strong>
                </div>
            </template>
        </b-table>
    </div>
</template>

<script>


    export default {
        name: 'Items',
        data() {
            return {
                isBusy: false,
                fields: ['name', 'code', 'price', 'category','-','count','+'],
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

        }
    }
</script>

<style scoped>
</style>
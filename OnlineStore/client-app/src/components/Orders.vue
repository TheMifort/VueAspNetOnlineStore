<template>
    <div>
        <b-table striped hover
                 :items="orders"
                 :fields="fields"
                 :busy="isBusy">

            <template v-slot:cell(confirm)="row">
                <b-button size="sm" @click="cart(row)" class="mr-2">
                    <i class="fas fa-sm fa-cart-plus"></i>
                </b-button>
            </template>
            <template v-slot:cell(complete)="row">
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
    </div>
</template>

<script>
    import moment from 'moment'
    export default {
        name: "Orders",
        data() {
            return {
                isBusy: false,
                fields: ['orderNumber',
                    {
                        key: 'orderDate', formatter: value => {
                            if (value === null) return '-';
                            return moment(value).format('DD.MM.YYYY');
                        }
                    }, {
                        key: 'shipmentDate', formatter: value => {
                            if (value === null) return '-';
                            return moment(value).format('DD.MM.YYYY');
                        }
                    }, 'confirm', 'complete', 'delete'],
            }
        },
        computed: {
            orders: {
                get() {
                    return this.$store.getters.orders;
                }
            }
        },

        methods: {
            async fetchData() {
                this.isBusy = true;
                await this.$store.dispatch('ORDERS_REQUEST');
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
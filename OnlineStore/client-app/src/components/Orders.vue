<template>
    <div>
        <b-table striped hover
                 :items="orders"
                 :fields="fields"
                 :busy="isBusy">

            <template v-slot:cell(confirm)="row">
                <b-button @click="confirm(row)"><i class="fas fa-sm fa-cart-plus"></i></b-button>
            </template>
            <template v-slot:cell(complete)="row">
                <b-button @click="complete(row)"><i class="fas fa-sm fa-edit"></i></b-button>
            </template>
            <template v-slot:cell(delete)="row">
                <b-button size="sm" v-if="row.item.status === 0" @click="deleteOrder(row)" class="mr-2">
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

        <b-modal ref="modal-order-confirm" id="modal-order-confirm" title="Confirm order" @ok="okConfirm"
                 @cancel="cancelConfirm">
            <b-form-group id="fieldset-1"
                          label="Shipment date"
                          label-for="input-shipmentDate">
                <b-form-input id="input-shipmentDate" type="date" v-model="order.shipmentDate" trim/>
            </b-form-group>
        </b-modal>

    </div>
</template>

<script>
    import moment from 'moment'

    export default {
        name: "Orders",
        data() {
            return {
                isBusy: false,
                order: {},

            }
        },
        computed: {
            orders: {
                get() {
                    return this.$store.getters.orders;
                }
            },
            fields: {
                get() {
                    if (this.$store.getters.isManager) return ['orderNumber',
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
                        }, 'confirm', 'complete', 'delete'];
                    return ['orderNumber',
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
                        }, 'delete'];
                }
            }
        },

        methods: {
            async fetchData() {
                this.isBusy = true;
                await this.$store.dispatch('ORDERS_REQUEST');
                this.isBusy = false;
            },
            confirm(row) {
                if (row.item.status !== 0) return;

                this.order.id = row.item.id;
                this.$refs['modal-order-confirm'].show();
            },
            async complete(row) {
                if (row.item.status !== 1) return;

                await this.$store.dispatch('ORDER_COMPLETE', row.item);
                await this.fetchData();
            },
            async okConfirm() {
                await this.$store.dispatch('ORDER_CONFIRM', this.order);
                await this.fetchData();
            },
            cancelConfirm() {
                this.order = {};
                this.order = {};
            },
            async deleteOrder(row) {
                await this.$store.dispatch('ORDER_DELETE', row.item);
                await this.fetchData();
            }
        },


        created: async function () {
            if (!this.$store.getters.isUser && !this.$store.getters.isManager) {
                this.$router.push("/");
                return;
            }
            await this.fetchData();
        },
    }
</script>

<style scoped>

</style>
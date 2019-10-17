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
    export default {
        name: "Orders",
        data() {
            return {
                isBusy: false,
                fields: ['orderNumber', 'orderDate', 'shipmentDate', 'status', 'confirm', 'complete', 'delete'],
            }
        },
        computed: {
            orders: {
                get() {
                    return this.$store.getters.orders;
                }
            }
        }
    }
</script>

<style scoped>

</style>
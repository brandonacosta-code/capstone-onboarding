import { Controller } from "../0-kendo-wrapper/index.js";
import { KendoController } from "../0-kendo-wrapper/kendo.controller.js";
import { CartVM } from "../view-model/cart.vm.js";

@Controller({
    templateUrl: '/view/cart',
    viewModel: CartVM
})
export class CartController extends KendoController<CartVM> {
    onInit(e: any) {

    }

    onShow(e: any) {
        this.model.init(); 
    }
}
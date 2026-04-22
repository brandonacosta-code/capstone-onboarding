declare var kendo: any;
import { Controller, KendoController } from "../0-kendo-wrapper/index.js";
import { ProductVM } from '../view-model/product.vm.js';

@Controller({
    templateUrl: "/view/product",
    viewModel: ProductVM
})
export class ProductController extends KendoController<ProductVM> {
    onInit(e:any) {
    }
 
}
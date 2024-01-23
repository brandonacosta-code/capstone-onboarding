import { Controller, KendoController } from "../0-kendo-wrapper/index.js";

import { HomeVM } from "../view-model/home.vm.js";

@Controller({
    templateUrl: "/view/home",
    viewModel: HomeVM
})
export class HomeController extends KendoController<HomeVM> {
    onInit() {
        // confirm("Are you sure you want to navigate to the home page?");
    }
}
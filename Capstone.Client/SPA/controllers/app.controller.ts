import { Controller, KendoController } from "../0-kendo-wrapper/index.js";

import { AppVM } from "../view-model/app.vm.js";
import { RouterInstance } from "../router.js";

@Controller({
    templateUrl: "/view",
    viewModel: AppVM
})
export class AppController extends KendoController<AppVM> {

    public name = "AppController";
    private routerSub?: string;

    public onInit(): void {

        this.routerSub = RouterInstance.$currentPath.subscribe((url) => {
            this.view.element.find("a").parent('li').removeClass("active");
            this.view.element.find(`a[href="#${url}"]`).parent('li').addClass("active");
        });

    }

    public onDestroy(): void {
        if (!!this.routerSub) {
            RouterInstance.$currentPath.unsubscribe(this.routerSub);
        }
    }
}

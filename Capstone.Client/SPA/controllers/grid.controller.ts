import { Controller, KendoController } from "../0-kendo-wrapper/index.js";

import { GridVM } from "../view-model/grid.vm.js";

@Controller({
    templateUrl: "/view/grid",
    viewModel: GridVM
})
export class GridController extends KendoController<GridVM> {
    public onInit(): void { }
}
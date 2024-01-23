import { templateMap, viewModelMap } from "./kendo-decorator.map.js"

/**
 * Registers a controller with a templateUrl and view model.
 * @param config 
 * @returns 
 */
export function Controller({ templateUrl, viewModel }: { templateUrl: string, viewModel: new () => object }) {

    return function <T extends { new(...args: unknown[]): object }>(constructor: T, ...args: unknown[]) {
        templateMap.set(constructor.name, templateUrl);
        viewModelMap.set(constructor.name, viewModel);
    }
}



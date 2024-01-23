import { templateMap, viewModelMap } from "./decorators/kendo-decorator.map.js";

import { KendoController } from "./kendo.controller.js";
import { ResourseFetcher } from "./resourses.fetcher.js";

const templateCache = new Map<string, string>();

export const modelMap = new Map<string, kendo.data.ObservableObject>();

export class KendoViewBuilder {

    /**
     * Builds a kendo view from a controller.
     * @param component 
     * @returns 
     */
    public static async build(component: KendoController) {

        const templateURL = templateMap.get(component.constructor.name);
        const viewModel = viewModelMap.get(component.constructor.name);
        if (!templateURL) {
            throw new Error(`Template for ${component.constructor.name} not found!`);
        }

        if (!viewModel) {
            throw new Error(`ViewModel for ${component.constructor.name} not found!`);
        }

        if (!templateCache.has(templateURL)) {
            templateCache.set(templateURL, await ResourseFetcher.fetchTemplate(templateURL));
        }

        const template = templateCache.get(templateURL)!;

        const vm = new viewModel();
        const model = KendoViewBuilder.bindObjects(vm);

        const view = new kendo.Layout(template, {
            model,
            init: (e) => component.onInit(e),
            show: (e) => component.onShow?.(e),
            hide: (e) => component.onHide?.(e)
        });

        component.view = view;
        component.model = vm;

        return component;
    }

    private static bindObjects(viewModel: object) {

        const obj = Object.assign({}, viewModel);

        Object.getOwnPropertyNames(viewModel.constructor.prototype).filter((prop) => prop !== 'constructor').forEach((prop) => {
            Object.defineProperty(obj, prop, {
                value: (...args: any[]) => (viewModel as any)[prop](...args),
                writable: false
            });
        });

        const model = kendo.observable(obj);

        Object.getOwnPropertyNames(viewModel).forEach((prop) => {
            let savedValue = (viewModel as any)[prop] || undefined;
            Object.defineProperty(viewModel, prop, {
                set: (value) => {
                    savedValue = value;
                    model.set(prop, value);
                },
                get: () => savedValue
            });
        });
        return model;
    }
}
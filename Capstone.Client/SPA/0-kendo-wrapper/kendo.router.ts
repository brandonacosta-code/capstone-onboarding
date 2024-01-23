import { KendoController } from "./kendo.controller.js";
import { KendoViewBuilder } from "./kendo.view-builder.js";
import { Observale } from "./observable.js";

export type KendoRoutes = {
    /**
     * The path to navigate to when the route is matched.
     */
    path: string;
    /**
     * The component to load when the route is matched.
     */
    component: string;
    /**
     * The title of the page.
     */
    tile?: string;
}[];

export type KendoRouterOptions = {
    /**
     * The base path for the controllers.
     */
    controllerBasePath?: string;
    /**
     * The routes to match.
     */
    routes: KendoRoutes;
    /**
     * The selector for the output component.
     */
    outputSelector: `#${string}`;
} & kendo.RouterOptions;

export class KendoRouter {
    private readonly router: kendo.Router;
    private readonly routes: KendoRoutes;
    private outputComponent?: kendo.Layout;
    private readonly outputSelector: KendoRouterOptions['outputSelector'];

    private _pathVariables: Record<string, string> = {};
    private _queryParameters: Record<string, string> = {};
    private readonly _currentPath = new Observale<string>('');
    public get $currentPath() {
        return this._currentPath;
    }
    public readonly $pathVariables: Observale<Record<string, string>>;
    public readonly $queryParameters: Observale<Record<string, string>>;
    basePath: string;

    private set pathVariables(value: Record<string, string>) {
        this._pathVariables = value;
        this.$pathVariables.publish(this.pathVariables);
    }

    public get pathVariables(): Record<string, string> {
        return { ...this._pathVariables };
    }

    private set queryParameters(value: Record<string, string>) {
        this._queryParameters = value;
        this.$queryParameters.publish(this.queryParameters);
    }

    public get queryParameters(): Record<string, string> {
        return { ...this._queryParameters };
    }

    constructor(
        options: KendoRouterOptions
    ) {
        this.router = new kendo.Router(options);
        this.routes = [...options.routes];
        this.outputSelector = options.outputSelector;
        this.basePath = options.controllerBasePath ?? '';

        this.$pathVariables = new Observale<Record<string, string>>(this.pathVariables);
        this.$queryParameters = new Observale<Record<string, string>>(this.queryParameters);

        this.setUp();
    }

    private setUp(): void {
        let currentController: KendoController<object>;
        for (const route of this.routes) {
            this.router.route(route.path, async (...args: unknown[]) => {

                const pathVariablesNames = route.path.match(/:[a-zA-Z0-9]+/g)?.map(x => x.replace(':', '')) ?? [];

                const pathVariables: Record<string, string> = {};
                for (let i = 0; i < pathVariablesNames.length; i++) {
                    pathVariables[pathVariablesNames[i]] = args[i] as string;
                }

                this.pathVariables = pathVariables;
                this.queryParameters = args[args.length - 1] as Record<string, string>;
                this._currentPath.publish(route.path);

                const module = (await import(`${this.basePath}${route.component}`));
                const key = Object.keys(module)[0];
                const controller: new () => KendoController = module[key];
                const viewController = await KendoViewBuilder.build(new controller());
                currentController?.onDestroy();
                currentController = viewController;
                document.title = route.tile ?? 'Capstone';

                this.outputComponent?.showIn(this.outputSelector, viewController.view);

            });
        }
    }

    /**
     * Starts the router.
     * @param outputComponent The component to show the views in.
     */
    public start(outputComponent: kendo.Layout) {
        this.outputComponent = outputComponent;
        this.router.start();
    }

    /**
     * Destroys the router.
     */
    public destroy(): void {
        this.router.destroy();
    }

    /**
     * Navigate to the given location.
     * @param route  The route to navigate to.
     * @param silent If set to true, the router callbacks will not be called.
     */
    public navigate(route: string, silent?: boolean): void {
        this.router.navigate(route, silent);
    }

    /**
     * Navigates to the given route, replacing the current view in the history stack
     * (like `window.history.replaceState` or `location.replace` work).
     * @param location 
     * @param silent 
     */
    public replace(location: string, silent?: boolean): void {
        this.router.replace(location, silent);
    }

}
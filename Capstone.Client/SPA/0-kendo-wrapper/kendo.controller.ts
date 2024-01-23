
/**
 * Base class for all controllers.
 */
export abstract class KendoController<T = object> {

    /**
     * The view to load.
     */
    public view!: kendo.Layout;
    public model!: T;

    /**
     * This method is called when the view is initialized.
     * @param e 
     */
    public abstract onInit(e: kendo.ViewEvent): void;

    /**
     * This method is called when the view is destroyed.
     */
    public onDestroy() { }

    /**
     * This method is called when the view is shown.
     * @param e 
     */
    public onShow?(e: kendo.ViewEvent): void;

    /**
     * this method is called when the view is hidden.
     * @param e 
     */
    public onHide?(e: kendo.ViewEvent): void;

}
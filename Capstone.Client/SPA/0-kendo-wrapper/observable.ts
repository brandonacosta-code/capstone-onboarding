type Optional<T> = T | undefined;

/**
 * A simple observable
 * @template T
 */
export class Observale<T = any> {
    private readonly listeners: Record<string, (args?: T) => void> = {};
    private _currentData: Optional<T>;

    /**
     * The current data
     */
    public get currentData(): Optional<T> {
        return this._currentData;
    }

    constructor(initData?: T) {
        this._currentData = initData;
    }

    /**
     * Don't forget to unsubscribe
     * @param callback 
     * @returns 
     */
    public subscribe(callback: (data?: T) => void) {
        let id = Math.random().toString(36).slice(2, 9);
        let numberOfTries = 0;
        while (id in this.listeners) {
            id = Math.random().toString(36).slice(2, 9);
            numberOfTries++;
            if (numberOfTries > 100) {
                throw new Error("Unable to generate unique id");
            }
        }
        this.listeners[id] = callback;
        if (this._currentData) {
            callback(this._currentData);
        }
        
        return id;
    }

    /**
     * Pushes data to all subscribers
     * @param data 
     */
    public publish(data?: T): void {

        this._currentData = data;

        Object.entries(this.listeners).forEach(([key, cb]) => cb(this._currentData));
    }

    /**
     * Unsibscribes from the observable
     * @param id 
     * @returns 
     */
    public unsubscribe(id: string): void {
        if (!this.listeners[id]) {
            return;
        }

        delete this.listeners[id];
    }
}
export class ResourseFetcher {
    /**
     * Fetches a template from a url.
     * @param url 
     * @returns 
     */
    public static async fetchTemplate(url: string): Promise<string> {
        return fetch(url)
            .then(response => response.text())
            .catch(error => console.log(error)) as Promise<string>;
    }

    public static async fetchModel<Return = any>(url: string): Promise<Return> {
        return fetch(url)
            .then(response => response.json())
            .catch(error => console.log(error));
    }
}
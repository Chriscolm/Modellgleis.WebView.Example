import { Renderer } from "/Scripts/renderer.js";

class Loader {
    load() {        
        fetch('/data/datamodel.json')
            .then(response => response.json())
            .then(data => this.render(data));
    };    

    render(data) {
        const renderer = new Renderer();
        renderer.init();
        renderer.createScene(data);
        renderer.render();
    };
}

export { Loader }
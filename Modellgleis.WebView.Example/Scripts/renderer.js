import * as THREE from '/Scripts/three.module.js';
import { OrbitControls } from '/Scripts/OrbitControls.js';

class Renderer {    

    constructor() {
        this.scene = null;
        this.camera = null;
        this.renderer = null;
        this.controls = null;
        this.isInitialized = false;
    };

    init() {
        if (this.isInitialized) {
            return;
        }
        const scene = new THREE.Scene();
        const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);

        const renderer = new THREE.WebGLRenderer();
        renderer.setSize(window.innerWidth, window.innerHeight);
        document.body.appendChild(renderer.domElement);

        camera.position.z = 5;
        const controls = new OrbitControls(camera, renderer.domElement);
        

        this.scene = scene;
        this.camera = camera;
        this.renderer = renderer;
        this.controls = controls;
                
        const r = function () {
            renderer.render(scene, camera);  
        }

        controls.addEventListener('change', r);
        controls.addEventListener('resize', r);

        this.isInitialized = true;
    };

    createScene(data) {
        const geometry = new THREE.BoxGeometry(data.Width, data.Height, data.Depth);
        const colors = [new THREE.Color(1, 0, 0), new THREE.Color(0, 1, 0), new THREE.Color(0, 0, 1)];
        
        for (var i = 0; i < 3; i++) {
            geometry.faces[4 * i].color = colors[i];
            geometry.faces[4 * i + 1].color = colors[i];
            geometry.faces[4 * i + 2].color = colors[i];
            geometry.faces[4 * i + 3].color = colors[i];
        }

        const material = new THREE.MeshBasicMaterial({ vertexColors: true });
        const cube = new THREE.Mesh(geometry, material);
        this.scene.add(cube);
    };

    render() {           
        this.renderer.render(this.scene, this.camera);    
    };
};

export { Renderer }
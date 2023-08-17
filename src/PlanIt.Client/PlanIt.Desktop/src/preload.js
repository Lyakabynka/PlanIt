const { contextBridge, ipcRenderer } = require('electron');

function init() {
    contextBridge.exposeInMainWorld('planProcessor', {
        process: (plan) => {
            console.log("After send: \n" + plan);
            return ipcRenderer.send('process_plan_desktop', plan);
        }
    });
    contextBridge.exposeInMainWorld('electron');
}

init();
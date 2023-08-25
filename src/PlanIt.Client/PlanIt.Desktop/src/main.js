const signalR = require('@microsoft/signalr');
const { Notification, ipcRenderer, BrowserView } = require('electron');
const say = require('say');

// Modules to control application life and create native browser window
const { shell, app, BrowserWindow, ipcMain } = require('electron')
const path = require('path')

const loudness = require('loudness');
const EnumPlanType = require('./enums/EnumPlanType.js');
const { title } = require('process');

const { CHANNELS } = require('./shared/constants.js');


const createWindow = () => {
    // Create the browser window.
    const mainWindow = new BrowserWindow({
        width: 800,
        height: 600,
        webPreferences: {
            preload: path.join(__dirname, 'preload.js'),
            nodeIntegration: false,
            contextIsolation: true,
            allowRunningInsecureContent: true,
        }
    })


    // and load the index.html of the app.
    mainWindow.loadURL('http://localhost:3000/')

    mainWindow.webContents.openDevTools();

    // Open the DevTools.
    // mainWindow.webContents.openDevTools()
}

// This method will be called when Electron has finished
// initialization and is ready to create browser windows.
// Some APIs can only be used after this event occurs.
app.whenReady().then(() => {
    createWindow()

    app.on('activate', () => {
        // On macOS it's common to re-create a window in the app when the
        // dock icon is clicked and there are no other windows open.
        if (BrowserWindow.getAllWindows().length === 0) createWindow()
    })
})

app.on('ready', () => {
    ipcMain.on('process_plan_desktop', async (event, plan) => {
        switch (plan.planType) {
            //notification
            case EnumPlanType.notification:
                new Notification({
                    title: plan.name,
                    body: plan.information,
                }).show()
                break;
            //open browser
            case EnumPlanType.openBrowser:
                shell.openExternal(plan.information);
                break;
            //open desktop
            case EnumPlanType.openDesktop:
                shell.openPath(plan.information);
                break;
            //set the volume
            case EnumPlanType.volume:
                loudness.setVolume(Number.parseInt(plan.information));
                break;
            //
            case EnumPlanType.focusOn:
                
                break;
        }
    })
})

// Quit when all windows are closed, except on macOS. There, it's common
// for applications and their menu bar to stay active until the user quits
// explicitly with Cmd + Q.
app.on('window-all-closed', () => {
    if (process.platform !== 'darwin') app.quit()
})
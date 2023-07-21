const signalR = require('@microsoft/signalr');
const { Notification, ipcMain } = require('electron');
const say = require('say');
// main.js

// Modules to control application life and create native browser window
const { shell, app, BrowserWindow } = require('electron')
const path = require('path')

const loudness = require('loudness');

const createWindow = () => {
    // Create the browser window.
    const mainWindow = new BrowserWindow({
        width: 800,
        height: 600,
        webPreferences: {
            preload: path.join(__dirname, 'preload.js')
        }
    })

    // and load the index.html of the app.
    mainWindow.loadFile('./src/index.html')

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

// Quit when all windows are closed, except on macOS. There, it's common
// for applications and their menu bar to stay active until the user quits
// explicitly with Cmd + Q.
app.on('window-all-closed', () => {
    if (process.platform !== 'darwin') app.quit()
})

EstablishConnection();

async function EstablishConnection() {
    let connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5003/plan-hub")
        .configureLogging(signalR.LogLevel.Information)
        .withAutomaticReconnect()
        .build();

    connection.on("ProcessPlan", (plan) => {

        console.log(plan);

        switch (plan.type) {
            //notification
            case 0:
                new Notification({
                    title: plan.name,
                    body: plan.information,
                }).show()
                break;
            //open browser
            case 1:
                shell.openExternal(plan.information);
                break;
            //open desktop
            case 2:
                shell.openPath(plan.information);
                break;
            //execute script
            case 3:

                break;
            //voice command
            case 4:
                say.speak(plan.information, 'Microsoft Zira Desktop', 1);
                break;
            //set the volume
            case 5:
                loudness.getVolume().then(currentVolume => {
                    loudness.setVolume(currentVolume + Number.parseInt(plan.information));
                });
                break;
            //special command
            case 6:
                
                break;
            //weather command
            case 7:
            
                break;
            case 8:

                break;
        }


        console.log(plan);
    });

    connection.onreconnecting(() => {
        console.log('Connection with server has been lost. Trying to reconnect...');
    })

    await connection.start().then(() => {
        console.log("yeey!");
    }).catch((e) => {
        console.log("Server is offline");
        console.log(e);
    })

    await connection.invoke('SubscribeToPlan', 'dda29bef-0069-473e-b652-dc0136ec90f2')
        .catch(err => {
            console.log(err);
        });
}
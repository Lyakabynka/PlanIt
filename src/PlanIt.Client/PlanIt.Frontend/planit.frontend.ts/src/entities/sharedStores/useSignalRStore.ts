import * as signalR from "@microsoft/signalr";
import { EnumPlanType, EnumPlatform, IPlan, useAuthStore } from "..";
import { CHANNELS } from "../../shared/electron/channels";
import { create } from "zustand";
import { wait } from "@testing-library/user-event/dist/utils";

declare global {
    interface Window {
        electron: boolean | undefined | null;
        planProcessor: any;
    }
}

interface ISignalRStore {
    connection: signalR.HubConnection | null,
    isElectron: () => boolean,
    establishConnection: (userId: string) => Promise<void>,
    closeConnection: () => Promise<void>
}

export const useSignalRStore = create<ISignalRStore>()((set, get) => ({

    connection: null,

    isElectron: () => {
        console.log(window.electron);

        return window.electron !== null && window.electron !== undefined;
    },

    establishConnection: async (userId) => {

        if (get().connection !== null) {
            await get().closeConnection();
        }

        set({
            connection: new signalR.HubConnectionBuilder()
                .withUrl("http://localhost:5003/plan-hub")
                .configureLogging(signalR.LogLevel.Information)
                .withAutomaticReconnect()
                .build()
        })

        const { connection } = get();

        connection!.on("ProcessPlan", (plan) => {

            console.log(plan);

            switch (plan.planType) {
                case EnumPlanType.notification:

                    break;
                case EnumPlanType.voiceCommand:
                    const synthesis = window.speechSynthesis;

                    const utterance = new SpeechSynthesisUtterance(plan.information)
                    utterance.voice = synthesis.getVoices()[4];

                    synthesis.speak(utterance);
                    break;
                case EnumPlanType.weatherCommand:

                    break;
            }

            if (get().isElectron()) {
                window.planProcessor.process(plan);
            }
        });

        connection!.onreconnecting(() => {
            console.warn('Connection with server has been lost. Trying to reconnect...');
        })

        await connection!.start().then(() => {
            console.info("Connection with SignalR hub has been successfully established!");
        }).catch((e) => {
            console.log("Server is offline");
            console.log(e);
        })

        await connection!.invoke('SubscribeToPlan', userId)
            .then(() => {
                console.log("Subscribed to plan: " + userId);
            })
            .catch(err => {
                console.log(err);
            });
    },

    closeConnection: async () => {
        const { connection } = get();
        await connection?.stop();
        set({ connection: null });
    }
}))
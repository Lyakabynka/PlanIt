interface PlanType {
    notification: string
    openBrowser: string,
    openDesktop: string,
    voiceCommand: string,
    volume: string,
    weatherCommand: string,
    focusOn: string,
}

export const EnumPlanType: PlanType = {
    notification: "Notification",
    openBrowser: "OpenBrowser",
    openDesktop: "OpenDesktop",
    voiceCommand: "VoiceCommand",
    volume: "Volume",
    weatherCommand: "WeatherCommand",
    focusOn: "FocusOn"
}
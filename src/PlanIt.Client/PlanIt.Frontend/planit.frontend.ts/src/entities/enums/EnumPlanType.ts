interface PlanType {
    notification: string
    openBrowser: string,
    openDesktop: string,
    executeScript: string,
    voiceCommand: string,
    volume: string,
    specialCommand: string,
    weatherCommand: string
}

export const EnumPlanType: PlanType = {
    notification: "Notification",
    openBrowser: "OpenBrowser",
    openDesktop: "OpenDesktop",
    executeScript: "ExecuteScript",
    voiceCommand: "VoiceCommand",
    volume: "Volume",
    specialCommand: "SpecialCommand",
    weatherCommand: "WeatherCommand"
}
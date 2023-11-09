interface ScheduleType {
    instant: string
    oneOff: string,
    recurring: string,
}

export const EnumScheduledPlanType: ScheduleType = {
    instant: "Instant",
    oneOff: "OneOff",
    recurring: "Recurring"
}
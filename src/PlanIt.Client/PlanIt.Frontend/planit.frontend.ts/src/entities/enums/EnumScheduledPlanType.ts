interface ScheduledPlanType {
    instant: string
    oneOff: string,
    recurring: string,
}

export const EnumScheduledPlanType: ScheduledPlanType = {
    instant: "Instant",
    oneOff: "OneOff",
    recurring: "Recurring"
}
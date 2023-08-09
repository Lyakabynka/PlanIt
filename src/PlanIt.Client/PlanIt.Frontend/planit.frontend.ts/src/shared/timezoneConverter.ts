export const UTCToLocalStringTime = (date: Date): string => {

    const localTime = new Date(date);
    const offsetInMinutes = localTime.getTimezoneOffset();
    const offsetInMilliseconds = offsetInMinutes * 60 * 1000;

    return toDateTimeString(new Date(localTime.getTime() - offsetInMilliseconds));
}

export const toDateTimeString = (date: Date) : string => {

    return [new Date(date).toLocaleDateString(), new Date(date).toLocaleTimeString()].join(' ');
}


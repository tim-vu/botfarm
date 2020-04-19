export const valueInRange = (min: number, max: number) => (value: number) => {
    return (
        (value >= min && value <= max) ||
        `This value must be greater than ${min - 1} and less than ${max + 1}`
    );
};

export const transformInt = ([event]: any[]) => {
    const number = parseInt(event.target.value);
    return isNaN(number) ? undefined : number;
};

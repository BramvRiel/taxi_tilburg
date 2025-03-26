export function format(str: string, args: { [key: string]: any }): string {
    return str.replace(/{(\w+)}/g, (match, name) => {
        return name in args ? args[name] : match;
    });
}

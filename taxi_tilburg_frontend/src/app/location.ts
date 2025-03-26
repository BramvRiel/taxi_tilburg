import { LocationConnection } from "./location-connection";

export interface Location {
id: number;
name: string;
latitude: number;
longitude: number;
canTravelTo?: LocationConnection[];
}

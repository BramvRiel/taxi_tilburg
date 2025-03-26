import { Injectable } from '@angular/core';
import { Location } from './location';
import { Observable, of } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { format } from './string-utils'; // Import the format function


@Injectable({
  providedIn: 'root'
})
export class LocationsService {
  private locationsUrl = 'https://localhost:7084/database/locations';
  private locationDetailsUrl = 'https://localhost:7084/database/location/{id}';

  constructor(private http: HttpClient) { }

  getLocations(): Observable<Location[]> {
    const locations = this.http.get<Location[]>(this.locationsUrl);
    return locations;
  }

  getLocationDetails(locationId: number): Observable<Location> {
    return this.http.get<Location>(format(this.locationDetailsUrl, { id: locationId }));
  }
}

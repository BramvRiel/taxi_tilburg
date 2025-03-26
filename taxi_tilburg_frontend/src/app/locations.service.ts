import { Injectable } from '@angular/core';
import { Location } from './location';
import { Observable, of } from 'rxjs';
//import { DEFAULT_LOCATIONS } from './mock-locations';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class LocationsService {
  private heroesUrl = 'https://localhost:7084/database/locations';

  constructor(private http: HttpClient) { }

  getHeroes(): Observable<Location[]> {
    return this.http.get<Location[]>(this.heroesUrl);
    //const heroes = of(DEFAULT_LOCATIONS);
    //return heroes;
  }
}

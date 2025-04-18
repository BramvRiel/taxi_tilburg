import { Component } from '@angular/core';
import { Location } from '../location';
import { CommonModule, NgFor } from '@angular/common';
import { LocationDetailComponent } from '../location-detail/location-detail.component';
import { LocationsService } from '../locations.service';

@Component({
  selector: 'app-location',
  standalone: true,
  imports: [CommonModule, NgFor, LocationDetailComponent],
  templateUrl: './location.component.html',
  styleUrl: './location.component.scss'
})
export class LocationComponent {
  locations: Location[] = [];
  constructor(private locationsService: LocationsService) { }

  ngOnInit(): void {
    this.getLocations();
  }

  getLocations(): void {
    this.locationsService.getLocations()
      .subscribe(locations => {
        this.locations = locations;
        for (let i = 0; i < this.locations.length; i++) {
          this.locationsService.getLocationDetails(this.locations[i].id)
            .subscribe(location => {
              this.locations[i].canTravelTo = location.canTravelTo;
            });
        }
      });
  }

  selectedLocation?: Location;

  onSelect(location: Location): void {
    this.selectedLocation = location;
  }
}

import { Component } from '@angular/core';
import { Location } from '../location';
import { DEFAULT_LOCATIONS } from '../mock-locations';
import { CommonModule, NgFor } from '@angular/common';
import { LocationDetailComponent } from '../location-detail/location-detail.component';

@Component({
  selector: 'app-location',
  standalone: true,
  imports: [CommonModule, NgFor, LocationDetailComponent],
  templateUrl: './location.component.html',
  styleUrl: './location.component.scss'
})
export class LocationComponent {
  locations = DEFAULT_LOCATIONS;
  selectedLocation?: Location;

  onSelect(location: Location): void {
    this.selectedLocation = location;
  }
}

import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Location } from '../location';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-location-detail',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './location-detail.component.html',
  styleUrl: './location-detail.component.scss'
})
export class LocationDetailComponent {
  @Input() location?: Location;
}

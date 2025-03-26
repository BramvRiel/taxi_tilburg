import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LocationComponent } from "./location/location.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, LocationComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'taxi_tilburg_frontend';
}

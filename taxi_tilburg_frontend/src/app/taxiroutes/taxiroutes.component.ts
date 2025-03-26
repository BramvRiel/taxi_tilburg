import { Component } from '@angular/core';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-taxiroutes',
  standalone: true,
  imports: [CommonModule, FormsModule, NgFor, NgIf],
  templateUrl: './taxiroutes.component.html',
  styleUrl: './taxiroutes.component.scss'
})
export class TaxiroutesComponent {

}

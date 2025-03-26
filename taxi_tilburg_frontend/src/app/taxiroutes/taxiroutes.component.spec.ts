import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaxiroutesComponent } from './taxiroutes.component';

describe('TaxiroutesComponent', () => {
  let component: TaxiroutesComponent;
  let fixture: ComponentFixture<TaxiroutesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaxiroutesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TaxiroutesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

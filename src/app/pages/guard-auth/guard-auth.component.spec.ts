import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GuardAuthComponent } from './guard-auth.component';

describe('GuardAuthComponent', () => {
  let component: GuardAuthComponent;
  let fixture: ComponentFixture<GuardAuthComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GuardAuthComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GuardAuthComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

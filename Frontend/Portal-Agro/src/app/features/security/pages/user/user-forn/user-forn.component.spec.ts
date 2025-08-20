import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserFornComponent } from './user-forn.component';

describe('UserFornComponent', () => {
  let component: UserFornComponent;
  let fixture: ComponentFixture<UserFornComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserFornComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserFornComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

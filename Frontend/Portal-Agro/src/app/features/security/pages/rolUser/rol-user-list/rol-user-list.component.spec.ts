import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RolUserListComponent } from './rol-user-list.component';

describe('RolUserListComponent', () => {
  let component: RolUserListComponent;
  let fixture: ComponentFixture<RolUserListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RolUserListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RolUserListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

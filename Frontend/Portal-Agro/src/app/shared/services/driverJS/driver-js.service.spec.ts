import { TestBed } from '@angular/core/testing';

import { DriverJsService } from './driver-js.service';

describe('DriverJsService', () => {
  let service: DriverJsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DriverJsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

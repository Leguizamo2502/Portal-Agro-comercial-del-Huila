import { TestBed } from '@angular/core/testing';

import { FarmImageService } from './farm-image.service';

describe('FarmImageService', () => {
  let service: FarmImageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FarmImageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

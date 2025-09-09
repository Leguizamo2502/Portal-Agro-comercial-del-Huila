import { TestBed } from '@angular/core/testing';

import { GenericService } from './generic.service';

describe('GenericService', () => {
  let service: GenericService<any, any>;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GenericService<any, any>);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

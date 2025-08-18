import { TestBed } from '@angular/core/testing';
import { CanMatchFn } from '@angular/router';

import { producerMatchGuard } from './producer-match.guard';

describe('producerMatchGuard', () => {
  const executeGuard: CanMatchFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => producerMatchGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});

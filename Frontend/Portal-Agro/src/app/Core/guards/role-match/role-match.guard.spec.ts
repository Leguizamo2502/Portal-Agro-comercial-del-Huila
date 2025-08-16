import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { roleMatchGuard } from './role-match.guard';

describe('roleMatchGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => roleMatchGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});

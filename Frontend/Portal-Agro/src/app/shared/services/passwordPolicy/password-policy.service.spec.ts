import { TestBed } from '@angular/core/testing';

import { PasswordPolicyService } from './password-policy.service';

describe('PasswordPolicyService', () => {
  let service: PasswordPolicyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PasswordPolicyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

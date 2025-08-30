import { TestBed } from '@angular/core/testing';

import { FavoriteFacadeService } from './favorite-facade.service';

describe('FavoriteFacadeService', () => {
  let service: FavoriteFacadeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FavoriteFacadeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

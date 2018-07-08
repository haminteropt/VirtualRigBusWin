import { TestBed, inject } from '@angular/core/testing';

import { DirService } from './DirService.service';

describe('ListServicesService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DirService]
    });
  });

  it('should be created', inject([DirService], (service: DirService) => {
    expect(service).toBeTruthy();
  }));
});

import { TestBed, inject } from '@angular/core/testing';

import { ErrorNotifyService } from './error-notify-service';

describe('ErrorNotifyServiceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ErrorNotifyService]
    });
  });

  it('should ...', inject([ErrorNotifyService], (service: ErrorNotifyService) => {
    expect(service).toBeTruthy();
  }));
});

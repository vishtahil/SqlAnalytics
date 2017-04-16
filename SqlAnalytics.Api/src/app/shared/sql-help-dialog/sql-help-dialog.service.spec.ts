import { TestBed, inject } from '@angular/core/testing';

import { SqlHelpDialogService } from './sql-help-dialog.service';

describe('SqlHelpDialogService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SqlHelpDialogService]
    });
  });

  it('should ...', inject([SqlHelpDialogService], (service: SqlHelpDialogService) => {
    expect(service).toBeTruthy();
  }));
});

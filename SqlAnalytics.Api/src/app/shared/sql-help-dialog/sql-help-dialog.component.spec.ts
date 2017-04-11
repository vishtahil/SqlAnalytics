import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SqlHelpDialogComponent } from './sql-help-dialog.component';

describe('SqlHelpDialogComponent', () => {
  let component: SqlHelpDialogComponent;
  let fixture: ComponentFixture<SqlHelpDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SqlHelpDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SqlHelpDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

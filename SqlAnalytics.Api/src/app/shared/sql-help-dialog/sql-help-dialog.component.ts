import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { SqlHelpDialogService } from './sql-help-dialog.service';
import { SqlHelpContentModel } from './sql-help-dialog.model';
import { ErrorNotifyService } from '../../shared/error-notify-service';

@Component({
  selector: '[modal-partial]',
  templateUrl: './sql-help-dialog.component.html',
  styleUrls: ['./sql-help-dialog.component.css'],
  providers: [SqlHelpDialogService]
})
export class SqlHelpDialogComponent implements OnInit {
  _sqlHelpContentModel: SqlHelpContentModel[];

  @ViewChild('modal') modal;
  constructor(private sqlHelpDialogService: SqlHelpDialogService,
    private errorNotifyService: ErrorNotifyService) { }

  ngOnInit() {
  }

  show(sqlHelpCode: string) {
    this.sqlHelpDialogService.getSqlHelpContent(sqlHelpCode)
      .subscribe((response: SqlHelpContentModel[]) => {
        this._sqlHelpContentModel = response;
        this.modal.show();
        console.log(this._sqlHelpContentModel);
      },
      (error: any) => {
        this.errorNotifyService.error(error);
      });
  }

  showExecutionPlanHelp(sqlHelpCode: string) {
    this.sqlHelpDialogService.getSqlHelpContent(sqlHelpCode)
      .subscribe((response: SqlHelpContentModel[]) => {
        this._sqlHelpContentModel = response;
        this.modal.show();
        console.log(this._sqlHelpContentModel);
      },
      (error: any) => {
        this.errorNotifyService.error(error);
      });
  }
}

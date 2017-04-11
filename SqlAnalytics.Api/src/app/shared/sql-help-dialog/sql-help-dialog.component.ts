import { Component, OnInit , Input,ViewChild} from '@angular/core';

@Component({
  selector: '[modal-partial]',
  templateUrl: './sql-help-dialog.component.html',
  styleUrls: ['./sql-help-dialog.component.css']
})
export class SqlHelpDialogComponent implements OnInit {
  @ViewChild('modal') modal;
  constructor() { }

  ngOnInit() {
  }

  show(){
    this.modal.show();
  }
}

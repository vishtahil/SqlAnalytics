import { Component, Input, OnChanges, ViewChild, OnInit, EventEmitter, Output, SimpleChanges, ChangeDetectionStrategy } from '@angular/core';
import { SqlOptimizationHint } from './sql-analytics-model';
import { SqlHintRegexService } from './sql-hints-regex';

@Component({
  selector: 'sql-analytics-optimize',
  templateUrl: './sql-analytics-optimize-component.html',
  styles: [`
      .alert {cursor:pointer;}
  `],
  providers: [SqlHintRegexService],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SqlAnalyticsOptimizeComponent implements OnInit, OnChanges {
  @Input()
  sqlHintsModel: SqlOptimizationHint[];

  @Input()
  sqlStmt: string;

  @Output()
  onSqlHint = new EventEmitter<string>();

  @Input('title') title;
  @Input('size') size = 'md';
  @ViewChild('modal') modal;

  originalSql: string;

  constructor(private _sqlHintRegexService: SqlHintRegexService) {
  }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges) {
    this.originalSql = this.sqlStmt;
  }

  matchPattern(sqlHintModel: SqlOptimizationHint) {
    this.sqlStmt = this.originalSql; //restore with original
    let regex = new RegExp(sqlHintModel.MatchedExpression, 'gi');
    var matches = this.sqlStmt.match(regex);

    if(matches && matches.length>0){
      this.sqlStmt=this.sqlStmt.split(matches[0]).join(`<span style='color:red;font-weight:bold'>${matches[0]}</span>`)
    }
    
    this.onSqlHint.emit(this.sqlStmt);
  }
}



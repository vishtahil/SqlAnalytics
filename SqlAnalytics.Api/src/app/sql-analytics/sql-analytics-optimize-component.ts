import {Component, Input, OnChanges, OnInit,EventEmitter, Output, SimpleChanges,ChangeDetectionStrategy} from '@angular/core';
import {SqlOptimizationHint} from './sql-analytics-model';
import {SqlHintRegexService} from './sql-hints-regex';

@Component({
    selector: 'sql-analytics-optimize',
    templateUrl:'./sql-analytics-optimize-component.html',
    styles: [`
      .alert {cursor:pointer;}
      .modal .modal-header { 
        background-color: #d6e9c6;
        color:#3c763d;
        font-wight:bold;
      }
      .list-heading{color:green;}
  `],
    providers: [SqlHintRegexService],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class SqlAnalyticsOptimizeComponent implements OnInit, OnChanges{
    @Input()
    sqlHintsModel:SqlOptimizationHint[];
    
    @Input()
    sqlStmt:string;
    
    @Output()
    onSqlHint = new EventEmitter<string>();

    originalSql:string;
    
    constructor(private _sqlHintRegexService:SqlHintRegexService) {
    }
    
    ngOnInit() {
    }

    ngOnChanges(changes: SimpleChanges) {
     this.originalSql=this.sqlStmt;
    }
    
    matchPattern(sqlHintModel:SqlOptimizationHint){
      this.sqlStmt=this.originalSql; //restore with original
      let regex=new RegExp(sqlHintModel.MatchedExpression, 'gi');
      var matches=this.sqlStmt.match(regex);
     
        for(let match of matches){
          this.sqlStmt = this.sqlStmt.replace(match,`<span style='color:red;font-weight:bold'>${match}</span>`);
        }
        this.onSqlHint.emit(this.sqlStmt);
      
    }
}



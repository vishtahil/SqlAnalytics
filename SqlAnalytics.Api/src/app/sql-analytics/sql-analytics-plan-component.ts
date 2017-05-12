﻿//our root app component
import {Component, Input, OnChanges, OnInit, SimpleChanges,ChangeDetectionStrategy} from '@angular/core';
import {SqlPlanStatisticsModel,SqlPlanStats} from './sql-analytics-model';


@Component({
    selector: 'sql-analytics-plan',
    templateUrl:'./sql-analytics-plan-component.html',
    styles: [`
        .borderless td, .borderless th {border: none;}
        .planTable td{word-break: break-all;}
    `],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class SqlAnalyticsPlanComponent implements OnInit, OnChanges{
    @Input()
    sqlPlanModel:SqlPlanStatisticsModel;
    operationWarnings:string[]=["Table Scan","Clustered Index Scan", "Index Scan",
    "Key Lookup"];

    supportedHelpCodes: string[] = ["Hash Match", "Index Scan", "Table Scan", "Stream Aggregate",
        "Compute Scalar","Clustered Index Scan", "Sort", "Merge Join", "Nested Loops", "Key Lookup", "Index Seek"];
   

    planTd: any[] = [];
   

    constructor() {
    }
    
    ngOnInit() {
    }

    ngOnChanges(changes: SimpleChanges) {
    }
    
    showAlert(colValue:SqlPlanStats){
      if(this.operationWarnings.indexOf(colValue.PhysicalOperation)>-1 ||
             this.operationWarnings.indexOf(colValue.LogicalOperation)>-1
             || !!colValue.NodeWarning.Key){
             return true;
             }
      return false;
    }

    showHelpCodes(colValue: SqlPlanStats) {
        if (this.supportedHelpCodes.indexOf(colValue.PhysicalOperation) > -1) {
            return true;
        }
        return false;
    }
    
    setClasses(colValue:SqlPlanStats) {
        var alertShow=this.showAlert(colValue);
        let classes =  {
            'alert': alertShow,    
            'alert-danger': alertShow, 
        };
        return classes;
    }
}



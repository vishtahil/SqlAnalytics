//our root app component
import {Component, Input, OnChanges, OnInit, SimpleChanges,ChangeDetectionStrategy} from '@angular/core';
import {SqlPlanStatisticsModel} from './sql-analytics-model';

@Component({
    selector: 'sql-analytics-plan',
    templateUrl:'./sql-analytics-plan-component.html',
    styles: [``],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class SqlAnalyticsPlanComponent implements OnInit, OnChanges{
    @Input()
    sqlPlanModel:SqlPlanStatisticsModel[];
    operationWarnings:string[]=["Table Scan","Clustered Index Scan", "Key Lookup"]
    constructor() {
    }
    
    ngOnInit() {
    }

    ngOnChanges(changes: SimpleChanges) {
    }
    
    showAlert(colValue:SqlPlanStatisticsModel){
      if(this.operationWarnings.indexOf(colValue.PhysicalOperation)>-1 ||
             this.operationWarnings.indexOf(colValue.LogicalOperation)>-1){
             return true;
             }
      return false;
    }
    
    setClasses(colValue:SqlPlanStatisticsModel) {
        console.log("whats happening");
        var alertShow=this.showAlert(colValue);
        let classes =  {
            'alert': alertShow,    
            'alert-danger': alertShow, 
        };
        return classes;
    }
}



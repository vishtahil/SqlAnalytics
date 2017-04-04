//our root app component
import {Component, Input, OnChanges, OnInit, SimpleChanges,ChangeDetectionStrategy} from '@angular/core';
import {SqlPlanOveriviewModel} from './sql-analytics-model';
import { WindowRef } from './../shared/windows-wrapper';

@Component({
    selector: 'sql-analytics-overview',
    templateUrl:'./sql-analytics-overview-component.html',
    styles: [``],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class SqlAnalyticsOverviewComponent implements OnInit, OnChanges {
    @Input()
    sqlOverViewModel:SqlPlanOveriviewModel;

    ngOnInit() {
    }

    ngOnChanges(changes: SimpleChanges) {
    }
    
    
    constructor(private winRef:WindowRef) {
    }
  
}
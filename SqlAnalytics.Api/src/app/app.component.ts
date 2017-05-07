import { Component,OnDestroy } from '@angular/core';
import {SqlAnalyticsComponent} from './sql-analytics/sql-analytics-component';
import {ErrorNotifyService,} from './shared/error-notify-service'
import { Subscription }   from 'rxjs/Subscription';
import Utilities from '../app/shared/utilities';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent  implements OnDestroy{
   alertClosed:boolean=true;
   alertMessage:string;
   subscription:Subscription;
   sqlModes:string[]=[Utilities.Constants.EXECUTION_PLAN_MODE,
   Utilities.Constants.SQL_LINT_MODE,
   Utilities.Constants.SQL_MODE];
   selectedSqlMode: string;

   constructor(private errorNotifyService:ErrorNotifyService){
   }

   setSqlMode(mode:string){
     this.selectedSqlMode=mode;
   }

   ngOnInit() {
      this.subscription = this.errorNotifyService.errorGenerated$
       .subscribe(exceptionMessage => {
         this.alertClosed=false;
         this.alertMessage=exceptionMessage;
        });
   }

   ngOnDestroy() {
    // prevent memory leak when component destroyed
    this.subscription.unsubscribe();
  }

}

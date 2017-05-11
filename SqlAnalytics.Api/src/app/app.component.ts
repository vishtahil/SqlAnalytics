import { Component,OnDestroy } from '@angular/core';
import {SqlAnalyticsComponent} from './sql-analytics/sql-analytics-component';
import {ErrorNotifyService,} from './shared/error-notify-service'
import { Subscription }   from 'rxjs/Subscription';
import Utilities from '../app/shared/utilities';
import { StorageRef } from './shared/local-storage-wrapper'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent  implements OnDestroy{
   alertClosed:boolean=true;
   alertMessage:string;
   subscription:Subscription;
   sqlModes=[Utilities.Constants.EXECUTION_PLAN_MODE,
            Utilities.Constants.SQL_LINT_MODE,
             Utilities.Constants.SQL_MODE];
   selectedSqlMode: string;

   constructor(private errorNotifyService:ErrorNotifyService, private storage: StorageRef){
    this.selectedSqlMode=this.storage.getItem(Utilities.Constants.SQL_MODE) || '';
  }

  onClearError() {
    this.alertClosed=true;
    console.log("on clear error");
  }

   setSqlMode(mode:string){
     this.selectedSqlMode=mode;
     this.storage.setItem(Utilities.Constants.SQL_MODE,this.selectedSqlMode);
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

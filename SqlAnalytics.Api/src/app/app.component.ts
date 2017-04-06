import { Component,OnDestroy } from '@angular/core';
import {SqlAnalyticsComponent} from './sql-analytics/sql-analytics-component';
import {ErrorNotifyService,} from './shared/error-notify-service'
import { Subscription }   from 'rxjs/Subscription';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent  implements OnDestroy{
   alertClosed:boolean=true;
   alertMessage:string;
   subscription:Subscription;

   constructor(private errorNotifyService:ErrorNotifyService){
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

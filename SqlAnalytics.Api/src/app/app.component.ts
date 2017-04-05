import { Component,OnDestroy } from '@angular/core';
import {SqlAnalyticsComponent} from './sql-analytics/sql-analytics-component';
import {ErrorNotifyService,} from './shared/error-notify-service'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent  {
   alertClosed:boolean=true;
   alertMessage:string;

   constructor(private errorNotifyService:ErrorNotifyService){
        this.errorNotifyService.errorGenerated$.subscribe(error=>{
          console.log(error);
          this.alertClosed=false;
          //this.alertMessage=error;
        });
   }

}

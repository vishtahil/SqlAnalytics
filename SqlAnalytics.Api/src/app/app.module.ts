//our root app component
import {Component, NgModule, ChangeDetectionStrategy,ErrorHandler} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {FormsModule} from '@angular/forms';
import {HttpModule} from '@angular/http';
import {AppComponent} from './app.component'
import {SqlAnalyticsComponent} from './sql-analytics/sql-analytics-component';
import {SqlAnalyticsOverviewComponent} from './sql-analytics/sql-analytics-overview-component';
import {SqlAnalyticsPlanComponent} from './sql-analytics/sql-analytics-plan-component';
import {SqlAnalyticsOptimizeComponent} from './sql-analytics/sql-analytics-optimize-component';
import { WindowRef } from './shared/windows-wrapper';
import {StorageRef} from './shared/local-storage-wrapper'
import { TabsModule,AlertModule ,ModalModule,TooltipModule   } from 'ngx-bootstrap';
import { SanitizeHtml } from './shared/sanitize-html.pipe';
import {ErrorNotifyService} from './shared/error-notify-service';
import GlobalErrorHandler from './shared/global-error-handler'


@NgModule({
    imports: [BrowserModule, FormsModule,HttpModule,
     TabsModule.forRoot(), AlertModule.forRoot(),
     ModalModule.forRoot(),TooltipModule.forRoot()],
    declarations: [AppComponent,SanitizeHtml,SqlAnalyticsComponent,SqlAnalyticsPlanComponent,
    SqlAnalyticsOverviewComponent,SqlAnalyticsOptimizeComponent],
    bootstrap: [AppComponent],
    providers: [ WindowRef,
    StorageRef,
    ErrorNotifyService,
    {provide:ErrorHandler,useClass:GlobalErrorHandler} ]
})
export class AppModule { }


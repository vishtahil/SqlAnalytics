//our root app component
import {Component, NgModule, ChangeDetectionStrategy} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {FormsModule} from '@angular/forms';
import {HttpModule} from '@angular/http';
import {AppComponent} from './app.component'
import {SqlAnalyticsComponent} from './sql-analytics/sql-analytics-component';
import {SqlAnalyticsOverviewComponent} from './sql-analytics/sql-analytics-overview-component';
import {SqlAnalyticsPlanComponent} from './sql-analytics/sql-analytics-plan-component';
import {SqlAnalyticsOptimizeComponent} from './sql-analytics/sql-analytics-optimize-component';
import { WindowRef } from './shared/windows-wrapper';
import { TabsModule } from 'ng2-bootstrap';
import { SanitizeHtml } from './shared/sanitize-html.pipe';


@NgModule({
    imports: [BrowserModule, FormsModule,HttpModule, TabsModule.forRoot()],
    declarations: [AppComponent,SanitizeHtml,SqlAnalyticsComponent,SqlAnalyticsPlanComponent,
    SqlAnalyticsOverviewComponent,SqlAnalyticsOptimizeComponent],
    bootstrap: [AppComponent],
    providers: [ WindowRef ]
})
export class AppModule { }


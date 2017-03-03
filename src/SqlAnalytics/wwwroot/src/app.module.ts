//our root app component
import {Component, NgModule, ChangeDetectionStrategy} from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

// import ng2-bootstrap pagination module
import { PaginationModule } from 'ng2-bootstrap';

@Component({
    selector: 'my-app',
    changeDetection: ChangeDetectionStrategy.OnPush,
    template: `
    <h4>Pager</h4>
    <div style="margin: 20px 0;">
      <pagination [directionLinks]="false" [totalItems]="totalPagerItems" [(ngModel)]="currentPagerPage" (numPages)="smallnumPagerPages = $event"></pagination>
    </div>

    <div>
      <pager [totalItems]="totalPagerItems" [(ngModel)]="currentPagerPage" (pageChanged)="pageChanged($event)" pageBtnClass="btn"
        itemsPerPage="10"></pager>
    </div>
  `,
})
export class AppComponent {
    public totalPagerItems = 64;
    public currentPagerPage = 3;
    public smallnumPagerPages = 0;

    constructor() {
    }

    public pageChanged(event: any): void {
        console.log('Page changed to: ' + event.page);
        console.log('Number items per page: ' + event.itemsPerPage);
    }

}

@NgModule({
    imports: [BrowserModule, PaginationModule.forRoot(), FormsModule],
    declarations: [AppComponent],
    bootstrap: [AppComponent]
})
export class AppModule { }
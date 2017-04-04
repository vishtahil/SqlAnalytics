import { Injectable }     from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import {Observable} from 'rxjs/Rx';

// Import RxJs required methods
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import {SqlStatisticsSummary} from './sql-analytics-model';
import {environment} from '../../environments/environment'

//https://scotch.io/tutorials/angular-2-http-requests-with-observables
@Injectable()
export class SqlAnalyticsService {
  private _analyticsUrl = `${environment.API_URL}analytics/analytics`; 

  constructor (private http: Http) {}
   
    getSqlSqlStats (sqlModel: any): Observable<SqlStatisticsSummary> {
        
        let bodyString = JSON.stringify(sqlModel); // Stringify payload
        let headers      = new Headers({ 'Content-Type': 'application/json' }); // ... Set content type to JSON
        let options       = new RequestOptions({ headers: headers }); // Create a request option
        return this.http.post(this._analyticsUrl, bodyString, options) // ...using post request
                         .map((res:Response) => res.json()) // ...and calling .json() on the response to return data
                         .catch((error:any) => Observable.throw(error.json().error || 'Server error')); //...errors if any
    }   
}
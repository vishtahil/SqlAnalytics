import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Rx';
// Import RxJs required methods
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import { environment } from '../../../environments/environment';
import {SqlHelpContentModel} from './sql-help-dialog.model';


@Injectable()
export class SqlHelpDialogService {
  private _helpUrl = `${environment.API_URL}analytics/help`;

  constructor(private http: Http) { }

  getSqlHelpContent (helpCode: string): Observable<SqlHelpContentModel[]> {
        let headers      = new Headers({ 'Content-Type': 'application/json' }); // ... Set content type to JSON
        let options       = new RequestOptions({ headers: headers }); // Create a request option
        return this.http.get(`${this._helpUrl}/${helpCode}`, options) // ...using post request
                         .map((res:Response) => res.json()) // ...and calling .json() on the response to return data
                         .catch((error:any) =>
                         {
                           return Observable.throw(error);}
                         ); //...errors if any
    }   
}

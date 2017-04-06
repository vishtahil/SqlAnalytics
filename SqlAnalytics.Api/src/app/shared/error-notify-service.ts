import { Injectable } from '@angular/core';
import { Subject }    from 'rxjs/Subject';
import { Observable }    from 'rxjs/Observable';
import { Observer }    from 'rxjs/Observer';
import { BehaviorSubject }    from 'rxjs/BehaviorSubject';
import 'rxjs/add/operator/share';

@Injectable()
export class ErrorNotifyService {
  private errorEventSource=new Subject<any>();
  errorGenerated$=this.errorEventSource.asObservable();

  constructor() {

  } 
  error(exception:any){
    this.errorEventSource.next(exception.json().error);
  }

}

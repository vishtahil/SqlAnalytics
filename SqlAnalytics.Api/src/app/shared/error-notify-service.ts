import { Injectable } from '@angular/core';
import { Subject }    from 'rxjs/Subject';

@Injectable()
export class ErrorNotifyService {
  private errorEventSource=new Subject<any>();
  errorGenerated$=this.errorEventSource.asObservable();

  constructor() { } 

  error(exception:any){
    console.log("whats happening");
    console.log(exception);
    this.errorEventSource.next(exception);
  }

}

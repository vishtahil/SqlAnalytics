import { Injectable } from '@angular/core';

@Injectable()
export class StorageRef {
   
  public setItem( key:string, data:string) {
   localStorage.setItem(key,data);
  }

  public getItem( key:string) {
   return localStorage.getItem(key);
  }
    
}
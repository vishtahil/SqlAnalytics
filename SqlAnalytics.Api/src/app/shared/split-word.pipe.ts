import { Pipe, PipeTransform } from '@angular/core';

@Pipe({name: 'splitword'})
export class SplitWordPipe implements PipeTransform {
  transform(value: string, args: string[]): any {
    if (!value) return value;

    return value.replace(/([a-z])([A-Z])/g, '$1 $2')
  }
}

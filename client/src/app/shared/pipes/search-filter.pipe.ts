import {Pipe, PipeTransform} from '@angular/core';

@Pipe({name: 'searchFilter'})
export class SearchFilterPipe implements PipeTransform {
  transform(value: any, search: string): any {
    debugger;
    if (!search) {
      return value;
    }
    return value.filter((v: any) => {
      if (!v?.name) {
        return;
      }
      return v.name.toLowerCase().indexOf(search.toLowerCase()) !== -1;
    });
  }
}

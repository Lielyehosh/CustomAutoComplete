import { Pipe, PipeTransform } from '@angular/core';

@Pipe({name: 'letterBold'})
export class LetterBoldPipe implements PipeTransform {
  transform(value: any, search: any): any {
    if (!search) { return value; }
    let searchLength = search.length;
    let  holder = value.split('');
    let indexAdder = 0;
    let indexes = this.locations(search.toLowerCase(), value.toLowerCase() )
    indexes = indexes.map( (x: any) => {
      let solution =  x + indexAdder;
      indexAdder += 2;
      return solution;
    })
    indexes.forEach( (i: any) => {
      holder.splice(i, 0 , "<span class = 'bold'>")
      holder.splice(i + searchLength +1, 0,  "</span>")
    })
    return holder.join('');
  }

  locations(substring: any, searchValue: any) {
    let indexes = [], i = -1;
    while((i = searchValue.indexOf(substring, i + 1 )) >= 0) {
      indexes.push(i);
    }
    return indexes;
  }
}

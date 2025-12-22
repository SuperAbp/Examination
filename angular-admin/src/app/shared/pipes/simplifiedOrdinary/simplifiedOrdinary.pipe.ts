import { Pipe, PipeTransform } from '@angular/core';
import { simplifiedOrdinary } from '@shared';

@Pipe({
    name: 'simplifiedOrdinary',
    standalone: false
})
export class SimplifiedOrdinaryPipe implements PipeTransform {
  transform(value: number): string {
    return simplifiedOrdinary(value);
  }
}

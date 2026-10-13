import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'price' })
export class PricePipe implements PipeTransform {
  transform(value: string | number | null | undefined): string {
    if (value === null || value === undefined || value === '') {
      return '—';
    }

    const numberValue = Number(value);
    return Number.isNaN(numberValue) ? `₪${value}` : `₪${numberValue.toFixed(2)}`;
  }
}

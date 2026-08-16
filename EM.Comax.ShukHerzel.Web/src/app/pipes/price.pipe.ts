import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'price' })
export class PricePipe implements PipeTransform {
  transform(value: string | null): string {
    if (!value) {
      return '—';
    }

    const numberValue = Number(value);
    return Number.isNaN(numberValue) ? `₪${value}` : `₪${numberValue.toFixed(2)}`;
  }
}

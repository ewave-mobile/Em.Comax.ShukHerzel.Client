import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'comaxDate' })
export class ComaxDatePipe implements PipeTransform {
  transform(value: string | null): string {
    if (!value?.trim()) {
      return '—';
    }

    const raw = value.trim();
    const match = raw.match(/^(\d{1,2})[\/\-.](\d{1,2})[\/\-.](\d{2,4})/);
    if (match) {
      const day = match[1].padStart(2, '0');
      const month = match[2].padStart(2, '0');
      const year = match[3].length === 2 ? `20${match[3]}` : match[3];
      return `${day}/${month}/${year}`;
    }

    const date = new Date(raw);
    if (Number.isNaN(date.getTime())) {
      return raw;
    }

    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    return `${day}/${month}/${date.getFullYear()}`;
  }
}

export function todayKey(): string {
  const date = new Date();
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  return `${year}-${month}-${day}`;
}

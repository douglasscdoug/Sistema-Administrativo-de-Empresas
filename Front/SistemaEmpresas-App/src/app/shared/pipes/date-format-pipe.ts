import { formatDate } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dateFormat',
  standalone: true
})
export class DateFormatPipe implements PipeTransform {

  transform(value: Date | string | null, formato: string = 'dd/MM/yyyy HH:mm'): string {
    if (!value) return '-';

    try {
      let valor = value.toString();

      valor = valor.replace(/\.(\d{3})\d+/, '.$1');

      if (!valor.includes('Z')) valor += 'Z';

      const data = new Date(valor);

      if (isNaN(data.getTime())) return '-';

      return formatDate(data, formato, 'pt-BR');
    } catch {
      return '-';
    }
  }
}
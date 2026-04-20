import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'firstKey',
})
export class FirstKeyPipe implements PipeTransform {
  transform(value: any, ...args: unknown[]): string | null {
    const keys = Object.keys(value);
    if (keys && keys.length > 0)
      return keys[0];
    return null;
  }
}
